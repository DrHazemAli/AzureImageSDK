namespace AzureImage.Inference.Models;

/// <summary>
/// Base interface for all image models
/// </summary>
public interface IImageModel
{
    /// <summary>
    /// Gets the model name/identifier
    /// </summary>
    string ModelName { get; }

    /// <summary>
    /// Gets the endpoint for this model
    /// </summary>
    string Endpoint { get; }

    /// <summary>
    /// Gets the API version for this model
    /// </summary>
    string ApiVersion { get; }

    /// <summary>
    /// Gets the API key for authentication
    /// </summary>
    string ApiKey { get; }
} 