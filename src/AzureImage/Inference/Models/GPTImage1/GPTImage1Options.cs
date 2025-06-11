using System;
using System.ComponentModel.DataAnnotations;

namespace AzureImage.Inference.Models.GPTImage1;

/// <summary>
/// Configuration options for GPT-Image-1 model
/// </summary>
public class GPTImage1Options
{
    /// <summary>
    /// Gets or sets the endpoint URL for the GPT-Image-1 deployment
    /// </summary>
    [Required]
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for authentication
    /// </summary>
    [Required]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deployment name for GPT-Image-1
    /// </summary>
    [Required]
    public string DeploymentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version for GPT-Image-1
    /// </summary>
    public string ApiVersion { get; set; } = "2025-04-01-preview";

    /// <summary>
    /// Gets or sets the model name/version
    /// </summary>
    public string ModelName { get; set; } = "gpt-image-1";

    /// <summary>
    /// Gets or sets the default image size
    /// </summary>
    public string DefaultSize { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the default image quality
    /// </summary>
    public string DefaultQuality { get; set; } = "high";

    /// <summary>
    /// Gets or sets the default output format
    /// </summary>
    public string DefaultOutputFormat { get; set; } = "PNG";

    /// <summary>
    /// Gets or sets the default compression level (0-100)
    /// </summary>
    public int DefaultCompression { get; set; } = 100;

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

        if (string.IsNullOrWhiteSpace(DeploymentName))
            throw new ArgumentException("DeploymentName is required", nameof(DeploymentName));

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

        if (DefaultCompression < 0 || DefaultCompression > 100)
            throw new ArgumentException("DefaultCompression must be between 0 and 100", nameof(DefaultCompression));

        if (!IsValidSize(DefaultSize))
            throw new ArgumentException("DefaultSize must be in format like '1024x1024'", nameof(DefaultSize));

        if (!IsValidQuality(DefaultQuality))
            throw new ArgumentException("DefaultQuality must be one of: low, medium, high", nameof(DefaultQuality));

        if (!IsValidOutputFormat(DefaultOutputFormat))
            throw new ArgumentException("DefaultOutputFormat must be PNG or JPEG", nameof(DefaultOutputFormat));
    }

    private static bool IsValidSize(string size)
    {
        if (string.IsNullOrWhiteSpace(size))
            return false;

        var parts = size.Split('x');
        if (parts.Length != 2)
            return false;

        if (!int.TryParse(parts[0], out var width) || !int.TryParse(parts[1], out var height))
            return false;

        // GPT-Image-1 supported sizes
        var validSizes = new[] { "1024x1024", "1024x1536", "1536x1024" };
        return Array.Exists(validSizes, s => s.Equals(size, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsValidQuality(string quality)
    {
        if (string.IsNullOrWhiteSpace(quality))
            return false;

        var validQualities = new[] { "low", "medium", "high" };
        return Array.Exists(validQualities, q => q.Equals(quality, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsValidOutputFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            return false;

        var validFormats = new[] { "PNG", "JPEG" };
        return Array.Exists(validFormats, f => f.Equals(format, StringComparison.OrdinalIgnoreCase));
    }
} 