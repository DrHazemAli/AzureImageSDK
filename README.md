# Azure Image SDK

[![NuGet](https://img.shields.io/nuget/v/AzureImage.svg)](https://www.nuget.org/packages/AzureImage/)
[![GitHub](https://img.shields.io/github/license/DrHazemAli/AzureImage)](LICENSE)
[![Build Status](https://github.com/DrHazemAli/AzureImage/workflows/CI%2FCD%20Pipeline/badge.svg)](https://github.com/DrHazemAli/AzureImage/actions)
[![codecov](https://codecov.io/gh/DrHazemAli/AzureImage/branch/main/graph/badge.svg)](https://codecov.io/gh/DrHazemAli/AzureImage)

A comprehensive .NET SDK for Azure Image services, providing easy-to-use client libraries for image generation capabilities including Stable Image Ultra, Stable Image Core and more.

![Azure Image SDK](https://github.com/DrHazemAli/AzureImage/blob/main/azure_image_sdk.jpg)


Azure Image SDK is a .NET library that enables developers to easily integrate Azure AI Foundry's image capabilities into their applications. The SDK supports .NET 6.0 and above, providing a modern and efficient way to work with AI-powered image generation and manipulation.


## Features

- 🎨 **Stable Image Ultra** - Advanced image generation with customizable parameters
- 🔧 **Modular Architecture** - Easy to extend with new AI services
- 📦 **Dependency Injection** - Full support for .NET DI container
- 🔄 **Retry Logic** - Built-in exponential backoff retry mechanism
- 🛡️ **Error Handling** - Comprehensive exception handling
- 📝 **Logging** - Structured logging with Microsoft.Extensions.Logging
- 🎯 **Type Safety** - Strongly typed models and responses
- ⚡ **Async/Await** - Fully asynchronous API
- 🧪 **Well Tested** - Comprehensive unit test coverage

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

### Simple Usage

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

### Advanced Usage with Custom Parameters

```csharp
using AzureImage.Inference.Models.StableImageUltra;

// Create model with custom configuration
var model = StableImageUltraModel.Create(
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

// Create request with StableImageUltra-specific parameters
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A futuristic cyberpunk cityscape",
    NegativePrompt = "blurry, low quality",
    Size = "1024x1024",
    OutputFormat = "png",
    Seed = 42
};

var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

// Access metadata
if (response.Metadata != null)
{
    Console.WriteLine($"Generated with seed: {response.Metadata.Seed}");
    Console.WriteLine($"Dimensions: {response.Metadata.Width}x{response.Metadata.Height}");
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

## Configuration

### appsettings.json

```json
{
  "AzureImage": {
    "StableImageUltra": {
      "Endpoint": "https://your-stable-image-ultra-endpoint.eastus.models.ai.azure.com",
      "ApiKey": "your-stable-image-ultra-api-key-here"
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

## Roadmap

- [ ] Support for additional Azure AI services
- [ ] Authentication with Azure AD/Managed Identity
- [ ] Batch image generation
- [ ] Image-to-image generation
- [ ] Streaming responses
- [ ] Configuration validation
- [ ] Performance optimizations

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 📖 [Documentation](https://github.com/DrHazemAli/AzureImage/wiki)
- 🐛 [Issue Tracker](https://github.com/DrHazemAli/AzureImage/issues)
- 💬 [Discussions](https://github.com/DrHazemAli/AzureImage/discussions)

## Acknowledgments

- Azure AI team for providing the APIs
- .NET community for excellent tooling and libraries
- Contributors and users of this SDK