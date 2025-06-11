# StableImageUltra Model

StableImageUltra is a powerful image generation model that creates high-quality images from text prompts. This guide covers everything you need to know about using StableImageUltra with the Azure AI SDK.

## Overview

StableImageUltra provides advanced image generation capabilities with support for:
- High-quality image generation
- Negative prompts for better control
- Multiple output formats (PNG, JPG, JPEG, WebP)
- Flexible image sizes
- Reproducible generation with seeds

## Quick Start

```csharp
using AzureAISDK.Core;
using AzureAISDK.Inference.Image.StableImageUltra;

// Create model
var model = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key");

// Create client
using var client = AzureAIClient.Create();

// Generate image
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A serene mountain landscape at sunset"
};

var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

// Save image
await response.SaveImageAsync("landscape.png");
```

## Configuration Options

### Basic Configuration

```csharp
var model = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key");
```

### Advanced Configuration

```csharp
var model = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key",
    options =>
    {
        options.ModelName = "Stable-Image-Ultra-v2";
        options.ApiVersion = "2024-05-01-preview";
        options.DefaultSize = "1024x1024";
        options.DefaultOutputFormat = "png";
        options.Timeout = TimeSpan.FromMinutes(5);
        options.MaxRetryAttempts = 3;
        options.RetryDelay = TimeSpan.FromSeconds(2);
    });
```

### Configuration Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Endpoint` | `string` | Required | Azure AI service endpoint URL |
| `ApiKey` | `string` | Required | API key for authentication |
| `ModelName` | `string` | `"Stable-Image-Ultra"` | Model identifier |
| `ApiVersion` | `string` | `"2024-05-01-preview"` | API version to use |
| `DefaultSize` | `string` | `"1024x1024"` | Default image dimensions |
| `DefaultOutputFormat` | `string` | `"png"` | Default output format |
| `Timeout` | `TimeSpan` | 5 minutes | Request timeout |
| `MaxRetryAttempts` | `int` | 3 | Maximum retry attempts |
| `RetryDelay` | `TimeSpan` | 1 second | Base retry delay |

## Request Parameters

### ImageGenerationRequest Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Model` | `string` | ✅ | Model name (auto-set by SDK) |
| `Prompt` | `string` | ✅ | Text description of desired image |
| `NegativePrompt` | `string?` | ❌ | What to avoid in the image |
| `Size` | `string` | ❌ | Image dimensions (e.g., "1024x1024") |
| `OutputFormat` | `string` | ❌ | Output format (png, jpg, jpeg, webp) |
| `Seed` | `int?` | ❌ | Seed for reproducible generation |

### Example Requests

#### Basic Request
```csharp
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A cute cat sitting in a garden"
};
```

#### Advanced Request
```csharp
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A futuristic cyberpunk cityscape with neon lights",
    NegativePrompt = "blurry, low quality, distorted, watermark",
    Size = "1024x1024",
    OutputFormat = "png",
    Seed = 42
};
```

## Response Handling

### ImageGenerationResponse

```csharp
var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

// Get image as bytes
byte[] imageBytes = response.GetImageBytes();

// Save to file
await response.SaveImageAsync("output.png");

// Access metadata
if (response.Metadata != null)
{
    Console.WriteLine($"Width: {response.Metadata.Width}");
    Console.WriteLine($"Height: {response.Metadata.Height}");
    Console.WriteLine($"Format: {response.Metadata.Format}");
    Console.WriteLine($"Seed: {response.Metadata.Seed}");
}
```

### Response Properties

| Property | Type | Description |
|----------|------|-------------|
| `Image` | `string` | Base64-encoded image data |
| `Metadata` | `ImageMetadata?` | Optional metadata about generation |

### ImageMetadata Properties

| Property | Type | Description |
|----------|------|-------------|
| `Width` | `int?` | Image width in pixels |
| `Height` | `int?` | Image height in pixels |
| `Format` | `string?` | Image format |
| `Seed` | `int?` | Seed used for generation |
| `Model` | `string?` | Model used for generation |

## Supported Image Sizes

StableImageUltra supports various image dimensions:

| Size | Aspect Ratio | Use Case |
|------|--------------|----------|
| `512x512` | 1:1 | Square, fast generation |
| `768x768` | 1:1 | Square, better quality |
| `1024x1024` | 1:1 | Square, high quality |
| `1024x768` | 4:3 | Landscape |
| `768x1024` | 3:4 | Portrait |
| `1920x1080` | 16:9 | Widescreen |
| `1080x1920` | 9:16 | Mobile portrait |

## Supported Output Formats

| Format | Extension | Use Case |
|--------|-----------|----------|
| `png` | `.png` | Lossless, transparency support |
| `jpg` | `.jpg` | Lossy compression, smaller files |
| `jpeg` | `.jpeg` | Same as jpg |
| `webp` | `.webp` | Modern format, good compression |

## Best Practices

### Prompt Engineering

#### Good Prompts
- Be specific and descriptive
- Include style information
- Mention desired mood or atmosphere
- Specify important details

```csharp
// Good example
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A majestic golden retriever sitting in a blooming cherry blossom garden during spring, soft natural lighting, photorealistic style, high detail"
};
```

#### Negative Prompts
Use negative prompts to avoid unwanted elements:

```csharp
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A beautiful landscape painting",
    NegativePrompt = "blurry, low quality, distorted, ugly, bad anatomy, extra limbs, watermark, signature, text"
};
```

### Performance Optimization

#### Choose Appropriate Size
- Use smaller sizes (512x512) for faster generation
- Use larger sizes (1024x1024) for final high-quality images

#### Use Seeds for Consistency
```csharp
// Generate consistent variations
var baseRequest = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A cozy cabin in the woods",
    Seed = 12345
};
```

#### Batch Processing
```csharp
var prompts = new[] { "landscape", "portrait", "abstract art" };
var tasks = prompts.Select(async prompt =>
{
    var request = new ImageGenerationRequest
    {
        Model = model.ModelName,
        Prompt = prompt
    };
    return await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
        model, request);
});

var responses = await Task.WhenAll(tasks);
```

## Error Handling

```csharp
try
{
    var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
        model, request);
    
    await response.SaveImageAsync("output.png");
}
catch (AzureAIException ex)
{
    Console.WriteLine($"Azure AI Error: {ex.Message}");
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

## Common Error Codes

| Status Code | Error Code | Description | Solution |
|-------------|------------|-------------|----------|
| 400 | `InvalidRequest` | Malformed request | Check request parameters |
| 401 | `Unauthorized` | Invalid API key | Verify API key |
| 403 | `Forbidden` | Access denied | Check permissions |
| 429 | `RateLimited` | Too many requests | Implement backoff |
| 500 | `InternalError` | Server error | Retry request |

## Examples

### Basic Image Generation
```csharp
var model = StableImageUltraModel.Create(endpoint, apiKey);
using var client = AzureAIClient.Create();

var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A peaceful zen garden with cherry blossoms"
};

var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

await response.SaveImageAsync("zen_garden.png");
```

### Advanced Generation with All Parameters
```csharp
var model = StableImageUltraModel.Create(endpoint, apiKey, options =>
{
    options.DefaultSize = "1024x768";
    options.DefaultOutputFormat = "webp";
});

using var client = AzureAIClient.Create();

var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A steampunk airship flying over a Victorian city, detailed brass machinery, copper pipes, steam clouds, dramatic lighting, dieselpunk aesthetic",
    NegativePrompt = "blurry, low quality, modern elements, cars, smartphones, contemporary clothing",
    Size = "1920x1080",
    OutputFormat = "png",
    Seed = 987654321
};

var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

Console.WriteLine($"Generated image with seed: {response.Metadata?.Seed}");
await response.SaveImageAsync("steampunk_airship.png");
```

## Sample Projects

- [Basic StableImageUltra Sample](../../samples/image-generation/StableImageUltra/README.md) - Simple image generation
- [Advanced Scenarios](../../examples/Advanced-Scenarios.md) - Complex use cases
- [Batch Processing](../../examples/Batch-Processing.md) - Generate multiple images

## Related Documentation

- [Model-Based Architecture](../../architecture/Model-Based-Architecture.md)
- [Configuration Guide](../../Configuration.md)
- [Error Handling](../../Error-Handling.md)
- [API Reference](../../api/Model-Interfaces.md) 