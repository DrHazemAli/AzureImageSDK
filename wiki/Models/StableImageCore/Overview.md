# StableImageCore Model

StableImageCore is a lightweight and efficient image generation model designed for general-purpose use cases.

## Overview

StableImageCore provides a balanced approach to image generation, offering good quality with faster generation times. It's ideal for applications that require quick image generation without compromising too much on quality.

## Features

- Fast image generation
- Standard resolution support
- Basic style presets
- Efficient resource usage
- Suitable for high-throughput scenarios

## Configuration

### Basic Configuration

```csharp
var options = new StableImageCoreOptions
{
    Endpoint = "your-core-endpoint",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-15-preview",
    DefaultSize = new ImageSize(512, 512),
    DefaultQuality = ImageQuality.Standard
};
```

### Configuration Properties

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| Endpoint | string | Model endpoint URL | Required |
| ApiKey | string | API key for authentication | Required |
| ApiVersion | string | API version to use | "2024-02-15-preview" |
| DefaultSize | ImageSize | Default image size | 512x512 |
| DefaultQuality | ImageQuality | Default image quality | Standard |

## Usage

### Basic Image Generation

```csharp
var client = new AzureImageClient(options);
var request = new ImageGenerationRequest
{
    Prompt = "A simple landscape with mountains",
    Model = "stable-image-core",
    Size = new ImageSize(512, 512)
};

var result = await client.GenerateImageAsync(request);
```

### Advanced Image Generation

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A simple landscape with mountains, basic style",
    Model = "stable-image-core",
    Size = new ImageSize(1024, 1024),
    Style = "basic",
    Quality = ImageQuality.Standard,
    NegativePrompt = "complex, detailed",
    Seed = 12345
};

var result = await client.GenerateImageAsync(request);
```

### Using Model-Specific Client

```csharp
var coreClient = new StableImageCoreClient(options);
var result = await coreClient.GenerateImageAsync(new StableImageCoreRequest
{
    Prompt = "A simple landscape with mountains",
    Size = new ImageSize(512, 512),
    Style = "basic"
});
```

## Supported Parameters

### Image Size

| Size | Description | Use Case |
|------|-------------|----------|
| 256x256 | Small | Icons, thumbnails |
| 512x512 | Medium | Standard images |
| 1024x1024 | Large | Higher quality images |

### Image Quality

| Quality | Description | Use Case |
|---------|-------------|----------|
| Basic | Minimal quality | Quick previews |
| Standard | Balanced quality | General purpose |
| Enhanced | Better quality | Improved results |

### Styles

| Style | Description |
|-------|-------------|
| basic | Simple, clean style |
| sketch | Sketch-like style |
| cartoon | Cartoon style |
| minimal | Minimalist style |

## Best Practices

1. **Prompt Engineering**
   - Keep prompts simple and clear
   - Avoid complex descriptions
   - Use basic style keywords

2. **Performance Optimization**
   - Use smaller image sizes for faster generation
   - Stick to standard quality for better performance
   - Consider batch processing for multiple images

3. **Error Handling**
   ```csharp
   try
   {
       var result = await client.GenerateImageAsync(request);
   }
   catch (StableImageCoreException ex)
   {
       // Handle model-specific errors
       Console.WriteLine($"Error: {ex.Message}");
       Console.WriteLine($"Error Code: {ex.ErrorCode}");
   }
   ```

## Limitations

- Maximum image size: 1024x1024
- Maximum prompt length: 500 characters
- Limited style options
- Basic quality settings

## Performance Considerations

- Faster generation times compared to Ultra model
- Lower resource usage
- Suitable for high-throughput scenarios
- Good for real-time applications

## Use Cases

1. **Quick Previews**
   - Generate quick image previews
   - Test different prompts
   - Iterate on ideas

2. **High-Throughput Applications**
   - Batch processing
   - Real-time generation
   - Multiple concurrent requests

3. **Resource-Constrained Environments**
   - Mobile applications
   - Web applications
   - Low-power devices

## Related Topics

- [Configuration Guide](../../Getting-Started/Configuration.md)
- [API Reference](../../API-Reference/IAzureImageClient.md)
- [Best Practices](../../Best-Practices/Performance.md)
- [Troubleshooting](../../Troubleshooting/Common-Issues.md)

## Support

If you need help:

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples) 