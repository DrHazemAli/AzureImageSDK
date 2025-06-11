using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AzureImage.Inference.Models
{
    /// <summary>
    /// Interface for image captioning models that can generate text descriptions from images.
    /// </summary>
    public interface IImageCaptioningModel
    {
        /// <summary>
        /// Generates a caption for the provided image.
        /// </summary>
        /// <param name="imageStream">The image stream to caption</param>
        /// <param name="options">Optional captioning options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The generated caption result</returns>
        Task<ImageCaptionResult> GenerateCaptionAsync(
            Stream imageStream,
            ImageCaptionOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a caption for the provided image URL.
        /// </summary>
        /// <param name="imageUrl">The image URL to caption</param>
        /// <param name="options">Optional captioning options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The generated caption result</returns>
        Task<ImageCaptionResult> GenerateCaptionAsync(
            string imageUrl,
            ImageCaptionOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates dense captions for multiple regions in the provided image.
        /// </summary>
        /// <param name="imageStream">The image stream to caption</param>
        /// <param name="options">Optional captioning options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The generated dense caption results</returns>
        Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
            Stream imageStream,
            ImageCaptionOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates dense captions for multiple regions in the provided image URL.
        /// </summary>
        /// <param name="imageUrl">The image URL to caption</param>
        /// <param name="options">Optional captioning options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The generated dense caption results</returns>
        Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
            string imageUrl,
            ImageCaptionOptions options = null,
            CancellationToken cancellationToken = default);
    }
} 