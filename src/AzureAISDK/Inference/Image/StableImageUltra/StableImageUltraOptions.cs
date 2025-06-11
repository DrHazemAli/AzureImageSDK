using System.ComponentModel.DataAnnotations;

namespace AzureAISDK.Inference.Image.StableImageUltra;

/// <summary>
/// Configuration options for StableImageUltra model
/// </summary>
public class StableImageUltraOptions
{
    /// <summary>
    /// Gets or sets the endpoint URL for the StableImageUltra deployment
    /// </summary>
    [Required]
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for authentication
    /// </summary>
    [Required]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version for StableImageUltra
    /// </summary>
    public string ApiVersion { get; set; } = "2024-05-01-preview";

    /// <summary>
    /// Gets or sets the model name/version
    /// </summary>
    public string ModelName { get; set; } = "Stable-Image-Ultra";

    /// <summary>
    /// Gets or sets the default image size
    /// </summary>
    public string DefaultSize { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the default output format
    /// </summary>
    public string DefaultOutputFormat { get; set; } = "png";

    /// <summary>
    /// Gets or sets the timeout for requests to this model
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for this model
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the retry delay for this model
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Validates the configuration options
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required options are missing or invalid</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Endpoint))
            throw new ArgumentException("Endpoint is required", nameof(Endpoint));

        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new ArgumentException("ApiKey is required", nameof(ApiKey));

        if (!Uri.IsWellFormedUriString(Endpoint, UriKind.Absolute))
            throw new ArgumentException("Endpoint must be a valid absolute URI", nameof(Endpoint));

        if (string.IsNullOrWhiteSpace(ModelName))
            throw new ArgumentException("ModelName is required", nameof(ModelName));

        if (Timeout <= TimeSpan.Zero)
            throw new ArgumentException("Timeout must be greater than zero", nameof(Timeout));

        if (MaxRetryAttempts < 0)
            throw new ArgumentException("MaxRetryAttempts must be non-negative", nameof(MaxRetryAttempts));

        if (RetryDelay < TimeSpan.Zero)
            throw new ArgumentException("RetryDelay must be non-negative", nameof(RetryDelay));
    }
} 