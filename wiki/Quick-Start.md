# Quick Start Guide

Get up and running with the Azure Image SDK in minutes! This guide will walk you through your first image generation using StableImageUltra.

## Prerequisites

- .NET 6.0 or later
- Azure AI service with StableImageUltra deployment
- Valid API endpoint and key

## Step 1: Install the SDK

### Using Package Manager Console
```powershell
Install-Package AzureImage
```

### Using .NET CLI
```bash
dotnet add package AzureImage
```

## Step 2: Create Your First Application

Create a new console application:

```bash
dotnet new console -n MyFirstAzureAI
cd MyFirstAzureAI
dotnet add package AzureImage
```

## Step 3: Generate Your First Image

Replace the contents of `Program.cs`:

```csharp
using AzureImage.Core;
using AzureImage.Inference.Image.StableImageUltra;

// Your Azure AI service details
string endpoint = "https://your-endpoint.eastus.models.ai.azure.com";
string apiKey = "your-api-key-here";

try
{
    // Create the model with your configuration
    var model = StableImageUltraModel.Create(endpoint, apiKey);

    // Create the Azure AI client
    using var client = AzureAIClient.Create();

    // Create your image generation request
    var request = new ImageGenerationRequest
    {
        Model = model.ModelName,
        Prompt = "A majestic mountain landscape at sunset with vibrant colors"
    };

    Console.WriteLine("🎨 Generating your first AI image...");

    // Generate the image
    var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
        model, request);

    // Save the image
    await response.SaveImageAsync("my-first-ai-image.png");

    Console.WriteLine("✅ Success! Your image has been saved as 'my-first-ai-image.png'");
    Console.WriteLine($"📊 Image data size: {response.Image.Length} characters");

    // Check if metadata is available
    if (response.Metadata != null)
    {
        Console.WriteLine($"📐 Image dimensions: {response.Metadata.Width}x{response.Metadata.Height}");
        Console.WriteLine($"🎯 Model used: {response.Metadata.Model}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();
```

## Step 4: Configure Your Credentials

Replace the placeholder values with your actual Azure AI service details:

```csharp
string endpoint = "https://your-stable-image-ultra-endpoint.eastus.models.ai.azure.com";
string apiKey = "your-actual-api-key-here";
```

### Alternative: Use Configuration File

Create `appsettings.json`:

```json
{
  "StableImageUltra": {
    "Endpoint": "https://your-endpoint.eastus.models.ai.azure.com",
    "ApiKey": "your-api-key-here"
  }
}
```

Install configuration packages:
```bash
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
```

Update your `Program.cs` to use configuration:

```csharp
using AzureImage.Core;
using AzureImage.Inference.Image.StableImageUltra;
using Microsoft.Extensions.Configuration;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Get credentials from configuration
string endpoint = configuration["StableImageUltra:Endpoint"]!;
string apiKey = configuration["StableImageUltra:ApiKey"]!;

// Rest of your code...
```

## Step 5: Run Your Application

```bash
dotnet run
```

You should see output like:
```
🎨 Generating your first AI image...
✅ Success! Your image has been saved as 'my-first-ai-image.png'
📊 Image data size: 87432 characters
📐 Image dimensions: 1024x1024
🎯 Model used: Stable-Image-Ultra

Press any key to exit...
```

## Next Steps

### 🎯 Advanced Features

Try advanced generation with more parameters:

```csharp
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A cyberpunk cityscape with neon lights reflecting on wet streets",
    NegativePrompt = "blurry, low quality, distorted",
    Size = "1024x768",
    OutputFormat = "png",
    Seed = 42
};
```

### ⚙️ Custom Model Configuration

Configure your model for different use cases:

```csharp
// High-quality model (slower, better results)
var hqModel = StableImageUltraModel.Create(endpoint, apiKey, options =>
{
    options.ModelName = "Stable-Image-Ultra-HQ";
    options.DefaultSize = "1024x1024";
    options.DefaultOutputFormat = "png";
    options.Timeout = TimeSpan.FromMinutes(10);
});

// Fast model (quicker, smaller images)
var fastModel = StableImageUltraModel.Create(endpoint, apiKey, options =>
{
    options.ModelName = "Stable-Image-Ultra-Fast";
    options.DefaultSize = "512x512";
    options.DefaultOutputFormat = "jpg";
    options.Timeout = TimeSpan.FromMinutes(2);
});
```

### 🔄 Batch Generation

Generate multiple images at once:

```csharp
var prompts = new[]
{
    "A peaceful forest glade",
    "A bustling city street",
    "A serene ocean sunset"
};

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

for (int i = 0; i < responses.Length; i++)
{
    await responses[i].SaveImageAsync($"batch-image-{i + 1}.png");
}
```

### 🏗️ Dependency Injection

For ASP.NET Core or hosted applications:

```csharp
// In Program.cs or Startup.cs
services.AddAzureAI(configuration.GetSection("AzureAI"));

// In your controller or service
public class ImageController : ControllerBase
{
    private readonly IAzureAIClient _azureAIClient;

    public ImageController(IAzureAIClient azureAIClient)
    {
        _azureAIClient = azureAIClient;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateImage([FromBody] string prompt)
    {
        var model = StableImageUltraModel.Create(endpoint, apiKey);
        var request = new ImageGenerationRequest
        {
            Model = model.ModelName,
            Prompt = prompt
        };

        var response = await _azureAIClient.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
            model, request);

        return File(response.GetImageBytes(), "image/png");
    }
}
```

## Common Issues

### ❌ Configuration Errors
Make sure your endpoint URL and API key are correct:
- Endpoint should include the full URL with protocol (`https://`)
- API key should be the actual key from your Azure AI service

### ❌ Network Errors
- Check your internet connection
- Verify firewall settings
- Ensure the endpoint is accessible

### ❌ API Errors
- Check if your Azure AI service is running
- Verify you have sufficient quota
- Review the error message for specific details

## Getting Help

- 📖 [Full Documentation](Home.md)
- 🎯 [StableImageUltra Guide](models/image-generation/StableImageUltra.md)
- 🔧 [Configuration Guide](Configuration.md)
- 🚀 [Sample Projects](examples/Sample-Projects.md)
- 🐛 [Issue Tracker](https://github.com/your-repo/AzureImage/issues)

## What's Next?

1. **[Explore More Models](models/)** - Learn about additional AI capabilities
2. **[Try the Samples](../samples/)** - Run comprehensive example applications
3. **[Read the Architecture Guide](architecture/Model-Based-Architecture.md)** - Understand the SDK design
4. **[Check Best Practices](Error-Handling.md)** - Learn production-ready patterns

Congratulations! 🎉 You've successfully generated your first AI image with the Azure Image SDK! 