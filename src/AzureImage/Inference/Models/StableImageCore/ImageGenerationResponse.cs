using System.Text.Json.Serialization;

namespace AzureImage.Inference.Models.StableImageCore;

/// <summary>
/// Response model for StableImageCore image generation
/// </summary>
public class ImageGenerationResponse
{
    /// <summary>
    /// Gets or sets the generated image data as base64 string
    /// </summary>
    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional metadata about the generation
    /// </summary>
    [JsonPropertyName("metadata")]
    public ImageMetadata? Metadata { get; set; }

    /// <summary>
    /// Converts the base64 image data to byte array
    /// </summary>
    /// <returns>The image data as byte array</returns>
    /// <exception cref="FormatException">Thrown when the image data is not valid base64</exception>
    public byte[] GetImageBytes()
    {
        if (string.IsNullOrEmpty(Image))
            throw new InvalidOperationException("No image data available");

        try
        {
            return Convert.FromBase64String(Image);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Invalid base64 image data", ex);
        }
    }

    /// <summary>
    /// Saves the image to a file
    /// </summary>
    /// <param name="filePath">The file path to save to</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SaveImageAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        var imageBytes = GetImageBytes();
        await File.WriteAllBytesAsync(filePath, imageBytes, cancellationToken);
    }
}

/// <summary>
/// Metadata information about the generated image
/// </summary>
public class ImageMetadata
{
    /// <summary>
    /// Gets or sets the width of the generated image
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the generated image
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the format of the generated image
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the seed used for generation
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    /// <summary>
    /// Gets or sets the model used for generation
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
} 