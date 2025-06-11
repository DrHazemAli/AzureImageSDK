# StableImageUltra Sample Application

This sample application demonstrates how to use the Azure AI SDK with StableImageUltra for image generation. It showcases various features and capabilities of the model through an interactive console application.

## Features Demonstrated

- 🖼️ **Basic Image Generation** - Simple prompt-to-image generation
- 🎯 **Advanced Generation** - Custom parameters including negative prompts, sizes, and formats
- 🌱 **Reproducible Generation** - Using seeds for consistent results
- 🎨 **Creative Prompts Gallery** - Pre-built creative prompts across different styles
- 📐 **Different Image Sizes** - Various aspect ratios and resolutions
- 🔄 **Batch Generation** - Generate multiple images concurrently
- ⚙️ **Configuration Examples** - Different model configurations for various use cases

## Prerequisites

- .NET 8.0 or later
- Azure AI service with StableImageUltra deployment
- Valid API key and endpoint URL

## Setup

### 1. Configure Your API Credentials

Create or update `appsettings.json` with your Azure AI service details:

```json
{
  "StableImageUltra": {
    "Endpoint": "https://your-stable-image-ultra-endpoint.eastus.models.ai.azure.com",
    "ApiKey": "your-api-key-here"
  }
}
```

### 2. Alternative: Use Environment Variables

You can also set credentials via environment variables:

```bash
export StableImageUltra__Endpoint="https://your-endpoint.eastus.models.ai.azure.com"
export StableImageUltra__ApiKey="your-api-key-here"
```

### 3. Build and Run

```bash
# Navigate to the sample directory
cd samples/image-generation/StableImageUltra

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## Sample Menu Options

### 1. 🖼️ Basic Image Generation
- Simple prompt input
- Uses default model settings
- Generates PNG images at 1024x1024 resolution

### 2. 🎯 Advanced Image Generation
- Custom prompt and negative prompt
- Choose image size and output format
- Interactive parameter configuration

### 3. 🌱 Reproducible Generation (with Seed)
- Demonstrates seed-based generation
- Generates 3 identical images using the same seed
- Perfect for understanding deterministic generation

### 4. 🎨 Creative Prompts Gallery
Pre-built prompts for various styles:
- **Fantasy**: Dragons, castles, magical elements
- **Cyberpunk**: Neon cities, futuristic technology
- **Nature**: Forests, landscapes, natural beauty
- **Space**: Cosmic scenes, astronauts, nebulae
- **Abstract**: Geometric patterns, artistic designs

### 5. 📐 Different Image Sizes
Demonstrates various supported resolutions:
- `512x512` - Square, fast generation
- `768x768` - Square, better quality
- `1024x1024` - Square, high quality
- `1024x768` - Landscape orientation
- `768x1024` - Portrait orientation

### 6. 🔄 Batch Generation
- Generates multiple images concurrently
- Demonstrates parallel processing
- Shows efficient API usage patterns

### 7. ⚙️ Configuration Examples
Three different model configurations:
- **High Performance**: Fast generation, smaller images
- **High Quality**: Slower generation, larger images
- **Custom Retry**: Enhanced resilience with custom retry policies

## Generated Files

All generated images are saved in the application directory with descriptive filenames:
- `basic_yyyyMMdd_HHmmss.png` - Basic generation
- `advanced_yyyyMMdd_HHmmss.{format}` - Advanced generation
- `reproducible_{seed}_variation_{n}.png` - Reproducible generation
- `gallery_{style}_yyyyMMdd_HHmmss.png` - Gallery prompts
- `size_{dimensions}_yyyyMMdd_HHmmss.png` - Size demonstrations
- `batch_{n}_yyyyMMdd_HHmmss.png` - Batch generation
- `config_{n}_yyyyMMdd_HHmmss.{format}` - Configuration examples

## Code Examples

### Basic Usage
```csharp
// Create model
var model = StableImageUltraModel.Create(endpoint, apiKey);

// Create client
using var client = AzureAIClient.Create();

// Generate image
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A peaceful zen garden"
};

var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

// Save image
await response.SaveImageAsync("zen_garden.png");
```

### Advanced Configuration
```csharp
var model = StableImageUltraModel.Create(endpoint, apiKey, options =>
{
    options.ModelName = "Stable-Image-Ultra-HQ";
    options.DefaultSize = "1024x1024";
    options.DefaultOutputFormat = "png";
    options.Timeout = TimeSpan.FromMinutes(10);
    options.MaxRetryAttempts = 3;
});
```

### Batch Processing
```csharp
var prompts = new[] { "landscape", "portrait", "abstract" };

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

## Best Practices Demonstrated

### Prompt Engineering
- **Be Descriptive**: Use detailed, specific prompts
- **Include Style**: Mention artistic styles or moods
- **Use Negative Prompts**: Exclude unwanted elements

Good prompt example:
```
"A majestic golden retriever sitting in a blooming cherry blossom garden during spring, soft natural lighting, photorealistic style, high detail"
```

### Error Handling
The sample includes comprehensive error handling:
- Configuration validation
- API error handling
- Network error recovery
- User input validation

### Performance Optimization
- **Batch Processing**: Generate multiple images concurrently
- **Appropriate Sizing**: Choose sizes based on use case
- **Seed Usage**: Use seeds for consistent variations

## Troubleshooting

### Common Issues

1. **Configuration Errors**
   - Ensure endpoint URL is correct
   - Verify API key is valid
   - Check network connectivity

2. **Generation Failures**
   - Check prompt content for restrictions
   - Verify requested size is supported
   - Monitor rate limits

3. **File Save Errors**
   - Ensure write permissions in output directory
   - Check available disk space

### Debug Information

Enable debug logging in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "StableImageUltra.Sample": "Debug"
    }
  }
}
```

## Related Documentation

- [StableImageUltra Model Documentation](../../../wiki/models/image-generation/StableImageUltra.md)
- [Azure AI SDK Installation Guide](../../../wiki/Installation.md)
- [Configuration Guide](../../../wiki/Configuration.md)
- [Error Handling Guide](../../../wiki/Error-Handling.md)

## Sample Output

When you run the application, you'll see:

```
🎨 StableImageUltra Sample Application
====================================

Choose an option:
1. 🖼️  Basic Image Generation
2. 🎯 Advanced Image Generation
3. 🌱 Reproducible Generation (with Seed)
4. 🎨 Creative Prompts Gallery
5. 📐 Different Image Sizes
6. 🔄 Batch Generation
7. ⚙️  Configuration Examples
0. 🚪 Exit

Enter your choice (0-7):
```

Follow the prompts to explore different features and capabilities of the StableImageUltra model! 