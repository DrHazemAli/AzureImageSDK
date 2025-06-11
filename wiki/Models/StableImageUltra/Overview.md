# StableImageUltra Model

StableImageUltra is a high-performance image generation model that provides superior quality and control over image generation.

## Overview

StableImageUltra is designed for high-quality image generation with advanced features and fine-grained control over the generation process. It's ideal for applications requiring professional-grade image generation.

## Features

- High-resolution image generation (up to 2048x2048)
- Advanced prompt understanding
- Multiple style presets
- Fine-grained quality control
- Negative prompt support
- Seed control for reproducible results

## Configuration

### Basic Configuration

```csharp
var options = new StableImageUltraOptions
{
    Endpoint = "your-ultra-endpoint",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-15-preview",
    DefaultSize = new ImageSize(1024, 1024),
    DefaultQuality = ImageQuality.High
};
```

### Configuration Properties

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| Endpoint | string | Model endpoint URL | Required |
| ApiKey | string | API key for authentication | Required |
| ApiVersion | string | API version to use | "2024-02-15-preview" |
| DefaultSize | ImageSize | Default image size | 1024x1024 |
| DefaultQuality | ImageQuality | Default image quality | High |

## Usage

### Basic Image Generation

```csharp
var client = new AzureImageClient(options);
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra",
    Size = new ImageSize(1024, 1024)
};

var result = await client.GenerateImageAsync(request);
```

### Advanced Image Generation

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains, photorealistic style",
    Model = "stable-image-ultra",
    Size = new ImageSize(2048, 2048),
    Style = "photorealistic",
    Quality = ImageQuality.High,
    NegativePrompt = "blurry, low quality, distorted",
    Seed = 12345
};

var result = await client.GenerateImageAsync(request);
```

### Using Model-Specific Client

```csharp
var ultraClient = new StableImageUltraClient(options);
var result = await ultraClient.GenerateImageAsync(new StableImageUltraRequest
{
    Prompt = "A beautiful sunset over mountains",
    Size = new ImageSize(1024, 1024),
    Style = "photorealistic"
});
```

## Supported Parameters

### Image Size

| Size | Description | Use Case |
|------|-------------|----------|
| 512x512 | Small | Thumbnails, icons |
| 1024x1024 | Medium | Standard images |
| 2048x2048 | Large | High-resolution images |

### Image Quality

| Quality | Description | Use Case |
|---------|-------------|----------|
| Standard | Balanced quality | General purpose |
| High | Enhanced quality | Professional use |
| Ultra | Maximum quality | Premium content |

### Styles

| Style | Description |
|-------|-------------|
| photorealistic | Realistic photography style |
| artistic | Artistic interpretation |
| cinematic | Movie-like quality |
| digital-art | Digital art style |
| anime | Anime/manga style |

## Best Practices

1. **Prompt Engineering**
   - Be specific and detailed in prompts
   - Use style keywords
   - Include negative prompts for unwanted elements

2. **Quality vs. Performance**
   - Use appropriate quality settings
   - Consider generation time
   - Balance resolution and quality

3. **Error Handling**
   ```csharp
   try
   {
       var result = await client.GenerateImageAsync(request);
   }
   catch (StableImageUltraException ex)
   {
       // Handle model-specific errors
       Console.WriteLine($"Error: {ex.Message}");
       Console.WriteLine($"Error Code: {ex.ErrorCode}");
   }
   ```

## Limitations

- Maximum image size: 2048x2048
- Maximum prompt length: 1000 characters
- Rate limits apply
- Some styles may require specific prompts

## Performance Considerations

- Higher resolutions require more processing time
- Ultra quality setting increases generation time
- Multiple image generation is parallelized
- Consider caching generated images

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