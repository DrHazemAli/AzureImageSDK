# Image Captioning

The AzureImage SDK provides powerful image captioning capabilities using Azure AI Vision Image Analysis 4.0 API. This feature allows you to generate human-readable descriptions of images automatically.

## Overview

Image captioning uses advanced AI models to analyze visual content and generate descriptive text. The SDK supports two types of captioning:

- **Single Caption**: Generate one comprehensive description of the entire image
- **Dense Captions**: Generate multiple descriptions for different regions within the image

## Features

- 🔍 **Single and Dense Captioning**: Generate comprehensive or region-specific descriptions
- 🌐 **Multi-language Support**: Support for multiple languages (default: English)
- ⚖️ **Gender-Neutral Options**: Generate inclusive descriptions
- 🖼️ **Multiple Input Sources**: Process images from URLs or local files
- 📦 **Bounding Box Information**: Get spatial coordinates for dense captions
- 🎯 **Confidence Scores**: Receive confidence ratings for generated captions

## Supported Regions

Azure AI Vision Image Analysis 4.0 Caption features are only available in specific Azure regions:

- East US
- West US  
- West US 2
- France Central
- North Europe
- West Europe
- Sweden Central
- Switzerland North
- Australia East
- Southeast Asia
- East Asia
- Korea Central
- Japan East

## Quick Start

### Basic Setup

```csharp
using AzureImage.Core;
using AzureImage.Inference.Models.AzureVisionCaptioning;
using Microsoft.Extensions.Logging;

// Create the client
using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
using var azureImageClient = AzureImageClient.Create(loggerFactory);

// Configure Azure Vision
var visionOptions = new AzureVisionCaptioningOptions
{
    Endpoint = "https://your-resource.cognitiveservices.azure.com",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-01"
};

// Create the captioning model
using var httpClient = new HttpClient();
var captioningModel = new AzureVisionCaptioningModel(httpClient, visionOptions);
```

### Generate Single Caption

```csharp
// From URL
var result = await azureImageClient.GenerateCaptionAsync(
    captioningModel, 
    "https://example.com/image.jpg");

Console.WriteLine($"Caption: {result.Caption.Text}");
Console.WriteLine($"Confidence: {result.Caption.Confidence:F4}");

// From local file
using var imageStream = File.OpenRead("image.jpg");
var result = await azureImageClient.GenerateCaptionAsync(
    captioningModel, 
    imageStream);
```

### Generate Dense Captions

```csharp
var result = await azureImageClient.GenerateDenseCaptionsAsync(
    captioningModel, 
    "https://example.com/image.jpg");

foreach (var caption in result.Captions)
{
    Console.WriteLine($"Caption: {caption.Text}");
    Console.WriteLine($"Region: {caption.BoundingBox}");
    Console.WriteLine($"Confidence: {caption.Confidence:F4}");
}
```

## Configuration Options

### ImageCaptionOptions

Configure captioning behavior with `ImageCaptionOptions`:

```csharp
var options = new ImageCaptionOptions
{
    Language = "en",                    // Language code (default: "en")
    GenderNeutralCaption = true,        // Use gender-neutral terms
    MaxDenseCaptions = 10               // Max dense captions (1-10)
};

var result = await azureImageClient.GenerateCaptionAsync(
    captioningModel, 
    imageUrl, 
    options);
```

### AzureVisionCaptioningOptions

Configure the Azure Vision service connection:

```csharp
var visionOptions = new AzureVisionCaptioningOptions
{
    Endpoint = "https://your-resource.cognitiveservices.azure.com",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-01",           // API version
    TimeoutSeconds = 30,                 // Request timeout
    MaxRetryAttempts = 3,                // Retry attempts
    EnableLogging = false                // Enable detailed logging
};
```

## Response Models

### ImageCaptionResult

Single caption response structure:

```csharp
public class ImageCaptionResult
{
    public Caption Caption { get; set; }           // Generated caption
    public string ModelVersion { get; set; }       // AI model version
    public ImageMetadata Metadata { get; set; }    // Image dimensions
}

public class Caption
{
    public string Text { get; set; }               // Caption text
    public double Confidence { get; set; }         // Confidence (0-1)
}
```

### DenseCaptionResult

Dense captions response structure:

```csharp
public class DenseCaptionResult
{
    public List<DenseCaption> Captions { get; set; }    // Multiple captions
    public string ModelVersion { get; set; }            // AI model version
    public ImageMetadata Metadata { get; set; }         // Image dimensions
}

public class DenseCaption : Caption
{
    public BoundingBox BoundingBox { get; set; }        // Region coordinates
}

public class BoundingBox
{
    public int X { get; set; }          // Top-left X coordinate
    public int Y { get; set; }          // Top-left Y coordinate
    public int Width { get; set; }      // Bounding box width
    public int Height { get; set; }     // Bounding box height
}
```

## Advanced Usage

### Gender-Neutral Captions

Generate inclusive descriptions by replacing gendered terms:

```csharp
var options = new ImageCaptionOptions
{
    GenderNeutralCaption = true
};

// Instead of "a man walking" → "a person walking"
// Instead of "a woman sitting" → "a person sitting"
```

### Multi-Language Support

Generate captions in different languages:

```csharp
var options = new ImageCaptionOptions
{
    Language = "es"  // Spanish
};

// Supported languages: en, es, ja, pt, zh
```

### Error Handling

Implement robust error handling:

```csharp
try
{
    var result = await azureImageClient.GenerateCaptionAsync(model, imageUrl);
    // Process successful result
}
catch (AzureVisionException ex)
{
    // Handle Azure Vision API specific errors
    Console.WriteLine($"API Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Response: {ex.ResponseContent}");
}
catch (ArgumentException ex)
{
    // Handle invalid arguments
    Console.WriteLine($"Invalid input: {ex.Message}");
}
catch (HttpRequestException ex)
{
    // Handle network-related errors
    Console.WriteLine($"Network error: {ex.Message}");
}
```

## Image Requirements

### Supported Formats

- JPEG (.jpg, .jpeg)
- PNG (.png)
- GIF (.gif)
- BMP (.bmp)
- WEBP (.webp)
- ICO (.ico)
- TIFF (.tiff)
- MPO (.mpo)

### Size Limitations

- **File Size**: Maximum 20 MB
- **Dimensions**: Between 50×50 and 16,000×16,000 pixels
- **URL Access**: Images must be publicly accessible for URL-based processing

## Best Practices

### Performance Optimization

1. **Reuse HTTP Client**: Share `HttpClient` instances across requests
2. **Configure Timeouts**: Set appropriate timeout values for your use case
3. **Implement Retry Logic**: Use the built-in retry mechanism
4. **Cache Results**: Store caption results to avoid repeated API calls

### Quality Considerations

1. **Image Quality**: Use high-quality, well-lit images for better results
2. **Image Size**: Optimal results with images between 512×512 and 2048×2048
3. **Content Clarity**: Images with clear subjects produce more accurate captions
4. **Multiple Regions**: Use dense captions for complex images with multiple subjects

### Security

1. **API Key Management**: Store API keys securely using Azure Key Vault
2. **Image Privacy**: Be mindful of sensitive content in images
3. **Access Control**: Implement proper authentication and authorization
4. **Data Retention**: Understand Azure's data retention policies

## Troubleshooting

### Common Issues

**"Caption features not supported in this region"**
- Ensure your Azure AI Vision resource is in a supported region
- Check the supported regions list above

**"Access denied due to invalid subscription key"**
- Verify your API key is correct and active
- Ensure the key matches your Azure AI Vision resource

**"The provided image URL is not accessible"**
- Check that the image URL is publicly accessible
- Verify the image format is supported
- Ensure the URL returns a valid image

**"Image format is not valid"**
- Confirm the image is in a supported format
- Check that the image file is not corrupted
- Verify the file size is within limits

### Performance Issues

**Slow Response Times**
- Check your internet connection
- Verify the Azure region is geographically close
- Consider using smaller image sizes
- Review timeout configurations

**Intermittent Failures**
- Implement retry logic with exponential backoff
- Check Azure service status
- Monitor your API quota and rate limits

## Samples

See the complete working example in the [ImageCaptioningSample](../../samples/ImageCaptioningSample/) project.

## API Reference

- [IImageCaptioningModel](../API-Reference/IImageCaptioningModel.md)
- [AzureVisionCaptioningModel](../API-Reference/AzureVisionCaptioningModel.md)
- [ImageCaptionOptions](../API-Reference/ImageCaptionOptions.md)
- [ImageCaptionResult](../API-Reference/ImageCaptionResult.md)

## Related Features

- [Image Utilities](../Utilities/AspectRatioConverter.md)
- [Error Handling](../Getting-Started/Error-Handling.md)
- [Configuration](../Getting-Started/Configuration.md) 