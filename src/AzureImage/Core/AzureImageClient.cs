using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AzureImage.Core.Services;
using AzureImage.Inference.Models;
using Microsoft.Extensions.Logging;

namespace AzureImage.Core;

/// <summary>
/// Main client implementation for Azure Image SDK
/// </summary>
public class AzureImageClient : IAzureImageClient, IDisposable
{
    private readonly ModelHttpClientService _modelHttpClientService;
    private readonly ILogger<AzureImageClient>? _logger;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureImageClient"/> class
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    public AzureImageClient(ILoggerFactory? loggerFactory = null)
    {
        _logger = loggerFactory?.CreateLogger<AzureImageClient>();
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
    /// Generates a caption for the provided image stream using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageStream">The image stream to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated caption result</returns>
    public async Task<ImageCaptionResult> GenerateCaptionAsync(
        IImageCaptioningModel model,
        Stream imageStream,
        ImageCaptionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (imageStream == null)
            throw new ArgumentNullException(nameof(imageStream));

        _logger?.LogInformation("Generating caption for image stream");

        try
        {
            var result = await model.GenerateCaptionAsync(imageStream, options, cancellationToken);
            _logger?.LogInformation("Caption generation completed successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Caption generation failed");
            throw;
        }
    }

    /// <summary>
    /// Generates a caption for the provided image URL using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageUrl">The image URL to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated caption result</returns>
    public async Task<ImageCaptionResult> GenerateCaptionAsync(
        IImageCaptioningModel model,
        string imageUrl,
        ImageCaptionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));

        _logger?.LogInformation("Generating caption for image URL: {ImageUrl}", imageUrl);

        try
        {
            var result = await model.GenerateCaptionAsync(imageUrl, options, cancellationToken);
            _logger?.LogInformation("Caption generation completed successfully for URL: {ImageUrl}", imageUrl);
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Caption generation failed for URL: {ImageUrl}", imageUrl);
            throw;
        }
    }

    /// <summary>
    /// Generates dense captions for multiple regions in the provided image stream using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageStream">The image stream to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated dense caption results</returns>
    public async Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
        IImageCaptioningModel model,
        Stream imageStream,
        ImageCaptionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (imageStream == null)
            throw new ArgumentNullException(nameof(imageStream));

        _logger?.LogInformation("Generating dense captions for image stream");

        try
        {
            var result = await model.GenerateDenseCaptionsAsync(imageStream, options, cancellationToken);
            _logger?.LogInformation("Dense caption generation completed successfully with {CaptionCount} captions", 
                result.Captions?.Count ?? 0);
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Dense caption generation failed");
            throw;
        }
    }

    /// <summary>
    /// Generates dense captions for multiple regions in the provided image URL using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageUrl">The image URL to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated dense caption results</returns>
    public async Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
        IImageCaptioningModel model,
        string imageUrl,
        ImageCaptionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));

        _logger?.LogInformation("Generating dense captions for image URL: {ImageUrl}", imageUrl);

        try
        {
            var result = await model.GenerateDenseCaptionsAsync(imageUrl, options, cancellationToken);
            _logger?.LogInformation("Dense caption generation completed successfully for URL: {ImageUrl} with {CaptionCount} captions", 
                imageUrl, result.Captions?.Count ?? 0);
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Dense caption generation failed for URL: {ImageUrl}", imageUrl);
            throw;
        }
    }

    /// <summary>
    /// Creates a new Azure Image client
    /// </summary>
    /// <param name="loggerFactory">The logger factory (optional)</param>
    /// <returns>A new Azure Image client instance</returns>
    public static AzureImageClient Create(ILoggerFactory? loggerFactory = null)
    {
        return new AzureImageClient(loggerFactory);
    }

    /// <summary>
    /// Disposes the Azure Image client and releases resources
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