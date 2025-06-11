using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace AzureImage.Inference.Models.GPTImage1;

/// <summary>
/// Request model for GPT-Image-1 image generation
/// </summary>
public class ImageGenerationRequest
{
    /// <summary>
    /// Gets or sets the model name
    /// </summary>
    [Required]
    [JsonPropertyName("model")]
    public string Model { get; set; } = "gpt-image-1";

    /// <summary>
    /// Gets or sets the text prompt for image generation
    /// </summary>
    [Required]
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the generated image
    /// Must be one of: 1024x1024, 1024x1536, or 1536x1024
    /// </summary>
    [JsonPropertyName("size")]
    public string Size { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the number of images to generate (1-10)
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; } = 1;

    /// <summary>
    /// Gets or sets the quality of the generated image
    /// Must be one of: low, medium, high
    /// </summary>
    [JsonPropertyName("quality")]
    public string Quality { get; set; } = "high";

    /// <summary>
    /// Gets or sets the output format of the generated image
    /// Must be one of: PNG, JPEG
    /// </summary>
    [JsonPropertyName("output_format")]
    public string? OutputFormat { get; set; } = "PNG";

    /// <summary>
    /// Gets or sets the compression level for the generated image (0-100)
    /// </summary>
    [JsonPropertyName("output_compression")]
    public int? OutputCompression { get; set; } = 100;

    /// <summary>
    /// Gets or sets a unique identifier for the user making the request
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    /// Validates the request model
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Model))
            throw new ArgumentException("Model is required", nameof(Model));

        if (string.IsNullOrWhiteSpace(Prompt))
            throw new ArgumentException("Prompt is required", nameof(Prompt));

        if (!IsValidSize(Size))
            throw new ArgumentException("Invalid size. Must be one of: 1024x1024, 1024x1536, 1536x1024", nameof(Size));

        if (N.HasValue && (N.Value < 1 || N.Value > 10))
            throw new ArgumentException("N must be between 1 and 10", nameof(N));

        if (!IsValidQuality(Quality))
            throw new ArgumentException("Invalid quality. Must be one of: low, medium, high", nameof(Quality));

        if (!string.IsNullOrEmpty(OutputFormat) && !IsValidOutputFormat(OutputFormat))
            throw new ArgumentException("Invalid output format. Must be PNG or JPEG", nameof(OutputFormat));

        if (OutputCompression.HasValue && (OutputCompression.Value < 0 || OutputCompression.Value > 100))
            throw new ArgumentException("OutputCompression must be between 0 and 100", nameof(OutputCompression));
    }

    private static bool IsValidSize(string size)
    {
        if (string.IsNullOrWhiteSpace(size))
            return false;

        var validSizes = new[] { "1024x1024", "1024x1536", "1536x1024" };
        return validSizes.Contains(size, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidQuality(string quality)
    {
        if (string.IsNullOrWhiteSpace(quality))
            return false;

        var validQualities = new[] { "low", "medium", "high" };
        return validQualities.Contains(quality, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidOutputFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            return false;

        var validFormats = new[] { "PNG", "JPEG" };
        return validFormats.Contains(format, StringComparer.OrdinalIgnoreCase);
    }
} 