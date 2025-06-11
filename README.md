# Azure Image SDK

[![NuGet](https://img.shields.io/nuget/v/AzureImage.svg)](https://www.nuget.org/packages/AzureImage/)
[![GitHub](https://img.shields.io/github/license/DrHazemAli/AzureImageSDK)](LICENSE)
[![Wiki](https://img.shields.io/badge/docs-wiki-blue.svg)](https://github.com/DrHazemAli/AzureImageSDK/wiki)

A comprehensive .NET SDK for Azure Image services, providing easy-to-use client libraries for image generation capabilities including Stable Image Ultra, Stable Image Core and more.

![Azure Image SDK](https://raw.githubusercontent.com/DrHazemAli/AzureImageSDK/refs/heads/main/azure_image_sdk.jpg)


Azure Image SDK is a .NET library that enables developers to easily integrate Azure AI Foundry's image capabilities into their applications. The SDK supports .NET 6.0 and above, providing a modern and efficient way to work with AI-powered image generation and manipulation.


## Features

### 🎨 AI-Powered Image Generation
- **Stable Image Ultra** - Advanced image generation with customizable parameters, high-quality outputs
- **Stable Image Core** - Core image generation capabilities with essential features
- **Configurable Parameters** - Custom sizes, prompts, seeds, output formats, and more

### 👁️ Computer Vision & AI Analysis
- **Azure Vision Captioning** - Generate descriptive captions for images using Azure AI Vision
- **Dense Captioning** - Generate multiple region-specific captions with bounding boxes
- **Multi-Input Support** - Analyze images from streams, URLs, or file paths
- **Language Support** - Multiple language options for captions
- **Gender-Neutral Options** - Configurable gender-neutral caption generation

### 🛠️ Image Processing Utilities
- **Format Conversion** - Convert between image formats (PNG, JPG, WEBP, etc.) and MIME types
- **Quality Analysis** - Analyze image sharpness, brightness, contrast, and overall quality scores
- **Metadata Extraction** - Extract comprehensive image metadata (dimensions, format, properties)
- **Size Validation** - Validate image dimensions and file sizes with customizable limits
- **Aspect Ratio Tools** - Convert and validate aspect ratios for different use cases

### 🏗️ Architecture & Infrastructure
- **Modular Design** - Easy to extend with new AI services and models
- **Dependency Injection** - Full support for .NET DI container and ASP.NET Core
- **Retry Logic** - Built-in exponential backoff retry mechanism with configurable policies
- **Error Handling** - Comprehensive exception handling with detailed error information
- **Logging Integration** - Structured logging with Microsoft.Extensions.Logging
- **Type Safety** - Strongly typed models, requests, and responses throughout
- **Async/Await** - Fully asynchronous API for optimal performance
- **Configuration Flexible** - Support for appsettings.json, environment variables, and code-based config
- **Well Tested** - Comprehensive unit test coverage across all components

## Installation

### Package Manager
```powershell
Install-Package AzureImage
```

### .NET CLI
```bash
dotnet add package AzureImage
```

### PackageReference
```xml
<PackageReference Include="AzureImage" Version="1.0.0" />
```

## Supported Frameworks

- .NET 6.0+
- .NET 7.0+
- .NET 8.0+

## Quick Start

### 1. Image Generation - Simple Usage

```csharp
using AzureImage.Core;
using AzureImage.Inference.Models.StableImageUltra;

// Create the Azure Image client
using var client = AzureImageClient.Create();

// Create StableImageUltra model with configuration
var model = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key");

// Create model-specific request
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A beautiful sunset over mountains"
};

// Generate image using the model
var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

// Save the image
await response.SaveImageAsync("sunset.png");
```

### 2. Computer Vision - Image Captioning

```csharp
using AzureImage.Core;
using AzureImage.Inference.Models.AzureVisionCaptioning;

// Create Azure Vision captioning model
var visionModel = AzureVisionCaptioningModel.Create(
    endpoint: "https://your-vision-endpoint.cognitiveservices.azure.com",
    apiKey: "your-vision-api-key");

// Generate caption from image file
using var imageStream = File.OpenRead("image.jpg");
var captionResult = await visionModel.GenerateCaptionAsync(imageStream);

Console.WriteLine($"Caption: {captionResult.Caption.Text}");
Console.WriteLine($"Confidence: {captionResult.Caption.Confidence:P2}");

// Generate dense captions with regions
var denseCaptions = await visionModel.GenerateDenseCaptionsAsync(
    "https://example.com/image.jpg");

foreach (var caption in denseCaptions.Captions)
{
    Console.WriteLine($"Region: {caption.Text} (Confidence: {caption.Confidence:P2})");
    Console.WriteLine($"Location: X={caption.BoundingBox.X}, Y={caption.BoundingBox.Y}");
}
```

### 3. Image Processing Utilities

```csharp
using AzureImage.Utilities;

// Format conversion
string mimeType = ImageFormatConverter.GetMimeType(".jpg"); // "image/jpeg"
string extension = ImageFormatConverter.GetFileExtension("image/png"); // ".png"
bool isValid = ImageFormatConverter.IsValidImageFormat("webp"); // true

// Quality analysis
using var imageStream = File.OpenRead("photo.jpg");
var qualityMetrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(imageStream);
Console.WriteLine($"Sharpness: {qualityMetrics.Sharpness:P2}");
Console.WriteLine($"Brightness: {qualityMetrics.Brightness:P2}");
Console.WriteLine($"Overall Score: {qualityMetrics.OverallScore:P2}");

// Metadata extraction
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(imageStream);
Console.WriteLine($"Dimensions: {metadata.Width}x{metadata.Height}");
Console.WriteLine($"Format: {metadata.Format}");

// Size validation
bool isValidSize = ImageSizeValidator.IsValidDimensions(1920, 1080, maxWidth: 2048, maxHeight: 2048);
```

### 4. Advanced Image Generation with Custom Parameters

```csharp
using AzureImage.Inference.Models.StableImageUltra;
using AzureImage.Inference.Models.StableImageCore;

// Stable Image Ultra with advanced options
var ultraModel = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key",
    options =>
    {
        options.ApiVersion = "2024-05-01-preview";
        options.ModelName = "Stable-Image-Ultra-v2";
        options.DefaultSize = "1024x1024";
        options.DefaultOutputFormat = "png";
        options.Timeout = TimeSpan.FromMinutes(5);
        options.MaxRetryAttempts = 3;
    });

// Stable Image Core for essential generation
var coreModel = StableImageCoreModel.Create(
    endpoint: "https://your-core-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-core-api-key");

// Generate with multiple parameters
var request = new ImageGenerationRequest
{
    Model = ultraModel.ModelName,
    Prompt = "A futuristic cyberpunk cityscape",
    NegativePrompt = "blurry, low quality",
    Size = "1024x1024",
    OutputFormat = "png",
    Seed = 42
};

var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    ultraModel, request);

// Access comprehensive metadata
if (response.Metadata != null)
{
    Console.WriteLine($"Generated with seed: {response.Metadata.Seed}");
    Console.WriteLine($"Dimensions: {response.Metadata.Width}x{response.Metadata.Height}");
    Console.WriteLine($"Format: {response.Metadata.Format}");
}
```

## Dependency Injection

### ASP.NET Core / .NET Host

```csharp
using AzureImage.Extensions;

// In Program.cs or Startup.cs
services.AddAzureImage(options =>
{
    options.Endpoint = "https://your-endpoint.eastus.models.ai.azure.com";
    options.ApiKey = "your-api-key";
    options.Timeout = TimeSpan.FromMinutes(5);
    options.MaxRetryAttempts = 3;
});

// Or from configuration
services.AddAzureImage(configuration.GetSection("AzureImage"));
```

### Using in Controllers/Services

```csharp
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IAzureImageClient _azureImageClient;

    public ImageController(IAzureImageClient azureImageClient)
    {
        _azureImageClient = azureImageClient;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateImage([FromBody] GenerateImageRequest request)
    {
        var response = await _azureAIClient.StableImageUltra.GenerateImageAsync(request.Prompt);
        return File(response.GetImageBytes(), "image/png");
    }
}
```

## Comprehensive API Reference

### 🎨 Image Generation Models

#### Stable Image Ultra API
```csharp
// Basic generation
var response = await client.StableImageUltra.GenerateImageAsync("your prompt");

// With parameters
var response = await client.StableImageUltra.GenerateImageAsync(
    prompt: "A serene landscape",
    negativePrompt: "blurry, distorted",
    size: "1024x1024",
    outputFormat: "png",
    seed: 12345
);

// With request object
var request = new ImageGenerationRequest
{
    Prompt = "A detailed prompt",
    NegativePrompt = "unwanted elements",
    Size = "512x512",
    OutputFormat = "jpg",
    Seed = 42
};
var response = await client.StableImageUltra.GenerateImageAsync(request);
```

#### Stable Image Core API
```csharp
// Essential image generation
var coreModel = StableImageCoreModel.Create(endpoint, apiKey);
var response = await client.GenerateImageAsync(coreModel, request);
```

### 👁️ Computer Vision APIs

#### Azure Vision Captioning
```csharp
// Single caption generation
var visionModel = AzureVisionCaptioningModel.Create(endpoint, apiKey);

// From image stream
var caption = await visionModel.GenerateCaptionAsync(imageStream);

// From image URL
var caption = await visionModel.GenerateCaptionAsync("https://example.com/image.jpg");

// With options
var options = new ImageCaptionOptions
{
    Language = "en",
    GenderNeutralCaption = true
};
var caption = await visionModel.GenerateCaptionAsync(imageStream, options);
```

#### Dense Captioning
```csharp
// Multiple region captions
var denseCaptions = await visionModel.GenerateDenseCaptionsAsync(imageStream);

foreach (var caption in denseCaptions.Captions)
{
    Console.WriteLine($"Caption: {caption.Text}");
    Console.WriteLine($"Confidence: {caption.Confidence}");
    Console.WriteLine($"Region: {caption.BoundingBox.X}, {caption.BoundingBox.Y}");
}
```

### 🛠️ Utility APIs

#### Image Format Conversion
```csharp
// Convert formats
string mimeType = ImageFormatConverter.GetMimeType(".jpg");
string extension = ImageFormatConverter.GetFileExtension("image/jpeg");
bool isValid = ImageFormatConverter.IsValidImageFormat("png");
string[] supportedFormats = ImageFormatConverter.GetSupportedFormats();
```

#### Image Quality Analysis
```csharp
// Quality metrics
var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(imageStream);
double sharpness = await ImageQualityAnalyzer.CalculateSharpnessAsync(imageStream);
double brightness = await ImageQualityAnalyzer.CalculateBrightnessAsync(imageStream);
double contrast = await ImageQualityAnalyzer.CalculateContrastAsync(imageStream);
```

#### Image Metadata Extraction
```csharp
// Extract comprehensive metadata
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(imageStream);
Console.WriteLine($"Size: {metadata.Width}x{metadata.Height}");
Console.WriteLine($"Format: {metadata.Format}");
Console.WriteLine($"File Size: {metadata.FileSizeBytes} bytes");
```

#### Image Size Validation
```csharp
// Validate dimensions and sizes
bool isValidDimension = ImageSizeValidator.IsValidDimensions(width, height);
bool isValidFileSize = ImageSizeValidator.IsValidFileSize(fileSizeBytes);
var validationResult = ImageSizeValidator.ValidateImage(imageStream);
```

#### Aspect Ratio Tools
```csharp
// Aspect ratio calculations
double ratio = AspectRatioConverter.CalculateAspectRatio(1920, 1080);
var dimensions = AspectRatioConverter.GetDimensionsForAspectRatio(16/9, 1920);
bool isValid = AspectRatioConverter.IsValidAspectRatio(1.77, tolerance: 0.01);
```

## Configuration

### appsettings.json

```json
{
  "AzureImage": {
    "StableImageUltra": {
      "Endpoint": "https://your-stable-image-ultra-endpoint.eastus.models.ai.azure.com",
      "ApiKey": "your-stable-image-ultra-api-key-here",
      "ApiVersion": "2024-05-01-preview",
      "DefaultSize": "1024x1024",
      "DefaultOutputFormat": "png"
    },
    "StableImageCore": {
      "Endpoint": "https://your-stable-image-core-endpoint.eastus.models.ai.azure.com",
      "ApiKey": "your-stable-image-core-api-key-here"
    },
    "AzureVisionCaptioning": {
      "Endpoint": "https://your-vision-endpoint.cognitiveservices.azure.com",
      "ApiKey": "your-vision-api-key-here",
      "ApiVersion": "2024-02-01"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "AzureImage": "Debug"
    }
  }
}
```

### Environment Variables

```bash
export AZURE_IMAGE_STABLE_IMAGE_ULTRA_ENDPOINT="https://your-endpoint.eastus.models.ai.azure.com"
export AZURE_IMAGE_STABLE_IMAGE_ULTRA_API_KEY="your-api-key"
```

### Model Configuration in Code

```csharp
// Basic configuration
var model = StableImageUltraModel.Create(endpoint, apiKey);

// Advanced configuration
var model = StableImageUltraModel.Create(endpoint, apiKey, options =>
{
    options.ModelName = "Stable-Image-Ultra-v2";
    options.ApiVersion = "2024-05-01-preview";
    options.DefaultSize = "1024x1024";
    options.DefaultOutputFormat = "png";
    options.Timeout = TimeSpan.FromMinutes(5);
    options.MaxRetryAttempts = 3;
    options.RetryDelay = TimeSpan.FromSeconds(1);
});
```

## Stable Image Ultra API

### Available Methods

#### Simple Generation
```csharp
var response = await client.StableImageUltra.GenerateImageAsync("your prompt");
```

#### With Parameters
```csharp
var response = await client.StableImageUltra.GenerateImageAsync(
    prompt: "A serene landscape",
    negativePrompt: "blurry, distorted",
    size: "1024x1024",
    outputFormat: "png",
    seed: 12345
);
```

#### With Request Object
```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A detailed prompt",
    NegativePrompt = "unwanted elements",
    Size = "512x512",
    OutputFormat = "jpg",
    Seed = 42
};

var response = await client.StableImageUltra.GenerateImageAsync(request);
```

### Supported Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| `Prompt` | `string` | Text description of the desired image | Required |
| `NegativePrompt` | `string?` | What to avoid in the image | `null` |
| `Size` | `string` | Image dimensions (e.g., "1024x1024") | `"1024x1024"` |
| `OutputFormat` | `string` | Image format (png, jpg, jpeg, webp) | `"png"` |
| `Seed` | `int?` | Seed for reproducible generation | `null` |

### Response Handling

```csharp
var response = await client.StableImageUltra.GenerateImageAsync("prompt");

// Get image as byte array
byte[] imageBytes = response.GetImageBytes();

// Save to file
await response.SaveImageAsync("output.png");

// Access metadata (if available)
if (response.Metadata != null)
{
    var width = response.Metadata.Width;
    var height = response.Metadata.Height;
    var format = response.Metadata.Format;
    var seed = response.Metadata.Seed;
}
```

## Error Handling

The SDK provides comprehensive error handling with custom exceptions:

```csharp
try
{
    var response = await client.StableImageUltra.GenerateImageAsync("prompt");
}
catch (AzureImageException ex)
{
    Console.WriteLine($"Azure Image Error: {ex.Message}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid parameters: {ex.Message}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Network error: {ex.Message}");
}
```

## Logging

The SDK integrates with Microsoft.Extensions.Logging:

```csharp
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

// The SDK will automatically use the configured logger
services.AddAzureImage(configuration.GetSection("AzureImage"));
```

## Authentication

Currently supports API key authentication. The API key should be obtained from your Azure AI service deployment.

## Development

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Azure AI service with Stable Image Ultra deployment

### Building

```bash
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Running Sample

1. Update `samples/SampleProject/appsettings.json` with your endpoint and API key
2. Run the sample:

```bash
cd samples/SampleProject
dotnet run
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation for API changes
- Ensure all tests pass before submitting PR

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 📖 [Documentation & Wiki](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- 📋 [Changelog](CHANGELOG.md)
- 🐛 [Issue Tracker](https://github.com/DrHazemAli/AzureImageSDK/issues)
- 💬 [Discussions](https://github.com/DrHazemAli/AzureImageSDK/discussions)
- 🔗 [GitHub Repository](https://github.com/DrHazemAli/AzureImageSDK)

## Acknowledgments

- Azure AI team for providing the APIs
- .NET community for excellent tooling and libraries
- Contributors and users of this SDK