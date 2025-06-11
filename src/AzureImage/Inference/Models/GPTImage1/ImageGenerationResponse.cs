using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AzureImage.Inference.Models.GPTImage1;

/// <summary>
/// Response model for GPT-Image-1 image generation
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
    /// The URL stays active for 24 hours
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the revised prompt that was actually used for generation
    /// </summary>
    [JsonPropertyName("revised_prompt")]
    public string? RevisedPrompt { get; set; }

    /// <summary>
    /// Downloads the image from the URL as a byte array
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for the download</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The image data as byte array</returns>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when the URL is empty</exception>
    /// <exception cref="HttpRequestException">Thrown when the download fails</exception>
    public async Task<byte[]> DownloadImageAsync(HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        if (httpClient == null)
            throw new ArgumentNullException(nameof(httpClient));

        if (string.IsNullOrEmpty(Url))
            throw new InvalidOperationException("Image URL is not available");

        try
        {
            return await httpClient.GetByteArrayAsync(Url, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"Failed to download image from URL: {Url}", ex);
        }
    }

    /// <summary>
    /// Downloads and saves the image to a file
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for the download</param>
    /// <param name="filePath">The file path to save to</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SaveImageAsync(HttpClient httpClient, string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        var imageBytes = await DownloadImageAsync(httpClient, cancellationToken);
        
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