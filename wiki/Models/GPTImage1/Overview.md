# GPT-Image-1 Model

GPT-Image-1 is Azure OpenAI's advanced image generation and editing model that supports both creating new images from text prompts and editing existing images.

## Overview

GPT-Image-1 represents the latest advancement in AI-powered image generation and editing. It combines sophisticated text understanding with powerful visual generation capabilities, enabling both high-quality image creation from textual descriptions and precise image editing operations.

## Features

- **Dual Functionality**: Image generation and image editing in one model
- **High-Quality Output**: Superior image quality with multiple quality levels
- **Flexible Formats**: PNG and JPEG output with configurable compression
- **Multiple Aspect Ratios**: Square, portrait, and landscape orientations
- **Batch Processing**: Generate up to 10 images in a single request
- **Content Safety**: Built-in content filtering and safety measures
- **Mask-Based Editing**: Precise editing using transparency masks
- **Revised Prompts**: Access to how the model interpreted your prompts

## Configuration

### Basic Configuration

```csharp
var options = new GPTImage1Options
{
    Endpoint = "https://your-resource.openai.azure.com/",
    ApiKey = "your-api-key",
    DeploymentName = "your-gpt-image-1-deployment",
    ApiVersion = "2025-04-01-preview",
    DefaultSize = "1024x1024",
    DefaultQuality = "high"
};

var model = new GPTImage1Model(options);
```

### Configuration Properties

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| Endpoint | string | Azure OpenAI resource URL | Required |
| ApiKey | string | API key for authentication | Required |
| DeploymentName | string | GPT-Image-1 deployment name | Required |
| ApiVersion | string | API version to use | "2025-04-01-preview" |
| ModelName | string | Model identifier | "gpt-image-1" |
| DefaultSize | string | Default image size | "1024x1024" |
| DefaultQuality | string | Default image quality | "high" |
| DefaultOutputFormat | string | Default output format | "PNG" |
| DefaultCompression | int | Default compression level (0-100) | 100 |
| Timeout | TimeSpan | Request timeout | 5 minutes |
| MaxRetryAttempts | int | Maximum retry attempts | 3 |
| RetryDelay | TimeSpan | Delay between retries | 1 second |

## Usage

### Image Generation

#### Basic Generation

```csharp
var request = new ImageGenerationRequest
{
    Model = "gpt-image-1",
    Prompt = "A serene mountain landscape with a crystal clear lake",
    Size = "1024x1024",
    Quality = "high",
    N = 1
};

request.Validate();
// Make API call to generate image
```

#### Advanced Generation with All Options

```csharp
var request = new ImageGenerationRequest
{
    Model = "gpt-image-1",
    Prompt = "Professional headshot in modern office environment",
    Size = "1024x1536", // Portrait orientation
    Quality = "high",
    N = 3, // Generate 3 variations
    OutputFormat = "JPEG",
    OutputCompression = 85,
    User = "user-001" // For tracking usage
};

request.Validate();
// Make API call
```

### Image Editing

#### Basic Image Editing

```csharp
// Load image from file
var request = await ImageEditingRequest.FromFileAsync(
    "original_image.png",
    "Add a bright blue sky with fluffy white clouds"
);

request.Size = "1024x1024";
request.Quality = "high";
request.Model = "gpt-image-1";

request.Validate();
// Make API call to edit image
```

#### Editing with Mask

```csharp
// Create request with mask for precise editing
var request = await ImageEditingRequest.FromFileAsync(
    "original_image.png",
    "Replace the sky with a sunset scene",
    "sky_mask.png" // Mask defining the sky area
);

request.Size = "1024x1024";
request.Quality = "high";
request.OutputFormat = "PNG";
request.OutputCompression = 100;

request.Validate();
// Make API call
```

#### Editing from Memory

```csharp
var imageBytes = await File.ReadAllBytesAsync("image.png");
var maskBytes = await File.ReadAllBytesAsync("mask.png");

var request = ImageEditingRequest.FromBytes(
    imageBytes,
    "Change the background to a forest setting",
    "image.png",
    maskBytes,
    "mask.png"
);

request.Validate();
// Make API call
```

## Supported Parameters

### Image Sizes

| Size | Aspect Ratio | Description | Use Case |
|------|-------------|-------------|----------|
| 1024x1024 | 1:1 (Square) | Equal width and height | Social media, avatars |
| 1024x1536 | 2:3 (Portrait) | Taller than wide | Mobile screens, portraits |
| 1536x1024 | 3:2 (Landscape) | Wider than tall | Desktop wallpapers, banners |

### Quality Levels

| Quality | Description | Generation Speed | Use Case |
|---------|-------------|------------------|----------|
| low | Basic quality | Fastest | Quick previews, testing |
| medium | Balanced quality | Moderate | General purpose |
| high | Premium quality | Slower | Final production, print |

### Output Formats

| Format | Description | Compression | Transparency |
|--------|-------------|-------------|--------------|
| PNG | Lossless compression | Not applicable | Supported |
| JPEG | Lossy compression | 0-100% | Not supported |

### Batch Processing

| Parameter | Range | Description |
|-----------|-------|-------------|
| N (Count) | 1-10 | Number of images to generate in one request |

## Best Practices

### 1. Prompt Engineering

```csharp
// Good: Clear, descriptive prompts
"A professional headshot of a person in business attire, well-lit, sharp focus"

// Good: Specific style and mood
"Watercolor painting of a peaceful garden with blooming flowers, soft pastels"

// Avoid: Vague or overly complex prompts
"Make something cool and awesome with lots of stuff"
```

### 2. Performance Optimization

```csharp
// Use medium quality for faster batch processing
var batchRequest = new ImageGenerationRequest
{
    Prompt = "Abstract art with vibrant colors",
    Quality = "medium", // Faster than "high"
    N = 5, // Batch multiple images
    Size = "1024x1024" // Square generates fastest
};

// Use high quality only for final production
var productionRequest = new ImageGenerationRequest
{
    Prompt = "Professional product photography",
    Quality = "high", // Best quality for final use
    N = 1, // Single image for precision
    OutputFormat = "PNG", // Lossless for print
    OutputCompression = 100
};
```

### 3. Error Handling

```csharp
try
{
    request.Validate();
    // Make API call
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation Error: {ex.Message}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Network Error: {ex.Message}");
}

// Handle content filtering
if (response.HasError && response.Error.IsContentFiltered)
{
    Console.WriteLine("Content was filtered by safety system");
}
```

### 4. Image Editing Tips

```csharp
// Use masks for precise editing
var maskEditRequest = await ImageEditingRequest.FromFileAsync(
    "portrait.jpg",
    "Change the background to a library setting",
    "background_mask.png" // Only edit the background
);

// Keep original quality with PNG
editRequest.OutputFormat = "PNG";
editRequest.OutputCompression = 100;
```

## Response Handling

### Generation Response

```csharp
var response = new ImageGenerationResponse
{
    Created = 1698116662,
    Data = new List<ImageData>
    {
        new ImageData
        {
            Url = "https://example.com/generated-image.png",
            RevisedPrompt = "A serene mountain landscape..."
        }
    }
};

// Access generated images
foreach (var imageData in response.Data)
{
    // Download image
    var imageBytes = await imageData.DownloadImageAsync(httpClient);
    
    // Save to file
    await imageData.SaveImageAsync(httpClient, "generated_image.png");
    
    // Check how prompt was interpreted
    Console.WriteLine($"Revised Prompt: {imageData.RevisedPrompt}");
}

// Convert timestamp to DateTime
var createdTime = response.CreatedDateTime;
```

## Limitations

- **Image Sizes**: Limited to three predefined sizes
- **Format Support**: Only PNG and JPEG (no WEBP, GIF, etc.)
- **Batch Limit**: Maximum 10 images per request
- **Content Filtering**: Subject to Azure's content safety policies
- **URL Lifetime**: Generated image URLs expire after 24 hours
- **File Size**: Large images may have slower generation times

## Performance Considerations

- **Size Impact**: Square images (1024x1024) generate fastest
- **Quality vs Speed**: Use "medium" quality for faster batch processing
- **Compression**: Higher compression reduces file size but may affect quality
- **Batch Efficiency**: Generating multiple images in one request is more efficient
- **Caching**: Consider caching generated images for repeated use

## Use Cases

### 1. Content Creation
- Social media graphics
- Blog post illustrations
- Marketing materials
- Website backgrounds

### 2. Product Development
- Concept art and prototyping
- User interface mockups
- Product visualization
- Brand asset creation

### 3. Image Enhancement
- Background replacement
- Object removal/addition
- Style transfer
- Quality enhancement

### 4. Creative Applications
- Digital art creation
- Photo manipulation
- Artistic effects
- Style variations

## Security and Safety

- **Content Filtering**: Built-in safety systems prevent harmful content
- **API Key Security**: Store API keys securely, never in source code
- **User Tracking**: Use the `User` parameter for usage monitoring
- **Data Privacy**: Generated images are temporary (24-hour URLs)

## Related Topics

- [Configuration Guide](../../Getting-Started/Configuration.md)
- [API Reference](../../API-Reference/GPTImage1.md)
- [Best Practices](../../Best-Practices/Image-Generation.md)
- [Troubleshooting](../../Troubleshooting/GPTImage1-Issues.md)
- [Sample Projects](../../../samples/image-generation/GPTImage1/)

## Support

If you need help:

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Azure OpenAI Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/openai/)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples) 