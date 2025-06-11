using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace AzureImage.Inference.Models.GPTImage1;

/// <summary>
/// Request model for GPT-Image-1 image editing
/// Note: This uses multipart/form-data format, not JSON
/// </summary>
public class ImageEditingRequest
{
    /// <summary>
    /// Gets or sets the model name
    /// </summary>
    [Required]
    public string Model { get; set; } = "gpt-image-1";

    /// <summary>
    /// Gets or sets the text prompt describing the edit to make
    /// </summary>
    [Required]
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image to edit as byte array
    /// </summary>
    [Required]
    public byte[] Image { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Gets or sets the filename for the image (used in multipart form data)
    /// </summary>
    public string ImageFileName { get; set; } = "image.png";

    /// <summary>
    /// Gets or sets the mask image as byte array (optional)
    /// Defines the area of the image to edit using fully transparent pixels
    /// </summary>
    public byte[]? Mask { get; set; }

    /// <summary>
    /// Gets or sets the filename for the mask image (used in multipart form data)
    /// </summary>
    public string? MaskFileName { get; set; } = "mask.png";

    /// <summary>
    /// Gets or sets the size of the generated image
    /// Must be one of: 1024x1024, 1024x1536, or 1536x1024
    /// </summary>
    public string Size { get; set; } = "1024x1024";

    /// <summary>
    /// Gets or sets the number of images to generate (1-10)
    /// </summary>
    public int? N { get; set; } = 1;

    /// <summary>
    /// Gets or sets the quality of the generated image
    /// Must be one of: low, medium, high
    /// </summary>
    public string Quality { get; set; } = "high";

    /// <summary>
    /// Gets or sets the output format of the generated image
    /// Must be one of: PNG, JPEG
    /// </summary>
    public string? OutputFormat { get; set; } = "PNG";

    /// <summary>
    /// Gets or sets the compression level for the generated image (0-100)
    /// </summary>
    public int? OutputCompression { get; set; } = 100;

    /// <summary>
    /// Gets or sets a unique identifier for the user making the request
    /// </summary>
    public string? User { get; set; }

    /// <summary>
    /// Creates an ImageEditingRequest from a file path
    /// </summary>
    /// <param name="imagePath">Path to the image file</param>
    /// <param name="prompt">The editing prompt</param>
    /// <param name="maskPath">Optional path to the mask file</param>
    /// <returns>A new ImageEditingRequest instance</returns>
    public static async System.Threading.Tasks.Task<ImageEditingRequest> FromFileAsync(
        string imagePath, 
        string prompt, 
        string? maskPath = null)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            throw new ArgumentException("Image path cannot be null or empty", nameof(imagePath));

        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Prompt cannot be null or empty", nameof(prompt));

        if (!File.Exists(imagePath))
            throw new FileNotFoundException($"Image file not found: {imagePath}");

        var request = new ImageEditingRequest
        {
            Image = await File.ReadAllBytesAsync(imagePath),
            ImageFileName = Path.GetFileName(imagePath),
            Prompt = prompt
        };

        if (!string.IsNullOrEmpty(maskPath))
        {
            if (!File.Exists(maskPath))
                throw new FileNotFoundException($"Mask file not found: {maskPath}");

            request.Mask = await File.ReadAllBytesAsync(maskPath);
            request.MaskFileName = Path.GetFileName(maskPath);
        }

        return request;
    }

    /// <summary>
    /// Creates an ImageEditingRequest from byte arrays
    /// </summary>
    /// <param name="imageBytes">The image data</param>
    /// <param name="prompt">The editing prompt</param>
    /// <param name="imageFileName">The image filename</param>
    /// <param name="maskBytes">Optional mask data</param>
    /// <param name="maskFileName">Optional mask filename</param>
    /// <returns>A new ImageEditingRequest instance</returns>
    public static ImageEditingRequest FromBytes(
        byte[] imageBytes, 
        string prompt, 
        string imageFileName = "image.png",
        byte[]? maskBytes = null, 
        string? maskFileName = "mask.png")
    {
        if (imageBytes == null || imageBytes.Length == 0)
            throw new ArgumentException("Image bytes cannot be null or empty", nameof(imageBytes));

        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Prompt cannot be null or empty", nameof(prompt));

        return new ImageEditingRequest
        {
            Image = imageBytes,
            ImageFileName = imageFileName,
            Prompt = prompt,
            Mask = maskBytes,
            MaskFileName = maskBytes != null ? maskFileName : null
        };
    }

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

        if (Image == null || Image.Length == 0)
            throw new ArgumentException("Image is required", nameof(Image));

        if (string.IsNullOrWhiteSpace(ImageFileName))
            throw new ArgumentException("ImageFileName is required", nameof(ImageFileName));

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

        // Validate that mask filename is provided if mask bytes are provided
        if (Mask != null && Mask.Length > 0 && string.IsNullOrWhiteSpace(MaskFileName))
            throw new ArgumentException("MaskFileName is required when Mask is provided", nameof(MaskFileName));
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