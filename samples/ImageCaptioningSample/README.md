# Azure Image Captioning Sample

This sample demonstrates how to use the AzureImage SDK to generate captions for images using Azure AI Vision Image Analysis 4.0 API.

## Features Demonstrated

- **Single Caption Generation**: Generate a single descriptive caption for an image
- **Dense Caption Generation**: Generate multiple captions for different regions of an image
- **Configurable Options**: Use gender-neutral captions, different languages, and other options
- **Multiple Input Sources**: Process images from URLs or local files
- **Error Handling**: Robust error handling and logging

## Prerequisites

1. **Azure AI Vision Resource**: You need an Azure AI Vision resource in a supported region
2. **API Key and Endpoint**: Obtain your API key and endpoint from the Azure portal
3. **.NET 6.0**: Ensure you have .NET 6.0 or later installed

## Supported Azure Regions

Azure AI Vision Image Analysis 4.0 Caption features are only available in certain regions:
- East US
- West US  
- West US 2
- France Central
- North Europe
- West Europe
- Sweden Central
- Switzerland North
- Australia East
- Southeast Asia
- East Asia
- Korea Central
- Japan East

Make sure your Azure AI Vision resource is created in one of these regions.

## Configuration

### Option 1: Configuration File
Update the `appsettings.json` file with your Azure AI Vision details:

```json
{
  "AzureVision": {
    "Endpoint": "https://your-resource-name.cognitiveservices.azure.com",
    "ApiKey": "your-api-key-here"
  }
}
```

### Option 2: Environment Variables
Set the following environment variables:

```bash
AzureVision__Endpoint=https://your-resource-name.cognitiveservices.azure.com
AzureVision__ApiKey=your-api-key-here
```

### Option 3: User Secrets (Recommended for Development)
```bash
dotnet user-secrets init
dotnet user-secrets set "AzureVision:Endpoint" "https://your-resource-name.cognitiveservices.azure.com"
dotnet user-secrets set "AzureVision:ApiKey" "your-api-key-here"
```

## Running the Sample

1. **Clone or download the AzureImage SDK**
2. **Navigate to the sample directory**:
   ```bash
   cd samples/ImageCaptioningSample
   ```

3. **Configure your Azure AI Vision credentials** (see Configuration section above)

4. **Run the sample**:
   ```bash
   dotnet run
   ```

## Sample Output

The application will demonstrate various image captioning scenarios:

```
=== Example 1: Generate Single Caption from URL ===
Image URL: https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png
Caption: A person pointing at a presentation screen
Confidence: 0.8934
Model Version: 2024-02-01
Image Dimensions: 1024x768

=== Example 2: Generate Caption with Options ===
Image URL: https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png
Gender-Neutral Caption: A person pointing at a presentation screen
Confidence: 0.8934

=== Example 3: Generate Dense Captions ===
Image URL: https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png
Found 3 dense captions:
  1. A person pointing at a presentation screen (Confidence: 0.8934)
     Region: X=150, Y=120, Width=400, Height=300
  2. A presentation screen with charts (Confidence: 0.7821)
     Region: X=450, Y=50, Width=500, Height=350
  3. A conference room setting (Confidence: 0.6789)
     Region: X=0, Y=0, Width=1024, Height=768
```

## Code Examples

### Basic Caption Generation

```csharp
using var azureImageClient = AzureImageClient.Create();

var visionOptions = new AzureVisionCaptioningOptions
{
    Endpoint = "https://your-resource.cognitiveservices.azure.com",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-01"
};

using var httpClient = new HttpClient();
var captioningModel = new AzureVisionCaptioningModel(httpClient, visionOptions);

var result = await azureImageClient.GenerateCaptionAsync(
    captioningModel, 
    "https://example.com/image.jpg");

Console.WriteLine($"Caption: {result.Caption.Text}");
Console.WriteLine($"Confidence: {result.Caption.Confidence:F4}");
```

### Caption Generation with Options

```csharp
var options = new ImageCaptionOptions
{
    Language = "en",
    GenderNeutralCaption = true
};

var result = await azureImageClient.GenerateCaptionAsync(
    captioningModel, 
    imageUrl,
    options);
```

### Dense Caption Generation

```csharp
var result = await azureImageClient.GenerateDenseCaptionsAsync(
    captioningModel, 
    imageUrl);

foreach (var caption in result.Captions)
{
    Console.WriteLine($"Caption: {caption.Text}");
    Console.WriteLine($"Region: {caption.BoundingBox}");
    Console.WriteLine($"Confidence: {caption.Confidence:F4}");
}
```

### Processing Local Files

```csharp
using var imageStream = File.OpenRead("path/to/image.jpg");

var result = await azureImageClient.GenerateCaptionAsync(
    captioningModel, 
    imageStream);
```

## Error Handling

The sample includes comprehensive error handling:

```csharp
try
{
    var result = await azureImageClient.GenerateCaptionAsync(model, imageUrl);
    // Process result
}
catch (AzureVisionException ex)
{
    Console.WriteLine($"Azure Vision API Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Response: {ex.ResponseContent}");
}
catch (Exception ex)
{
    Console.WriteLine($"General Error: {ex.Message}");
}
```

## Supported Image Formats

- JPEG
- PNG
- GIF
- BMP
- WEBP
- ICO
- TIFF
- MPO

## Image Requirements

- File size: Less than 20 MB
- Dimensions: Between 50x50 and 16,000x16,000 pixels
- Publicly accessible URLs (for URL-based processing)

## Troubleshooting

### Common Issues

1. **"Caption features not supported in this region"**
   - Ensure your Azure AI Vision resource is in a supported region
   - Check the list of supported regions above

2. **"Access denied due to invalid subscription key"**
   - Verify your API key is correct
   - Ensure the API key corresponds to the correct resource

3. **"The provided image URL is not accessible"**
   - Ensure the image URL is publicly accessible
   - Check that the image format is supported

4. **"Image format is not valid"**
   - Verify the image is in a supported format
   - Check that the image file is not corrupted

### Getting Help

- Check the Azure AI Vision documentation
- Review the Azure portal for resource status
- Verify your subscription has the necessary permissions
- Check the Azure service status page for any outages

## Next Steps

- Explore other features of the AzureImage SDK
- Integrate image captioning into your applications
- Experiment with different image types and options
- Review the Azure AI Vision documentation for advanced features 