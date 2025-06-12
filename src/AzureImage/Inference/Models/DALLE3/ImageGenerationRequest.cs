using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace AzureImage.Inference.Models.DALLE3;

/// <summary>
/// Request model for DALL-E 3 image generation
/// </summary>
public class ImageGenerationRequest
{
    /// <summary>
    /// Gets or sets the text prompt for image generation
    /// </summary>
    [Required]
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the generated image
    /// Must be one of: 1024x1024, 1792x1024, or 1024x1792
    /// </summary>
    [JsonPropertyName("size")]
    public string Size { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the number of images to generate
    /// For DALL-E 3, this must be 1 (batch generation not supported)
    /// </summary>
    [JsonPropertyName("n")]
    public int N { get; set; } = 1;

    /// <summary>
    /// Gets or sets the quality of the generated image
    /// Must be one of: standard, hd
    /// </summary>
    [JsonPropertyName("quality")]
    public string Quality { get; set; } = "standard";

    /// <summary>
    /// Gets or sets the style of the generated image
    /// Must be one of: natural, vivid
    /// </summary>
    [JsonPropertyName("style")]
    public string Style { get; set; } = "vivid";

    /// <summary>
    /// Gets or sets the format in which the generated images are returned
    /// Must be one of: url, b64_json
    /// Note: This parameter is not supported for GPT-Image-1
    /// </summary>
    [JsonPropertyName("response_format")]
    public string? ResponseFormat { get; set; } = "url";

    /// <summary>
    /// Validates the request model
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Prompt))
            throw new ArgumentException("Prompt is required", nameof(Prompt));

        if (!IsValidSize(Size))
            throw new ArgumentException("Invalid size. Must be one of: 1024x1024, 1792x1024, 1024x1792", nameof(Size));

        if (N != 1)
            throw new ArgumentException("DALL-E 3 only supports generating 1 image per request (n must be 1)", nameof(N));

        if (!IsValidQuality(Quality))
            throw new ArgumentException("Invalid quality. Must be one of: standard, hd", nameof(Quality));

        if (!IsValidStyle(Style))
            throw new ArgumentException("Invalid style. Must be one of: natural, vivid", nameof(Style));

        if (!string.IsNullOrEmpty(ResponseFormat) && !IsValidResponseFormat(ResponseFormat))
            throw new ArgumentException("Invalid response format. Must be one of: url, b64_json", nameof(ResponseFormat));
    }

    private static bool IsValidSize(string size)
    {
        if (string.IsNullOrWhiteSpace(size))
            return false;

        var validSizes = new[] { "1024x1024", "1792x1024", "1024x1792" };
        return validSizes.Contains(size, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidQuality(string quality)
    {
        if (string.IsNullOrWhiteSpace(quality))
            return false;

        var validQualities = new[] { "standard", "hd" };
        return validQualities.Contains(quality, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidStyle(string style)
    {
        if (string.IsNullOrWhiteSpace(style))
            return false;

        var validStyles = new[] { "natural", "vivid" };
        return validStyles.Contains(style, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsValidResponseFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            return false;

        var validFormats = new[] { "url", "b64_json" };
        return validFormats.Contains(format, StringComparer.OrdinalIgnoreCase);
    }
} 