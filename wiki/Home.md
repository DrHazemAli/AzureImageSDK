# Azure Image SDK Wiki

Welcome to the Azure Image SDK documentation! This SDK provides a comprehensive .NET solution for integrating with Azure AI services using a modern model-based architecture.

## 🏗️ Architecture Overview

The Azure Image SDK follows a **model-based architecture** where each AI model has its own configuration, parameters, and capabilities. This design provides:

- **🎯 Model Independence**: Each model can have different endpoints, API versions, and parameters
- **🔧 Flexible Configuration**: Per-model timeouts, retry policies, and settings
- **📈 Scalability**: Easy to add new models without affecting existing ones
- **🧪 Type Safety**: Strong typing for model-specific requests and responses

## 📚 Documentation Structure

### Getting Started
- [Installation Guide](Installation.md)
- [Quick Start](Quick-Start.md)
- [Configuration](Configuration.md)

### Model Types

#### Image Generation
- [StableImageUltra](models/image-generation/StableImageUltra.md)

### Advanced Topics
- [Model-Based Architecture](architecture/Model-Based-Architecture.md)
- [Error Handling](Error-Handling.md)
- [Logging](Logging.md)
- [Dependency Injection](Dependency-Injection.md)
- [Testing](Testing.md)

### Examples & Samples
- [Basic Usage Examples](examples/Basic-Usage.md)
- [Advanced Scenarios](examples/Advanced-Scenarios.md)
- [Sample Projects](examples/Sample-Projects.md)

### API Reference
- [Core Interfaces](api/Core-Interfaces.md)
- [Model Interfaces](api/Model-Interfaces.md)
- [Configuration Options](api/Configuration-Options.md)

### Development
- [Contributing](Contributing.md)
- [Building from Source](Building.md)
- [Release Notes](Release-Notes.md)

## 🚀 Quick Example

```csharp
using AzureImage.Core;
using AzureImage.Inference.Image.StableImageUltra;

// Create model with configuration
var model = StableImageUltraModel.Create(
    endpoint: "https://your-endpoint.eastus.models.ai.azure.com",
    apiKey: "your-api-key");

// Create Azure AI client
using var client = AzureAIClient.Create();

// Create model-specific request
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A beautiful sunset over mountains"
};

// Generate image
var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);

// Save the result
await response.SaveImageAsync("sunset.png");
```

## 🎯 Key Benefits

- **Model-Specific Configuration**: Each model manages its own endpoints, API versions, and parameters
- **Type Safety**: Strongly typed requests and responses for each model
- **Flexible Architecture**: Easy to extend with new models and capabilities
- **Production Ready**: Built-in retry logic, error handling, and logging
- **Dependency Injection**: Full support for .NET DI container

## 📖 Next Steps

1. [Install the SDK](Installation.md)
2. [Follow the Quick Start guide](Quick-Start.md)
3. [Explore model-specific documentation](models/)
4. [Check out sample projects](examples/Sample-Projects.md)

## 🆘 Support

- 📖 [Documentation](https://github.com/your-repo/AzureImage/wiki)
- 🐛 [Issue Tracker](https://github.com/your-repo/AzureImage/issues)
- 💬 [Discussions](https://github.com/your-repo/AzureImage/discussions) 