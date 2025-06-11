using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzureAISDK.Inference.Image.StableImageUltra;

/// <summary>
/// Request model for StableImageUltra image generation
/// </summary>
public class ImageGenerationRequest
{
    /// <summary>
    /// Gets or sets the model name
    /// </summary>
    [Required]
    [JsonPropertyName("model")]
    public string Model { get; set; } = "Stable-Image-Ultra";

    /// <summary>
    /// Gets or sets the text prompt for image generation
    /// </summary>
    [Required]
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the negative prompt (what to avoid in the image)
    /// </summary>
    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt { get; set; }

    /// <summary>
    /// Gets or sets the size of the generated image
    /// </summary>
    [JsonPropertyName("size")]
    public string Size { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the output format of the generated image
    /// </summary>
    [JsonPropertyName("output_format")]
    public string OutputFormat { get; set; } = "png";

    /// <summary>
    /// Gets or sets the seed for reproducible generation
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

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
            throw new ArgumentException("Invalid size format. Use format like '1024x1024'", nameof(Size));

        if (!IsValidOutputFormat(OutputFormat))
            throw new ArgumentException("Invalid output format. Supported formats: png, jpg, jpeg, webp", nameof(OutputFormat));

        if (Seed.HasValue && Seed.Value < 0)
            throw new ArgumentException("Seed must be non-negative", nameof(Seed));
    }

    private static bool IsValidSize(string size)
    {
        if (string.IsNullOrWhiteSpace(size))
            return false;

        var parts = size.Split('x');
        if (parts.Length != 2)
            return false;

        return int.TryParse(parts[0], out var width) && 
               int.TryParse(parts[1], out var height) &&
               width > 0 && height > 0;
    }

    private static bool IsValidOutputFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            return false;

        var validFormats = new[] { "png", "jpg", "jpeg", "webp" };
        return validFormats.Contains(format.ToLowerInvariant());
    }
} 