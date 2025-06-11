using System;

namespace AzureImage.Inference.Models;

/// <summary>
/// Interface for image editing models
/// </summary>
public interface IImageEditingModel : IImageModel
{
    /// <summary>
    /// Gets the endpoint path for image editing for this model
    /// </summary>
    string ImageEditingEndpoint { get; }

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