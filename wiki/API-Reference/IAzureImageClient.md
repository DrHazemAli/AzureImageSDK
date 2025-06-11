# IAzureImageClient Interface Reference

The `IAzureImageClient` interface is the main entry point for interacting with Azure AI Foundry's image generation services.

## Interface Definition

```csharp
public interface IAzureImageClient
{
    Task<ImageGenerationResult> GenerateImageAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<ImageGenerationResult>> GenerateImagesAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default);
    Task<ImageGenerationResult> GenerateImageWithModelAsync<T>(ImageGenerationRequest request, CancellationToken cancellationToken = default) where T : IImageGenerationModel;
}
```

## Methods

### GenerateImageAsync

Generates a single image based on the provided request.

```csharp
Task<ImageGenerationResult> GenerateImageAsync(
    ImageGenerationRequest request,
    CancellationToken cancellationToken = default
);
```

#### Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| request | ImageGenerationRequest | The image generation request parameters |
| cancellationToken | CancellationToken | Optional cancellation token |

#### Returns

`Task<ImageGenerationResult>` - The generated image result

#### Example

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra",
    Size = new ImageSize(1024, 1024)
};

var result = await client.GenerateImageAsync(request);
Console.WriteLine($"Generated image URL: {result.ImageUrl}");
```

### GenerateImagesAsync

Generates multiple images based on the provided request.

```csharp
Task<IEnumerable<ImageGenerationResult>> GenerateImagesAsync(
    ImageGenerationRequest request,
    CancellationToken cancellationToken = default
);
```

#### Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| request | ImageGenerationRequest | The image generation request parameters |
| cancellationToken | CancellationToken | Optional cancellation token |

#### Returns

`Task<IEnumerable<ImageGenerationResult>>` - Collection of generated image results

#### Example

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Model = "stable-image-ultra",
    Size = new ImageSize(1024, 1024),
    NumberOfImages = 4
};

var results = await client.GenerateImagesAsync(request);
foreach (var result in results)
{
    Console.WriteLine($"Generated image URL: {result.ImageUrl}");
}
```

### GenerateImageWithModelAsync

Generates an image using a specific model implementation.

```csharp
Task<ImageGenerationResult> GenerateImageWithModelAsync<T>(
    ImageGenerationRequest request,
    CancellationToken cancellationToken = default
) where T : IImageGenerationModel;
```

#### Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| request | ImageGenerationRequest | The image generation request parameters |
| cancellationToken | CancellationToken | Optional cancellation token |

#### Type Parameters

| Parameter | Description |
|-----------|-------------|
| T | The type of model to use, must implement IImageGenerationModel |

#### Returns

`Task<ImageGenerationResult>` - The generated image result

#### Example

```csharp
var request = new ImageGenerationRequest
{
    Prompt = "A beautiful sunset over mountains",
    Size = new ImageSize(1024, 1024)
};

var result = await client.GenerateImageWithModelAsync<StableImageUltraModel>(request);
Console.WriteLine($"Generated image URL: {result.ImageUrl}");
```

## Request Parameters

### ImageGenerationRequest

| Property | Type | Description | Required |
|----------|------|-------------|----------|
| Prompt | string | The text prompt for image generation | Yes |
| Model | string | The model to use for generation | Yes |
| Size | ImageSize | The size of the generated image | No |
| Style | string | The style of the generated image | No |
| Quality | ImageQuality | The quality of the generated image | No |
| NegativePrompt | string | Text to avoid in the generated image | No |
| Seed | int? | Random seed for reproducible results | No |
| NumberOfImages | int | Number of images to generate | No |

## Response Types

### ImageGenerationResult

| Property | Type | Description |
|----------|------|-------------|
| ImageUrl | string | URL of the generated image |
| Prompt | string | The prompt used for generation |
| Model | string | The model used for generation |
| CreatedAt | DateTime | Timestamp of image generation |
| RequestId | string | Unique identifier for the request |

## Error Handling

The client throws the following exceptions:

- `AzureImageException` - Base exception for all SDK errors
- `AuthenticationException` - Authentication-related errors
- `ValidationException` - Request validation errors
- `RateLimitException` - Rate limiting errors
- `TimeoutException` - Request timeout errors

## Best Practices

1. **Error Handling**
   ```csharp
   try
   {
       var result = await client.GenerateImageAsync(request);
   }
   catch (AzureImageException ex)
   {
       // Handle specific error types
       switch (ex)
       {
           case AuthenticationException:
               // Handle authentication errors
               break;
           case ValidationException:
               // Handle validation errors
               break;
           case RateLimitException:
               // Handle rate limiting
               break;
           default:
               // Handle other errors
               break;
       }
   }
   ```

2. **Cancellation**
   ```csharp
   using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
   try
   {
       var result = await client.GenerateImageAsync(request, cts.Token);
   }
   catch (OperationCanceledException)
   {
       // Handle cancellation
   }
   ```

3. **Model Selection**
   ```csharp
   // Use specific model for better control
   var result = await client.GenerateImageWithModelAsync<StableImageUltraModel>(request);
   ```

## Related Topics

- [Configuration Guide](../Getting-Started/Configuration.md)
- [Quick Start Guide](../Getting-Started/Quick-Start.md)
- [Best Practices](../Best-Practices/Performance.md)
- [Troubleshooting](../Troubleshooting/Common-Issues.md)

## Support

If you need help:

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples) 