using AzureAISDK.Core.Services;
using AzureAISDK.Inference.Image;
using Microsoft.Extensions.Logging;

namespace AzureAISDK.Core;

/// <summary>
/// Main client implementation for Azure AI SDK
/// </summary>
public class AzureAIClient : IAzureAIClient, IDisposable
{
    private readonly ModelHttpClientService _modelHttpClientService;
    private readonly ILogger<AzureAIClient>? _logger;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureAIClient"/> class
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    public AzureAIClient(ILoggerFactory? loggerFactory = null)
    {
        _logger = loggerFactory?.CreateLogger<AzureAIClient>();
        var modelLogger = loggerFactory?.CreateLogger<ModelHttpClientService>();
        _modelHttpClientService = new ModelHttpClientService(logger: modelLogger);
    }

    /// <summary>
    /// Generates an image using the specified image generation model and request
    /// </summary>
    /// <typeparam name="TRequest">The type of the model-specific request</typeparam>
    /// <typeparam name="TResponse">The type of the model-specific response</typeparam>
    /// <param name="model">The image generation model to use</param>
    /// <param name="request">The model-specific request object</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The model-specific response</returns>
    public async Task<TResponse> GenerateImageAsync<TRequest, TResponse>(
        IImageGenerationModel model,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        _logger?.LogInformation("Generating image with model {ModelName}", model.ModelName);

        try
        {
            var response = await _modelHttpClientService.PostAsync<TRequest, TResponse>(
                model, 
                request, 
                cancellationToken);

            _logger?.LogInformation("Image generation completed successfully with model {ModelName}", 
                model.ModelName);
            
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Image generation failed with model {ModelName}", model.ModelName);
            throw;
        }
    }

    /// <summary>
    /// Creates a new Azure AI client
    /// </summary>
    /// <param name="loggerFactory">The logger factory (optional)</param>
    /// <returns>A new Azure AI client instance</returns>
    public static AzureAIClient Create(ILoggerFactory? loggerFactory = null)
    {
        return new AzureAIClient(loggerFactory);
    }

    /// <summary>
    /// Disposes the Azure AI client and releases resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _modelHttpClientService?.Dispose();
            _disposed = true;
        }
    }
} 