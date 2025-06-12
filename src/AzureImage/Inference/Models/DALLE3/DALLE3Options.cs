using System;
using System.ComponentModel.DataAnnotations;

namespace AzureImage.Inference.Models.DALLE3;

/// <summary>
/// Configuration options for DALL-E 3 model
/// </summary>
public class DALLE3Options
{
    /// <summary>
    /// Gets or sets the endpoint URL for the DALL-E 3 deployment
    /// </summary>
    [Required]
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for authentication
    /// </summary>
    [Required]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deployment name for DALL-E 3
    /// </summary>
    [Required]
    public string DeploymentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version for DALL-E 3
    /// </summary>
    public string ApiVersion { get; set; } = "2024-02-01";

    /// <summary>
    /// Gets or sets the model name/version
    /// </summary>
    public string ModelName { get; set; } = "dall-e-3";

    /// <summary>
    /// Gets or sets the default image size
    /// </summary>
    public string DefaultSize { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the default image quality
    /// </summary>
    public string DefaultQuality { get; set; } = "standard";

    /// <summary>
    /// Gets or sets the default image style
    /// </summary>
    public string DefaultStyle { get; set; } = "vivid";

    /// <summary>
    /// Gets or sets the default response format
    /// </summary>
    public string DefaultResponseFormat { get; set; } = "url";

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

        if (!IsValidSize(DefaultSize))
            throw new ArgumentException("DefaultSize must be one of: 1024x1024, 1792x1024, 1024x1792", nameof(DefaultSize));

        if (!IsValidQuality(DefaultQuality))
            throw new ArgumentException("DefaultQuality must be one of: standard, hd", nameof(DefaultQuality));

        if (!IsValidStyle(DefaultStyle))
            throw new ArgumentException("DefaultStyle must be one of: natural, vivid", nameof(DefaultStyle));

        if (!IsValidResponseFormat(DefaultResponseFormat))
            throw new ArgumentException("DefaultResponseFormat must be one of: url, b64_json", nameof(DefaultResponseFormat));
    }

    private static bool IsValidSize(string size)
    {
        if (string.IsNullOrWhiteSpace(size))
            return false;

        // DALL-E 3 supported sizes
        var validSizes = new[] { "1024x1024", "1792x1024", "1024x1792" };
        return Array.Exists(validSizes, s => s.Equals(size, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsValidQuality(string quality)
    {
        if (string.IsNullOrWhiteSpace(quality))
            return false;

        var validQualities = new[] { "standard", "hd" };
        return Array.Exists(validQualities, q => q.Equals(quality, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsValidStyle(string style)
    {
        if (string.IsNullOrWhiteSpace(style))
            return false;

        var validStyles = new[] { "natural", "vivid" };
        return Array.Exists(validStyles, s => s.Equals(style, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsValidResponseFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            return false;

        var validFormats = new[] { "url", "b64_json" };
        return Array.Exists(validFormats, f => f.Equals(format, StringComparison.OrdinalIgnoreCase));
    }
} 