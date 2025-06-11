# Model-Based Architecture

The Azure AI SDK is built around a **model-based architecture** that provides flexibility, scalability, and type safety. This guide explains the design principles and benefits of this approach.

## Architecture Overview

Instead of having clients that manage configuration, the SDK uses **models** as the primary configuration and capability containers. Each AI model encapsulates:

- 🔧 **Configuration**: Endpoint, API key, version, timeouts
- 🎯 **Capabilities**: Model-specific parameters and features  
- 🛡️ **Behavior**: Retry policies, error handling, defaults
- 🔒 **Isolation**: Independent configuration per model instance

## Core Components

### 1. Generic Client
```csharp
public interface IAzureAIClient : IDisposable
{
    Task<TResponse> GenerateImageAsync<TRequest, TResponse>(
        IImageGenerationModel model, 
        TRequest request, 
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class;
}
```

The client is **generic** and works with any model type. It doesn't hold configuration - that's the model's responsibility.

### 2. Model Interfaces
```csharp
public interface IImageModel
{
    string ModelName { get; }
    string Endpoint { get; }
    string ApiKey { get; }
    string ApiVersion { get; }
    TimeSpan Timeout { get; }
    int MaxRetryAttempts { get; }
    TimeSpan RetryDelay { get; }
}

public interface IImageGenerationModel : IImageModel
{
    string DefaultSize { get; }
    string DefaultOutputFormat { get; }
}
```

Models implement these interfaces to provide their capabilities and configuration.

### 3. Model Implementations
```csharp
public class StableImageUltraModel : IImageGenerationModel
{
    // Model-specific configuration
    public string ModelName { get; private set; }
    public string Endpoint { get; private set; }
    public string ApiKey { get; private set; }
    // ... other properties

    public static StableImageUltraModel Create(
        string endpoint, 
        string apiKey, 
        Action<StableImageUltraOptions>? configure = null)
    {
        // Factory method for easy creation
    }
}
```

## Key Benefits

### 🎯 Model Independence
Each model manages its own configuration and behavior:

```csharp
// Different models, different configurations
var fastModel = StableImageUltraModel.Create(fastEndpoint, fastApiKey, options =>
{
    options.Timeout = TimeSpan.FromMinutes(2);
    options.DefaultSize = "512x512";
});

var hqModel = StableImageUltraModel.Create(hqEndpoint, hqApiKey, options =>
{
    options.Timeout = TimeSpan.FromMinutes(10);
    options.DefaultSize = "1024x1024";
});
```

### 🔧 Flexible Configuration
Models can have completely different settings:

```csharp
var productionModel = StableImageUltraModel.Create(prodEndpoint, prodKey, options =>
{
    options.ApiVersion = "2024-05-01-preview";
    options.MaxRetryAttempts = 5;
    options.RetryDelay = TimeSpan.FromSeconds(2);
});

var developmentModel = StableImageUltraModel.Create(devEndpoint, devKey, options =>
{
    options.ApiVersion = "2024-05-01-preview";
    options.MaxRetryAttempts = 1;
    options.RetryDelay = TimeSpan.FromSeconds(1);
});
```

### 📈 Scalability
Easy to add new models without affecting existing ones:

```csharp
// Adding a new model type doesn't break existing code
public class NewImageModel : IImageGenerationModel
{
    // New model with its own configuration and capabilities
}

// Existing StableImageUltra code continues to work unchanged
```

### 🧪 Type Safety
Strong typing for model-specific requests and responses:

```csharp
// Each model has its own request/response types
var stableRequest = new ImageGenerationRequest { /* StableImageUltra-specific */ };
var stableResponse = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    stableModel, stableRequest);

// Future models will have their own types
var newRequest = new NewImageRequest { /* New model-specific */ };
var newResponse = await client.GenerateImageAsync<NewImageRequest, NewImageResponse>(
    newModel, newRequest);
```

## Usage Patterns

### Basic Usage
```csharp
// 1. Create model with configuration
var model = StableImageUltraModel.Create(endpoint, apiKey);

// 2. Create generic client
using var client = AzureAIClient.Create();

// 3. Create model-specific request
var request = new ImageGenerationRequest
{
    Model = model.ModelName,
    Prompt = "A beautiful landscape"
};

// 4. Generate using the model
var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    model, request);
```

### Multiple Models
```csharp
// Create multiple model instances for different purposes
var fastModel = StableImageUltraModel.Create(endpoint1, key1, opt => opt.DefaultSize = "512x512");
var qualityModel = StableImageUltraModel.Create(endpoint2, key2, opt => opt.DefaultSize = "1024x1024");

using var client = AzureAIClient.Create();

// Use appropriate model based on needs
var quickPreview = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    fastModel, quickRequest);

var finalImage = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
    qualityModel, qualityRequest);
```

### Dependency Injection
```csharp
// Register models in DI container
services.AddSingleton(provider =>
    StableImageUltraModel.Create(
        configuration["StableImageUltra:Endpoint"],
        configuration["StableImageUltra:ApiKey"]));

services.AddSingleton<IAzureAIClient, AzureAIClient>();

// Inject both client and model
public class ImageService
{
    private readonly IAzureAIClient _client;
    private readonly StableImageUltraModel _model;

    public ImageService(IAzureAIClient client, StableImageUltraModel model)
    {
        _client = client;
        _model = model;
    }

    public async Task<ImageGenerationResponse> GenerateImageAsync(string prompt)
    {
        var request = new ImageGenerationRequest
        {
            Model = _model.ModelName,
            Prompt = prompt
        };

        return await _client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
            _model, request);
    }
}
```

## HTTP Client Management

The SDK uses a shared `HttpClient` with per-request headers to ensure thread safety:

```csharp
public class ModelHttpClientService
{
    private static readonly HttpClient _httpClient = new();

    public async Task<TResponse> SendAsync<TRequest, TResponse>(
        IImageModel model, 
        TRequest request)
    {
        // Create request with model-specific configuration
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, model.Endpoint);
        
        // Set per-request headers (thread-safe)
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", model.ApiKey);
        httpRequest.Headers.Add("api-version", model.ApiVersion);
        
        // Set per-request timeout
        using var cts = new CancellationTokenSource(model.Timeout);
        
        // Send request
        var response = await _httpClient.SendAsync(httpRequest, cts.Token);
        
        // Handle response...
    }
}
```

## Comparison with Traditional Approach

### ❌ Traditional Client-Based Approach
```csharp
// Problems with client-based approach:
public class StableImageUltraClient
{
    private readonly string _endpoint;
    private readonly string _apiKey;
    private readonly TimeSpan _timeout;
    
    // Configuration tied to client instance
    // Hard to have multiple configurations
    // Difficult to extend to new models
}

// Usage requires multiple client instances
var fastClient = new StableImageUltraClient(fastEndpoint, fastKey, fastTimeout);
var slowClient = new StableImageUltraClient(slowEndpoint, slowKey, slowTimeout);
```

### ✅ Model-Based Approach
```csharp
// Benefits of model-based approach:
public class StableImageUltraModel : IImageGenerationModel
{
    // Configuration encapsulated in model
    // Easy to create multiple configurations
    // Generic client works with any model
}

// Single client, multiple model configurations
using var client = AzureAIClient.Create();
var fastModel = StableImageUltraModel.Create(fastEndpoint, fastKey, /* options */);
var slowModel = StableImageUltraModel.Create(slowEndpoint, slowKey, /* options */);
```

## Future Extensibility

The model-based architecture makes it easy to add new capabilities:

### New Model Types
```csharp
// Adding a text generation model
public interface ITextGenerationModel : IBaseModel
{
    int MaxTokens { get; }
    double Temperature { get; }
}

public class GPTModel : ITextGenerationModel
{
    // Text-specific configuration
}

// Client gets new method automatically
public interface IAzureAIClient
{
    // Existing method
    Task<TResponse> GenerateImageAsync<TRequest, TResponse>(
        IImageGenerationModel model, TRequest request);
    
    // New method for text generation
    Task<TResponse> GenerateTextAsync<TRequest, TResponse>(
        ITextGenerationModel model, TRequest request);
}
```

### New Capabilities
```csharp
// Adding streaming support
public interface IStreamingModel : IBaseModel
{
    bool SupportsStreaming { get; }
}

// Models can opt into streaming
public class StreamingStableImageModel : StableImageUltraModel, IStreamingModel
{
    public bool SupportsStreaming => true;
}
```

## Best Practices

### 1. Use Factory Methods
```csharp
// ✅ Good: Use factory methods for model creation
var model = StableImageUltraModel.Create(endpoint, apiKey, options =>
{
    options.DefaultSize = "1024x1024";
});

// ❌ Avoid: Direct constructor calls
var model = new StableImageUltraModel { /* manual setup */ };
```

### 2. Configure Models Once
```csharp
// ✅ Good: Create models once, reuse them
var model = StableImageUltraModel.Create(endpoint, apiKey, configure);
using var client = AzureAIClient.Create();

// Reuse model for multiple requests
var response1 = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(model, request1);
var response2 = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(model, request2);
```

### 3. Use Dependency Injection
```csharp
// ✅ Good: Register models in DI for production apps
services.AddSingleton<StableImageUltraModel>(provider =>
    StableImageUltraModel.Create(/* configuration */));

services.AddSingleton<IAzureAIClient, AzureAIClient>();
```

### 4. Model-Specific Namespaces
```csharp
// ✅ Good: Import model-specific namespaces
using AzureAISDK.Core;
using AzureAISDK.Inference.Image.StableImageUltra;

// All StableImageUltra types are available
var model = StableImageUltraModel.Create(/* ... */);
var request = new ImageGenerationRequest { /* ... */ };
var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(/* ... */);
```

## Summary

The model-based architecture provides:

- **🎯 Flexibility**: Each model has its own configuration
- **📈 Scalability**: Easy to add new models and capabilities  
- **🧪 Type Safety**: Strong typing for model-specific operations
- **🔒 Isolation**: Models don't interfere with each other
- **🔧 Extensibility**: Future-proof design for new features
- **🚀 Performance**: Efficient HTTP client management
- **🏗️ Simplicity**: Clean separation of concerns

This architecture enables the SDK to grow and adapt to new AI services while maintaining backward compatibility and providing an excellent developer experience. 