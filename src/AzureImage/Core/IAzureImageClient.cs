using AzureImage.Inference.Models;

namespace AzureImage.Core;

/// <summary>
/// Main client interface for Azure Image SDK providing access to all image services
/// </summary>
public interface IAzureImageClient
{
    /// <summary>
    /// Generates an image using the specified image generation model and request
    /// </summary>
    /// <typeparam name="TRequest">The type of the model-specific request</typeparam>
    /// <typeparam name="TResponse">The type of the model-specific response</typeparam>
    /// <param name="model">The image generation model to use</param>
    /// <param name="request">The model-specific request object</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The model-specific response</returns>
    Task<TResponse> GenerateImageAsync<TRequest, TResponse>(
        IImageGenerationModel model,
        TRequest request,
        CancellationToken cancellationToken = default);
} 