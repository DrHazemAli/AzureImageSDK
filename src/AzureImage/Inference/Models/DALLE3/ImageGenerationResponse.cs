using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AzureImage.Inference.Models.DALLE3;

/// <summary>
/// Response model for DALL-E 3 image generation
/// </summary>
public class ImageGenerationResponse
{
    /// <summary>
    /// Gets or sets the timestamp when the image was created
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; set; }

    /// <summary>
    /// Gets or sets the list of generated images
    /// </summary>
    [JsonPropertyName("data")]
    public List<ImageData> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the error information if the request failed
    /// </summary>
    [JsonPropertyName("error")]
    public ErrorInfo? Error { get; set; }

    /// <summary>
    /// Gets a value indicating whether the response contains an error
    /// </summary>
    [JsonIgnore]
    public bool HasError => Error != null;

    /// <summary>
    /// Gets the creation date and time from the timestamp
    /// </summary>
    [JsonIgnore]
    public DateTime CreatedDateTime => DateTimeOffset.FromUnixTimeSeconds(Created).DateTime;
}

/// <summary>
/// Represents a single generated image in the response
/// </summary>
public class ImageData
{
    /// <summary>
    /// Gets or sets the URL where the generated image can be downloaded
    /// Available when response_format is "url"
    /// The URL stays active for 24 hours
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded image data
    /// Available when response_format is "b64_json"
    /// </summary>
    [JsonPropertyName("b64_json")]
    public string? B64Json { get; set; }

    /// <summary>
    /// Gets or sets the revised prompt that was actually used for generation
    /// </summary>
    [JsonPropertyName("revised_prompt")]
    public string? RevisedPrompt { get; set; }

    /// <summary>
    /// Gets a value indicating whether this image data contains a URL
    /// </summary>
    [JsonIgnore]
    public bool HasUrl => !string.IsNullOrEmpty(Url);

    /// <summary>
    /// Gets a value indicating whether this image data contains base64 data
    /// </summary>
    [JsonIgnore]
    public bool HasBase64Data => !string.IsNullOrEmpty(B64Json);

    /// <summary>
    /// Gets the image data as a byte array
    /// </summary>
    /// <returns>The image data as byte array</returns>
    /// <exception cref="InvalidOperationException">Thrown when no image data is available</exception>
    /// <exception cref="FormatException">Thrown when the base64 data is invalid</exception>
    public byte[] GetImageBytes()
    {
        if (HasBase64Data)
        {
            try
            {
                return Convert.FromBase64String(B64Json!);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid base64 image data", ex);
            }
        }

        throw new InvalidOperationException("No base64 image data available. Use DownloadImageAsync for URL-based responses.");
    }

    /// <summary>
    /// Downloads the image from the URL as a byte array
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for the download</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The image data as byte array</returns>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when no URL is available</exception>
    /// <exception cref="HttpRequestException">Thrown when the download fails</exception>
    public async Task<byte[]> DownloadImageAsync(HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        if (httpClient == null)
            throw new ArgumentNullException(nameof(httpClient));

        if (!HasUrl)
            throw new InvalidOperationException("No image URL available. Use GetImageBytes() for base64 responses.");

        try
        {
            return await httpClient.GetByteArrayAsync(Url!, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"Failed to download image from URL: {Url}", ex);
        }
    }

    /// <summary>
    /// Gets the image data as byte array, automatically choosing between base64 and URL
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for URL downloads (can be null for base64 data)</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The image data as byte array</returns>
    public async Task<byte[]> GetImageBytesAsync(HttpClient? httpClient = null, CancellationToken cancellationToken = default)
    {
        if (HasBase64Data)
        {
            return GetImageBytes();
        }
        
        if (HasUrl)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient), "HttpClient is required for URL-based image download");
            
            return await DownloadImageAsync(httpClient, cancellationToken);
        }

        throw new InvalidOperationException("No image data available in response");
    }

    /// <summary>
    /// Saves the image to a file
    /// </summary>
    /// <param name="filePath">The file path to save to</param>
    /// <param name="httpClient">The HTTP client to use for URL downloads (can be null for base64 data)</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SaveImageAsync(string filePath, HttpClient? httpClient = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        var imageBytes = await GetImageBytesAsync(httpClient, cancellationToken);
        
        // Ensure directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllBytesAsync(filePath, imageBytes, cancellationToken);
    }
}

/// <summary>
/// Represents error information in the response
/// </summary>
public class ErrorInfo
{
    /// <summary>
    /// Gets or sets the error code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this error is due to content filtering
    /// </summary>
    [JsonIgnore]
    public bool IsContentFiltered => string.Equals(Code, "contentFilter", StringComparison.OrdinalIgnoreCase);
} 