namespace AzureImage.Inference.Models;

/// <summary>
/// Interface for image generation models
/// </summary>
public interface IImageGenerationModel : IImageModel
{
    /// <summary>
    /// Gets the endpoint path for image generation for this model
    /// </summary>
    string ImageGenerationEndpoint { get; }

    /// <summary>
    /// Gets the timeout for requests to this model
    /// </summary>
    TimeSpan Timeout { get; }

    /// <summary>
    /// Gets the maximum retry attempts for this model
    /// </summary>
    int MaxRetryAttempts { get; }

    /// <summary>
    /// Gets the retry delay for this model
    /// </summary>
    TimeSpan RetryDelay { get; }
} 