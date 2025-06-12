# Azure Image SDK

<div align="center">

[![NuGet](https://img.shields.io/nuget/v/AzureImage.svg)](https://www.nuget.org/packages/AzureImage/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AzureImage.svg)](https://www.nuget.org/packages/AzureImage/)
[![GitHub](https://img.shields.io/github/license/DrHazemAli/AzureImageSDK)](LICENSE)
[![Wiki](https://img.shields.io/badge/docs-wiki-blue.svg)](https://github.com/DrHazemAli/AzureImageSDK/wiki)
[![Changelog](https://img.shields.io/badge/changelog-releases-green.svg)](CHANGELOG.md)
[![.NET](https://img.shields.io/badge/.NET-6.0%2B-512BD4)](https://dotnet.microsoft.com/)

**A powerful, comprehensive .NET SDK for Azure AI Foundry image services**

*Seamlessly integrate advanced AI-powered image generation, analysis, and processing capabilities into your applications*

![Azure Image SDK](https://raw.githubusercontent.com/DrHazemAli/AzureImageSDK/refs/heads/main/azure_image_sdk.jpg)

[**🚀 Quick Start**](#quick-start) • [**📖 Documentation**](https://github.com/DrHazemAli/AzureImageSDK/wiki) • [**🔧 API Reference**](#comprehensive-api-reference) • [**💬 Discussions**](https://github.com/DrHazemAli/AzureImageSDK/discussions)

</div>

---

## 🌟 **What's New in v1.0.1**

- ✨ **NEW: DALL-E 3 Support** - Advanced OpenAI image generation with natural and vivid styles
- ✨ **NEW: GPT-Image-1 Support** - Next-generation image generation and editing capabilities  
- 🔧 **Enhanced Utilities** - Comprehensive image processing toolset with quality analysis
- 📚 **Complete Documentation** - Detailed wikis for all utilities and features
- 📦 **NuGet Enhancements** - README now included in NuGet package

> 📝 **[View complete changelog](CHANGELOG.md)** for detailed release notes and migration guides.

---

## 🎯 **Overview**

Azure Image SDK is a modern, feature-rich .NET library that enables developers to easily integrate Azure AI Foundry's cutting-edge image capabilities into their applications. Supporting .NET 6.0+, it provides a unified, type-safe, and performant way to work with multiple AI-powered image services.

### ⚡ **Why Choose Azure Image SDK?**

- 🎨 **Multi-Model Support** - Unified API for Stable Image Ultra/Core, DALL-E 3, and GPT-Image-1
- 🛠️ **Rich Utilities** - Complete image processing toolkit built-in
- 🔌 **DI Ready** - First-class dependency injection support
- 🔄 **Resilient** - Built-in retry logic and comprehensive error handling
- 📊 **Observable** - Integrated logging and monitoring capabilities
- 🧪 **Well Tested** - Comprehensive test coverage for production reliability

---

## ✨ **Features**

### 🎨 **AI-Powered Image Generation**
| Model | Capabilities | Best For |
|-------|-------------|----------|
| **🚀 DALL-E 3** | Natural & vivid styles, high-quality outputs | Creative projects, marketing content |
| **⚡ GPT-Image-1** | Generation + editing, multiple formats | Versatile applications, content editing |
| **🎯 Stable Image Ultra** | Advanced customization, precise control | Professional workflows, branded content |
| **💎 Stable Image Core** | Essential generation, fast processing | High-volume applications, prototyping |

#### **Key Generation Features**
- 🎛️ **Advanced Parameters** - Size, style, quality, seeds, negative prompts
- 🔄 **Multiple Formats** - PNG, JPEG, WebP support
- 📐 **Flexible Sizing** - Square, landscape, portrait orientations
- 🎭 **Style Control** - Natural, vivid, artistic variations
- 🌱 **Reproducible** - Seed-based generation for consistency

### 👁️ **Computer Vision & AI Analysis**
- **🏷️ Azure Vision Captioning** - Generate descriptive captions with confidence scores
- **🎯 Dense Captioning** - Multiple region-specific captions with bounding boxes
- **🌐 Multi-Input Support** - Process images from streams, URLs, or file paths
- **🗣️ Language Support** - Multiple language options for international applications
- **⚖️ Gender-Neutral Options** - Inclusive captioning for diverse content

### 🛠️ **Professional Image Processing Utilities**
- **🔄 Format Conversion** - Universal format and MIME type conversion (PNG, JPG, WebP, etc.)
- **📊 Quality Analysis** - Advanced sharpness, brightness, contrast, and overall quality scoring
- **📋 Metadata Extraction** - Comprehensive EXIF data and image properties extraction
- **📏 Size Validation** - Smart dimension validation with customizable constraints
- **📐 Aspect Ratio Tools** - Professional aspect ratio calculation and validation

### 🏗️ **Enterprise-Grade Architecture**
- **🧩 Modular Design** - Easily extendable with new AI services and models
- **💉 Dependency Injection** - Full ASP.NET Core and .NET Host integration
- **🔄 Resilient Operations** - Exponential backoff retry with configurable policies
- **🚨 Error Handling** - Comprehensive exception system with detailed diagnostics
- **📝 Logging Integration** - Microsoft.Extensions.Logging with structured data
- **🔒 Type Safety** - Strongly typed models, requests, and responses throughout
- **⚡ Async/Await** - Fully asynchronous operations for optimal performance
- **⚙️ Flexible Configuration** - appsettings.json, environment variables, and fluent APIs

---

## 📦 **Installation**

### Package Manager Console
```powershell
Install-Package AzureImage
```

### .NET CLI
```bash
dotnet add package AzureImage
```

### PackageReference
```xml
<PackageReference Include="AzureImage" Version="1.0.1" />
```

### Supported Frameworks
- ✅ .NET 6.0+
- ✅ .NET 7.0+  
- ✅ .NET 8.0+

---

## 🚀 **Quick Start**

### 1. **🎨 Image Generation - DALL-E 3**

```csharp
using AzureImage.Core;
using AzureImage.Inference.Models.DALLE3;

// Create DALL-E 3 model
var dalle3Model = DALLE3Model.Create(
    endpoint: "https://your-resource.openai.azure.com",
    apiKey: "your-api-key",
    deploymentName: "dalle3-deployment");

// Generate with advanced options
var request = new ImageGenerationRequest
{
    Prompt = "A majestic mountain landscape at sunset, painted in impressionist style",
    Size = "1024x1024",
    Quality = "hd",
    Style = "vivid"
};

var response = await client.GenerateImageAsync(dalle3Model, request);
await response.SaveImageAsync("masterpiece.png");

Console.WriteLine($"Generated with revised prompt: {response.RevisedPrompt}");
```

### 2. **⚡ Image Generation & Editing - GPT-Image-1**

```csharp
using AzureImage.Inference.Models.GPTImage1;

// Create GPT-Image-1 model (supports both generation and editing)
var gptImageModel = GPTImage1Model.Create(
    endpoint: "https://your-resource.openai.azure.com",
    apiKey: "your-api-key", 
    deploymentName: "gpt-image-1-deployment");

// Generate new image
var genRequest = new ImageGenerationRequest
{
    Prompt = "A modern office space with natural lighting",
    Size = "1024x1024",
    Quality = "high",
    OutputFormat = "png"
};

var genResponse = await client.GenerateImageAsync(gptImageModel, genRequest);

// Edit existing image
var editRequest = new ImageEditingRequest
{
    Image = File.ReadAllBytes("original.png"),
    Prompt = "Add a beautiful plant in the corner",
    Size = "1024x1024"
};

var editResponse = await client.EditImageAsync(gptImageModel, editRequest);
await editResponse.SaveImageAsync("edited.png");
```

### 3. **🎯 Professional Image Generation - Stable Image Ultra**

```csharp
using AzureImage.Inference.Models.StableImageUltra;

// Create Stable Image Ultra model with advanced configuration
var ultraModel = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key",
    options =>
    {
        options.ApiVersion = "2024-05-01-preview";
        options.DefaultSize = "1024x1024";
        options.DefaultOutputFormat = "png";
        options.Timeout = TimeSpan.FromMinutes(5);
        options.MaxRetryAttempts = 3;
    });

// Generate with fine-grained control
var request = new ImageGenerationRequest
{
    Prompt = "A professional product photo of a luxury watch on marble surface",
    NegativePrompt = "blurry, low quality, distorted, watermark",
    Size = "1024x1024",
    OutputFormat = "png",
    Seed = 42 // For reproducible results
};

var response = await client.GenerateImageAsync(ultraModel, request);

// Access comprehensive metadata
if (response.Metadata != null)
{
    Console.WriteLine($"Generated: {response.Metadata.Width}x{response.Metadata.Height}");
    Console.WriteLine($"Seed used: {response.Metadata.Seed}");
    Console.WriteLine($"Format: {response.Metadata.Format}");
}
```

### 4. **👁️ Computer Vision - Image Analysis**

```csharp
using AzureImage.Inference.Models.AzureVisionCaptioning;

// Create Azure Vision model
var visionModel = AzureVisionCaptioningModel.Create(
    endpoint: "https://your-vision.cognitiveservices.azure.com",
    apiKey: "your-vision-api-key");

// Analyze image from URL
var captionResult = await visionModel.GenerateCaptionAsync(
    "https://example.com/image.jpg");

Console.WriteLine($"Caption: {captionResult.Caption.Text}");
Console.WriteLine($"Confidence: {captionResult.Caption.Confidence:P2}");

// Generate detailed dense captions
var denseCaptions = await visionModel.GenerateDenseCaptionsAsync(imageStream);
foreach (var caption in denseCaptions.Captions)
{
    Console.WriteLine($"Region: {caption.Text} (Confidence: {caption.Confidence:P2})");
    Console.WriteLine($"Location: X={caption.BoundingBox.X}, Y={caption.BoundingBox.Y}");
}
```

### 5. **🛠️ Professional Image Processing**

```csharp
using AzureImage.Utilities;

// Format conversion and validation
string mimeType = ImageFormatConverter.GetMimeType(".jpg"); // "image/jpeg"
bool isValidFormat = ImageFormatConverter.IsValidImageFormat("webp"); // true
string[] supportedFormats = ImageFormatConverter.GetSupportedFormats();

// Comprehensive quality analysis
using var imageStream = File.OpenRead("photo.jpg");
var qualityMetrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(imageStream);

Console.WriteLine($"📊 Quality Analysis:");
Console.WriteLine($"   Sharpness: {qualityMetrics.Sharpness:P2}");
Console.WriteLine($"   Brightness: {qualityMetrics.Brightness:P2}");
Console.WriteLine($"   Contrast: {qualityMetrics.Contrast:P2}");
Console.WriteLine($"   Overall Score: {qualityMetrics.OverallScore:P2}");

// Extract detailed metadata
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(imageStream);
Console.WriteLine($"📋 Metadata:");
Console.WriteLine($"   Dimensions: {metadata.Width}x{metadata.Height}");
Console.WriteLine($"   Format: {metadata.Format}");
Console.WriteLine($"   File Size: {metadata.Size:N0} bytes");

// Smart size validation with constraints
var constraints = new SizeConstraints
{
    MinWidth = 320, MaxWidth = 1920,
    MinHeight = 240, MaxHeight = 1080,
    MinAspectRatio = 0.5, MaxAspectRatio = 2.0
};

bool isValidSize = ImageSizeValidator.IsWithinBounds(
    metadata.Width, metadata.Height, constraints);

// Intelligent scaling while preserving aspect ratio
var (scaledWidth, scaledHeight) = ImageSizeValidator.ScaleToFit(
    metadata.Width, metadata.Height, 1024, 1024);
```

---

## ⚙️ **Configuration**

### 📝 **appsettings.json Configuration**

```json
{
  "AzureImage": {
    "DALLE3": {
      "Endpoint": "https://your-resource.openai.azure.com",
      "ApiKey": "your-dalle3-api-key",
      "DeploymentName": "dalle3-deployment",
      "ApiVersion": "2024-02-01",
      "DefaultSize": "1024x1024",
      "DefaultQuality": "hd",
      "DefaultStyle": "vivid"
    },
    "GPTImage1": {
      "Endpoint": "https://your-resource.openai.azure.com", 
      "ApiKey": "your-gpt-image-api-key",
      "DeploymentName": "gpt-image-1-deployment",
      "ApiVersion": "2025-04-01-preview",
      "DefaultSize": "1024x1024",
      "DefaultQuality": "high"
    },
    "StableImageUltra": {
      "Endpoint": "https://your-stable-image-ultra.eastus.models.ai.azure.com",
      "ApiKey": "your-stable-image-ultra-api-key",
      "ApiVersion": "2024-05-01-preview",
      "DefaultSize": "1024x1024",
      "DefaultOutputFormat": "png"
    },
    "StableImageCore": {
      "Endpoint": "https://your-stable-image-core.eastus.models.ai.azure.com",
      "ApiKey": "your-stable-image-core-api-key"
    },
    "AzureVisionCaptioning": {
      "Endpoint": "https://your-vision.cognitiveservices.azure.com",
      "ApiKey": "your-vision-api-key",
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

### 🌐 **Environment Variables**

```bash
# DALL-E 3
export AZURE_IMAGE_DALLE3_ENDPOINT="https://your-resource.openai.azure.com"
export AZURE_IMAGE_DALLE3_API_KEY="your-dalle3-api-key"
export AZURE_IMAGE_DALLE3_DEPLOYMENT_NAME="dalle3-deployment"

# GPT-Image-1  
export AZURE_IMAGE_GPTIMAGE1_ENDPOINT="https://your-resource.openai.azure.com"
export AZURE_IMAGE_GPTIMAGE1_API_KEY="your-gpt-image-api-key"
export AZURE_IMAGE_GPTIMAGE1_DEPLOYMENT_NAME="gpt-image-1-deployment"

# Stable Image Ultra
export AZURE_IMAGE_STABLE_IMAGE_ULTRA_ENDPOINT="https://your-endpoint.eastus.models.ai.azure.com"
export AZURE_IMAGE_STABLE_IMAGE_ULTRA_API_KEY="your-api-key"
```

### 💉 **Dependency Injection Setup**

```csharp
using AzureImage.Extensions;

// In Program.cs or Startup.cs
builder.Services.AddAzureImage(options =>
{
    options.DALLE3.Endpoint = "https://your-resource.openai.azure.com";
    options.DALLE3.ApiKey = "your-api-key";
    options.DALLE3.DeploymentName = "dalle3-deployment";
    
    options.GPTImage1.Endpoint = "https://your-resource.openai.azure.com";
    options.GPTImage1.ApiKey = "your-api-key";
    options.GPTImage1.DeploymentName = "gpt-image-1-deployment";
    
    options.Timeout = TimeSpan.FromMinutes(5);
    options.MaxRetryAttempts = 3;
});

// Or load from configuration
builder.Services.AddAzureImage(
    builder.Configuration.GetSection("AzureImage"));
```

### 🎮 **Using in Controllers/Services**

```csharp
[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IAzureImageClient _azureImageClient;

    public ImageController(IAzureImageClient azureImageClient)
    {
        _azureImageClient = azureImageClient;
    }

    [HttpPost("generate/dalle3")]
    public async Task<IActionResult> GenerateWithDALLE3([FromBody] GenerateImageRequest request)
    {
        var dalle3Model = DALLE3Model.Create(/* configuration */);
        var response = await _azureImageClient.GenerateImageAsync(dalle3Model, request);
        return File(response.GetImageBytes(), "image/png", "generated-image.png");
    }

    [HttpPost("edit/gpt-image")]
    public async Task<IActionResult> EditWithGPTImage(IFormFile image, [FromForm] string prompt)
    {
        var gptImageModel = GPTImage1Model.Create(/* configuration */);
        var editRequest = new ImageEditingRequest
        {
            Image = await image.GetBytesAsync(),
            Prompt = prompt
        };
        
        var response = await _azureImageClient.EditImageAsync(gptImageModel, editRequest);
        return File(response.GetImageBytes(), "image/png", "edited-image.png");
    }
}
```

---

## 📊 **Comprehensive API Reference**

### 🎨 **Image Generation Models Comparison**

| Feature | DALL-E 3 | GPT-Image-1 | Stable Image Ultra | Stable Image Core |
|---------|----------|-------------|-------------------|-------------------|
| **Generation** | ✅ Advanced | ✅ Advanced | ✅ Professional | ✅ Essential |
| **Editing** | ❌ | ✅ Full Support | ❌ | ❌ |
| **Styles** | Natural, Vivid | Multiple | Custom | Standard |
| **Sizes** | 1024x1024, 1792x1024, 1024x1792 | 1024x1024, 1024x1536, 1536x1024 | Flexible | Flexible |
| **Quality Options** | Standard, HD | Low, Medium, High | Custom | Standard |
| **Batch Generation** | Single (n=1) | 1-10 images | Single | Single |
| **Response Format** | URL, Base64 | PNG, JPEG | Multiple | Multiple |

### 🎛️ **Model-Specific Parameters**

#### **DALL-E 3 Parameters**
```csharp
var request = new DALLE3.ImageGenerationRequest
{
    Prompt = "Your creative prompt",
    Size = "1024x1024", // 1024x1024, 1792x1024, 1024x1792
    Quality = "hd",      // standard, hd
    Style = "vivid",     // natural, vivid
    ResponseFormat = "url" // url, b64_json
};
```

#### **GPT-Image-1 Parameters**
```csharp
// Generation
var genRequest = new GPTImage1.ImageGenerationRequest
{
    Prompt = "Your detailed prompt",
    Size = "1024x1024",     // 1024x1024, 1024x1536, 1536x1024
    Quality = "high",       // low, medium, high  
    N = 1,                  // 1-10 images
    OutputFormat = "png",   // png, jpeg
    OutputCompression = 100, // 0-100
    User = "user-123"       // Optional user ID
};

// Editing
var editRequest = new GPTImage1.ImageEditingRequest
{
    Image = imageBytes,     // Original image as byte array
    Mask = maskBytes,       // Optional mask (PNG, same dimensions)
    Prompt = "Edit description",
    Size = "1024x1024",
    Quality = "high"
};
```

#### **Stable Image Ultra Parameters**
```csharp
var request = new StableImageUltra.ImageGenerationRequest
{
    Prompt = "Professional prompt",
    NegativePrompt = "unwanted, blurry, low quality",
    Size = "1024x1024",
    OutputFormat = "png",   // png, jpg, jpeg, webp
    Seed = 42               // For reproducible results
};
```

### 👁️ **Computer Vision APIs**

#### **Azure Vision Captioning**
```csharp
// Single caption
var visionModel = AzureVisionCaptioningModel.Create(endpoint, apiKey);
var caption = await visionModel.GenerateCaptionAsync(imageStream);

// With options
var options = new ImageCaptionOptions
{
    Language = "en",              // Language code
    GenderNeutralCaption = true   // Inclusive language
};
var caption = await visionModel.GenerateCaptionAsync(imageStream, options);

// Dense captioning (multiple regions)
var denseCaptions = await visionModel.GenerateDenseCaptionsAsync(imageStream);
```

### 🛠️ **Utility APIs Deep Dive**

#### **📋 Image Format Converter**
```csharp
// Format detection and conversion
string mimeType = ImageFormatConverter.GetMimeType(".jpg");         // "image/jpeg"
string extension = ImageFormatConverter.GetFileExtension("image/png"); // ".png"
bool isSupported = ImageFormatConverter.IsValidImageFormat("webp");    // true
string[] formats = ImageFormatConverter.GetSupportedFormats();         // All supported
string[] mimeTypes = ImageFormatConverter.GetSupportedMimeTypes();     // All MIME types
```

#### **📊 Image Quality Analyzer**
```csharp
// Individual metrics
double sharpness = await ImageQualityAnalyzer.CalculateSharpnessAsync(stream);   // 0.0-1.0
double brightness = await ImageQualityAnalyzer.CalculateBrightnessAsync(stream); // 0.0-1.0  
double contrast = await ImageQualityAnalyzer.CalculateContrastAsync(stream);     // 0.0-1.0

// Comprehensive analysis
var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
Console.WriteLine($"Overall Quality: {metrics.OverallScore:P2}");
```

#### **📋 Image Metadata Extractor**  
```csharp
// Extract comprehensive metadata
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(filePath);
Console.WriteLine($"Dimensions: {metadata.Width}x{metadata.Height}");
Console.WriteLine($"Format: {metadata.Format}");
Console.WriteLine($"File Size: {metadata.Size:N0} bytes");
Console.WriteLine($"Created: {metadata.CreationDate}");

// Validate metadata integrity
bool hasValidMetadata = await ImageMetadataExtractor.HasValidMetadataAsync(stream);
```

#### **📏 Image Size Validator**
```csharp
// Simple validation
bool isValid = ImageSizeValidator.IsValidSize(1920, 1080, 2048, 2048);

// Advanced constraint validation
var constraints = new SizeConstraints
{
    MinWidth = 320, MaxWidth = 1920,
    MinHeight = 240, MaxHeight = 1080,
    MinAspectRatio = 0.5, MaxAspectRatio = 2.0
};
bool withinBounds = ImageSizeValidator.IsWithinBounds(width, height, constraints);

// Intelligent scaling
var (fitWidth, fitHeight) = ImageSizeValidator.ScaleToFit(3000, 2000, 1024, 1024);
var (fillWidth, fillHeight) = ImageSizeValidator.ScaleToFill(1920, 1080, 800, 600);
```

#### **📐 Aspect Ratio Converter** 
```csharp
// Calculate ratios
double ratio = AspectRatioConverter.CalculateAspectRatio(1920, 1080);              // 1.777...
var dimensions = AspectRatioConverter.GetDimensionsForAspectRatio(16.0/9.0, 1920); // (1920, 1080)
bool isValid = AspectRatioConverter.IsValidAspectRatio(1.77, tolerance: 0.01);     // true
```

---

## 🔧 **Advanced Usage Examples**

### 🎨 **Multi-Model Image Generation Pipeline**

```csharp
public class ImageGenerationPipeline
{
    private readonly IAzureImageClient _client;
    private readonly DALLE3Model _dalle3;
    private readonly GPTImage1Model _gptImage1; 
    private readonly StableImageUltraModel _stableUltra;

    public async Task<GenerationResult> GenerateWithFallback(string prompt)
    {
        var strategies = new[]
        {
            (Model: _dalle3, Name: "DALL-E 3"),
            (Model: _gptImage1, Name: "GPT-Image-1"), 
            (Model: _stableUltra, Name: "Stable Image Ultra")
        };

        foreach (var (model, name) in strategies)
        {
            try
            {
                Console.WriteLine($"Attempting generation with {name}...");
                var response = await _client.GenerateImageAsync(model, new { Prompt = prompt });
                
                // Analyze quality
                using var stream = new MemoryStream(response.GetImageBytes());
                var quality = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
                
                if (quality.OverallScore > 0.7) // Quality threshold
                {
                    return new GenerationResult
                    {
                        Success = true,
                        ModelUsed = name,
                        ImageData = response.GetImageBytes(),
                        QualityScore = quality.OverallScore
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{name} failed: {ex.Message}");
            }
        }

        return new GenerationResult { Success = false };
    }
}
```

### 📊 **Batch Image Quality Assessment**

```csharp
public class BatchImageProcessor
{
    public async Task<QualityReport> AnalyzeImageCollection(string[] imagePaths)
    {
        var results = new List<ImageAnalysisResult>();
        var semaphore = new SemaphoreSlim(Environment.ProcessorCount); // Limit concurrency

        var tasks = imagePaths.Select(async path =>
        {
            await semaphore.WaitAsync();
            try
            {
                return await AnalyzeSingleImage(path);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var analysisResults = await Task.WhenAll(tasks);
        
        return new QualityReport
        {
            TotalImages = analysisResults.Length,
            AverageQuality = analysisResults.Average(r => r.QualityScore),
            HighQualityCount = analysisResults.Count(r => r.QualityScore > 0.8),
            LowQualityImages = analysisResults
                .Where(r => r.QualityScore < 0.5)
                .Select(r => r.FilePath)
                .ToList()
        };
    }

    private async Task<ImageAnalysisResult> AnalyzeSingleImage(string path)
    {
        using var stream = File.OpenRead(path);
        
        // Extract metadata
        var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(stream);
        
        // Analyze quality
        stream.Position = 0;
        var quality = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
        
        // Validate dimensions
        var constraints = new SizeConstraints { MinWidth = 100, MaxWidth = 4096 };
        bool validSize = ImageSizeValidator.IsWithinBounds(
            metadata.Width, metadata.Height, constraints);

        return new ImageAnalysisResult
        {
            FilePath = path,
            Metadata = metadata,
            QualityScore = quality.OverallScore,
            IsValidSize = validSize,
            Format = metadata.Format
        };
    }
}
```

---

## 🚨 **Error Handling & Diagnostics**

The SDK provides comprehensive error handling with detailed diagnostics:

```csharp
try
{
    var response = await client.GenerateImageAsync(model, request);
}
catch (AzureImageException ex)
{
    // Azure Image SDK specific errors
    Console.WriteLine($"SDK Error: {ex.Message}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Request ID: {ex.RequestId}");
}
catch (ArgumentException ex)
{
    // Parameter validation errors
    Console.WriteLine($"Invalid parameters: {ex.Message}");
}
catch (HttpRequestException ex)
{
    // Network connectivity errors
    Console.WriteLine($"Network error: {ex.Message}");
}
catch (TaskCanceledException ex)
{
    // Timeout errors
    Console.WriteLine($"Request timeout: {ex.Message}");
}
```

### 📝 **Structured Logging**

```csharp
// Configure logging in Program.cs
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddApplicationInsights(); // Optional: Azure Application Insights
    logging.SetMinimumLevel(LogLevel.Information);
});

// The SDK automatically logs:
// - Request/response details
// - Performance metrics  
// - Error diagnostics
// - Quality analysis results
// - Retry attempts
```

---

## 🧪 **Testing & Development**

### 📦 **Running the Samples**

```bash
# Clone the repository
git clone https://github.com/DrHazemAli/AzureImageSDK.git
cd AzureImageSDK

# Navigate to samples
cd samples/SampleProject

# Configure your API keys in appsettings.json
# Then run the sample
dotnet run
```

### 🔧 **Building from Source**

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Create NuGet package
dotnet pack --configuration Release
```

---

## 📚 **Documentation & Resources**

| Resource | Description | Link |
|----------|-------------|------|
| 📖 **Wiki** | Comprehensive documentation and guides | [GitHub Wiki](https://github.com/DrHazemAli/AzureImageSDK/wiki) |
| 🔧 **API Reference** | Detailed API documentation | [Wiki/API-Reference](https://github.com/DrHazemAli/AzureImageSDK/wiki/API-Reference) |
| 🚀 **Getting Started** | Step-by-step setup guides | [Wiki/Getting-Started](https://github.com/DrHazemAli/AzureImageSDK/wiki/Getting-Started) |
| 🛠️ **Utilities Guide** | Image processing utilities documentation | [Wiki/Utilities](https://github.com/DrHazemAli/AzureImageSDK/wiki/Utilities) |
| 💡 **Best Practices** | Performance and security recommendations | [Wiki/Best-Practices](https://github.com/DrHazemAli/AzureImageSDK/wiki/Best-Practices) |
| 🐛 **Troubleshooting** | Common issues and solutions | [Wiki/Troubleshooting](https://github.com/DrHazemAli/AzureImageSDK/wiki/Troubleshooting) |
| 📝 **Changelog** | Release notes and migration guides | [CHANGELOG.md](CHANGELOG.md) |

---

## 🤝 **Contributing**

We welcome contributions! Here's how you can help:

### 🔧 **Development Setup**

1. **Fork & Clone**
   ```bash
   git clone https://github.com/your-username/AzureImageSDK.git
   cd AzureImageSDK
   ```

2. **Install Dependencies**
   ```bash
   dotnet restore
   ```

3. **Create Feature Branch**
   ```bash
   git checkout -b feature/amazing-new-feature
   ```

4. **Make Changes & Test**
   ```bash
   dotnet build
   dotnet test
   ```

5. **Submit Pull Request**

### 📋 **Contribution Guidelines**

- ✅ Follow C# coding conventions and best practices
- ✅ Write comprehensive unit tests for new features  
- ✅ Update documentation for API changes
- ✅ Ensure all tests pass before submitting
- ✅ Add entry to [CHANGELOG.md](CHANGELOG.md) for significant changes

### 🐛 **Reporting Issues**

Found a bug? Have a feature request? Please:

1. **Search existing issues** to avoid duplicates
2. **Use issue templates** for consistency
3. **Provide detailed reproduction steps**
4. **Include environment information** (.NET version, OS, etc.)

---

## 📄 **License**

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 🙏 **Acknowledgments**

- **Azure AI Team** - For providing excellent AI APIs and documentation
- **.NET Community** - For outstanding tooling, libraries, and ecosystem
- **Contributors** - Everyone who has contributed code, documentation, or feedback
- **Users** - The developers and organizations using this SDK in production

---

## 📞 **Support & Community**

| Channel | Purpose | Link |
|---------|---------|------|
| 📖 **Documentation** | Comprehensive guides and API reference | [GitHub Wiki](https://github.com/DrHazemAli/AzureImageSDK/wiki) |
| 🐛 **Issues** | Bug reports and feature requests | [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues) |
| 💬 **Discussions** | Q&A, ideas, and community chat | [GitHub Discussions](https://github.com/DrHazemAli/AzureImageSDK/discussions) |
| 📝 **Changelog** | Release notes and breaking changes | [CHANGELOG.md](CHANGELOG.md) |
| 🔗 **NuGet** | Package downloads and versions | [NuGet Gallery](https://www.nuget.org/packages/AzureImage/) |

---

<div align="center">

**Made with ❤️ for the .NET community**

⭐ **Star this repository** if you find it useful!

[🚀 **Get Started**](#quick-start) • [📖 **Documentation**](https://github.com/DrHazemAli/AzureImageSDK/wiki) • [💬 **Discussions**](https://github.com/DrHazemAli/AzureImageSDK/discussions)

</div>