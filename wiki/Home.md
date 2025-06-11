# AzureImage SDK Documentation

Welcome to the AzureImage SDK documentation! This SDK provides a simple and efficient way to interact with Azure AI Foundry's image generation and manipulation services.

## Overview

AzureImage SDK is a .NET library that enables developers to easily integrate Azure AI Foundry's image capabilities into their applications. The SDK supports .NET 6.0 and above, providing a modern and efficient way to work with AI-powered image generation and manipulation.

## Key Features

- 🚀 Easy integration with Azure AI Foundry
- 🎨 Support for multiple image generation models
- ⚡ High-performance async operations
- 🔒 Built-in security and authentication
- 📦 Available as NuGet and GitHub packages
- 🧪 Comprehensive test coverage
- 📚 Extensive documentation and examples

## Quick Links

- [Getting Started](Getting-Started/Installation.md)
- [Quick Start Guide](Getting-Started/Quick-Start.md)
- [API Reference](API-Reference/IAzureImageClient.md)
- [Examples](Examples/Basic-Usage.md)
- [Release Notes](Release-Notes/Changelog.md)

## Supported Models

- [StableImageUltra](Models/StableImageUltra/Overview.md)
- [StableImageCore](Models/StableImageCore/Overview.md)

## Installation

The SDK is available as both a NuGet package and a GitHub package. See our [Installation Guide](Getting-Started/Installation.md) for detailed instructions.

```bash
# NuGet Package
dotnet add package AzureImage

# GitHub Package
dotnet add package AzureImage --source https://nuget.pkg.github.com/DrHazemAli/index.json
```

## Quick Example

```csharp
using AzureImage;
using AzureImage.Inference.Models;

// Initialize the client
var client = new AzureImageClient(new AzureImageOptions
{
    Endpoint = "your-endpoint",
    ApiKey = "your-api-key"
});

// Generate an image
var result = await client.GenerateImageAsync(new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra"
});
```

## Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for more information.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples)

## References

- [Azure AI Foundry Documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Azure SDK Design Guidelines](https://azure.github.io/azure-sdk/dotnet_introduction.html) 