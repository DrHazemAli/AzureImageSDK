# Quick Start Guide

This guide will help you get started with the AzureImage SDK quickly. We'll cover the basic usage patterns and common scenarios.

## Basic Usage

### 1. Initialize the Client

```csharp
using AzureImage;
using AzureImage.Inference.Models;

// Create client instance
var client = new AzureImageClient(new AzureImageOptions
{
    Endpoint = "your-endpoint",
    ApiKey = "your-api-key"
});
```

### 2. Generate an Image

```csharp
// Basic image generation
var result = await client.GenerateImageAsync(new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra",
    Size = new ImageSize(1024, 1024)
});

// Access the generated image
var imageUrl = result.ImageUrl;
```

### 3. Generate Multiple Images

```csharp
// Generate multiple images
var results = await client.GenerateImagesAsync(new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra",
    Size = new ImageSize(1024, 1024),
    NumberOfImages = 4
});

// Process results
foreach (var result in results)
{
    Console.WriteLine($"Generated image URL: {result.ImageUrl}");
}
```

## Common Scenarios

### Image Generation with Parameters

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra",
    Size = new ImageSize(1024, 1024),
    Style = "photographic",
    Quality = ImageQuality.High,
    NegativePrompt = "blurry, low quality",
    Seed = 12345
};

var result = await client.GenerateImageAsync(request);
```

### Error Handling

```csharp
try
{
    var result = await client.GenerateImageAsync(request);
}
catch (AzureImageException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
    Console.WriteLine($"Request ID: {ex.RequestId}");
}
```

### Using Different Models

```csharp
// Using StableImageUltra
var ultraResult = await client.GenerateImageAsync(new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra"
});

// Using StableImageCore
var coreResult = await client.GenerateImageAsync(new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-core"
});
```

## ASP.NET Core Integration

### 1. Configure Services

```csharp
// Program.cs or Startup.cs
using AzureImage.Extensions;

public void ConfigureServices(IServiceCollection services)
{
    services.AddAzureImage(options =>
    {
        options.Endpoint = Configuration["AzureImage:Endpoint"];
        options.ApiKey = Configuration["AzureImage:ApiKey"];
    });
}
```

### 2. Use in Controllers

```csharp
public class ImageController : ControllerBase
{
    private readonly IAzureImageClient _client;

    public ImageController(IAzureImageClient client)
    {
        _client = client;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateImage([FromBody] ImageGenerationRequest request)
    {
        var result = await _client.GenerateImageAsync(request);
        return Ok(result);
    }
}
```

## Next Steps

- [Configuration Guide](Configuration.md)
- [API Reference](../API-Reference/IAzureImageClient.md)
- [Examples](../Examples/Basic-Usage.md)
- [Best Practices](../Best-Practices/Performance.md)

## Sample Projects

Check out our sample projects for more examples:

- [Basic Console Application](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples/SampleProject)
- [ASP.NET Core Web API](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples/image-generation)

## Support

If you need help:

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples) 