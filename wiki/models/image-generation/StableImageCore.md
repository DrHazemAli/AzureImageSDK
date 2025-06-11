# StableImageCore

The StableImageCore module provides integration with Azure AI's Stable Image Core model for image generation.

## Installation

StableImageCore is included in the AzureAI SDK package:

```bash
dotnet add package AzureImage
```

## Configuration

### Basic Configuration

```csharp
var model = StableImageCoreModel.Create(
    endpoint: "https://your-endpoint.com",
    apiKey: "your-api-key"
);
```

### Advanced Configuration

```csharp
var model = StableImageCoreModel.Create(
    endpoint: "https://your-endpoint.com",
    apiKey: "your-api-key",
    configure: options => {
        options.ApiVersion = "2024-05-01-preview";
        options.Timeout = TimeSpan.FromMinutes(10);
        options.MaxRetryAttempts = 5;
        options.RetryDelay = TimeSpan.FromSeconds(2);
        options.DefaultSize = "1920x1080";
        options.DefaultOutputFormat = "jpeg";
    }
);
```

## Usage

### Basic Image Generation

```csharp
// Create a request
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful landscape with mountains and a lake",
    Size = "1024x1024",
    OutputFormat = "png"
};

// Validate the request
request.Validate();

// Use with HTTP client to make API call
// var response = await httpClient.SendAsync(request, model);
```

### Using Negative Prompts

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A serene garden scene",
    NegativePrompt = "people, animals, cars",
    Size = "1024x1024"
};
```

### Setting a Seed for Reproducible Generation

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "Abstract art with vibrant colors",
    Seed = 12345,
    Size = "512x512"
};
```

### Processing Response

```csharp
// Assuming you have a response from the API
byte[] imageBytes = response.GetImageBytes();

// Save to file
await response.SaveImageAsync("output/generated-image.png");

// Access metadata
if (response.Metadata != null)
{
    Console.WriteLine($"Width: {response.Metadata.Width}");
    Console.WriteLine($"Height: {response.Metadata.Height}");
    Console.WriteLine($"Format: {response.Metadata.Format}");
    Console.WriteLine($"Seed: {response.Metadata.Seed}");
    Console.WriteLine($"Model: {response.Metadata.Model}");
}
```

## Error Handling

```csharp
try
{
    var model = StableImageCoreModel.Create(endpoint, apiKey);
    var request = new ImageGenerationRequest
    {
        Prompt = "A beautiful landscape",
        Size = "invalid-size" // This will cause validation to fail
    };
    
    request.Validate(); // Will throw ArgumentException
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Best Practices

1. **Validate Configuration**: Always validate configuration options before using the model
2. **Validate Requests**: Validate requests before sending to the API
3. **Handle Errors**: Implement proper error handling for API failures
4. **Set Timeouts**: Configure appropriate timeouts for your use case
5. **Use Retry Logic**: Enable retry attempts for transient failures

## Related Resources

- [Sample Application](../samples/image-generation/StableImageCore)
- [API Reference](API-Reference.md)
- [Configuration Reference](Configuration.md)
- [Troubleshooting Guide](Troubleshooting.md)
