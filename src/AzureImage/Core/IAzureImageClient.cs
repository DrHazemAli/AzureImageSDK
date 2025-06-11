using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

    /// <summary>
    /// Generates a caption for the provided image stream using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageStream">The image stream to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated caption result</returns>
    Task<ImageCaptionResult> GenerateCaptionAsync(
        IImageCaptioningModel model,
        Stream imageStream,
        ImageCaptionOptions options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a caption for the provided image URL using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageUrl">The image URL to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated caption result</returns>
    Task<ImageCaptionResult> GenerateCaptionAsync(
        IImageCaptioningModel model,
        string imageUrl,
        ImageCaptionOptions options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates dense captions for multiple regions in the provided image stream using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageStream">The image stream to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated dense caption results</returns>
    Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
        IImageCaptioningModel model,
        Stream imageStream,
        ImageCaptionOptions options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates dense captions for multiple regions in the provided image URL using the specified captioning model
    /// </summary>
    /// <param name="model">The image captioning model to use</param>
    /// <param name="imageUrl">The image URL to caption</param>
    /// <param name="options">Optional captioning options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The generated dense caption results</returns>
    Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
        IImageCaptioningModel model,
        string imageUrl,
        ImageCaptionOptions options = null,
        CancellationToken cancellationToken = default);
} 