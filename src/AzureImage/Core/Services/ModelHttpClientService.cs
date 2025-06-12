using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AzureImage.Core.Exceptions;
using AzureImage.Inference.Models;
using Microsoft.Extensions.Logging;

namespace AzureImage.Core.Services;

/// <summary>
/// HTTP client service for making requests to AI models
/// </summary>
public class ModelHttpClientService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ModelHttpClientService>? _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelHttpClientService"/> class
    /// </summary>
    /// <param name="httpClient">The HTTP client to use</param>
    /// <param name="logger">The logger</param>
    public ModelHttpClientService(HttpClient? httpClient = null, ILogger<ModelHttpClientService>? logger = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    /// <summary>
    /// Sends a POST request to the specified model for image generation
    /// </summary>
    /// <typeparam name="TRequest">The type of the request</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
    /// <param name="model">The image generation model</param>
    /// <param name="request">The request body</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The deserialized response</returns>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        IImageGenerationModel model,
        TRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var requestUri = model.ImageGenerationEndpoint;
        var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await SendWithRetryAsync(
            HttpMethod.Post, 
            model, 
            requestUri, 
            content, 
            cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(responseContent))
            throw new AzureImageException("Empty response received from server");

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions) 
                ?? throw new AzureImageException("Failed to deserialize response");
        }
        catch (JsonException ex)
        {
            throw new AzureImageException("Failed to deserialize response", ex);
        }
    }

    /// <summary>
    /// Sends an HTTP request with retry logic
    /// </summary>
    /// <param name="method">The HTTP method</param>
    /// <param name="model">The model configuration</param>
    /// <param name="requestUri">The request URI</param>
    /// <param name="content">The request content</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The HTTP response message</returns>
    private async Task<HttpResponseMessage> SendWithRetryAsync(
        HttpMethod method,
        IImageGenerationModel model,
        string requestUri,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        var uri = new Uri(new Uri(model.Endpoint), requestUri);
        var uriBuilder = new UriBuilder(uri);
        
        // Add API version query parameter
        if (string.IsNullOrEmpty(uriBuilder.Query))
        {
            uriBuilder.Query = $"api-version={Uri.EscapeDataString(model.ApiVersion)}";
        }
        else
        {
            var existingQuery = uriBuilder.Query.TrimStart('?');
            uriBuilder.Query = $"{existingQuery}&api-version={Uri.EscapeDataString(model.ApiVersion)}";
        }

        var finalUri = uriBuilder.Uri;

        Exception? lastException = null;
        
        // Create a cancellation token that respects the model's timeout
        using var timeoutCts = new CancellationTokenSource(model.Timeout);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        
        for (int attempt = 0; attempt <= model.MaxRetryAttempts; attempt++)
        {
            try
            {
                using var request = new HttpRequestMessage(method, finalUri);
                
                // Set headers per-request to avoid conflicts between different models
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", model.ApiKey);
                request.Headers.Add("User-Agent", "AzureImage/1.0.1");
                
                if (content != null)
                {
                    request.Content = content;
                }

                _logger?.LogDebug("Sending {Method} request to {Uri} (attempt {Attempt})", 
                    method, finalUri, attempt + 1);

                var response = await _httpClient.SendAsync(request, combinedCts.Token);

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogDebug("Request successful with status {StatusCode}", response.StatusCode);
                    return response;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = $"Request failed with status {response.StatusCode}: {errorContent}";
                
                _logger?.LogWarning("Request failed with status {StatusCode}: {ErrorContent}", 
                    response.StatusCode, errorContent);

                // Don't retry on client errors (4xx), only on server errors (5xx) and network issues
                if ((int)response.StatusCode < 500)
                {
                    throw new AzureImageException(errorMessage, null, (int)response.StatusCode);
                }

                lastException = new AzureImageException(errorMessage, null, (int)response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                lastException = new AzureImageException("Network error occurred", ex);
                _logger?.LogWarning(ex, "Network error occurred (attempt {Attempt})", attempt + 1);
            }
            catch (OperationCanceledException ex) when (timeoutCts.Token.IsCancellationRequested)
            {
                lastException = new AzureImageException($"Request timed out after {model.Timeout}", ex);
                _logger?.LogWarning(ex, "Request timed out after {Timeout} (attempt {Attempt})", model.Timeout, attempt + 1);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw; // Re-throw cancellation from the original token
            }
            catch (AzureImageException)
            {
                throw; // Re-throw Azure AI exceptions without retry
            }

            // Don't delay after the last attempt
            if (attempt < model.MaxRetryAttempts)
            {
                var delay = TimeSpan.FromMilliseconds(model.RetryDelay.TotalMilliseconds * Math.Pow(2, attempt));

                _logger?.LogDebug("Waiting {Delay}ms before retry", delay.TotalMilliseconds);
                await Task.Delay(delay, combinedCts.Token);
            }
        }

        throw lastException ?? new AzureImageException("Request failed after all retry attempts");
    }

    /// <summary>
    /// Disposes the HTTP client service
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClient?.Dispose();
            _disposed = true;
        }
    }
} 