# ImageMetadataExtractor

The **ImageMetadataExtractor** utility class provides functionality for extracting comprehensive metadata information from image files and streams. This utility is essential for analyzing image properties, validating image characteristics, and gathering detailed information about image files.

## Overview

The `ImageMetadataExtractor` is a static utility class that helps developers:
- Extract detailed metadata from image streams and files
- Validate image metadata integrity
- Analyze image properties and characteristics
- Get comprehensive information about image dimensions, format, and file properties

## Features

### 📊 Metadata Extraction
- **Dimensions** - Width and height in pixels
- **File Format** - Image format detection
- **File Size** - Size in bytes
- **Timestamps** - Creation and modification dates
- **Properties** - Additional image properties and EXIF data
- **Validation** - Metadata integrity checking

### 🔍 Input Support
- **Stream Processing** - Extract from any readable stream
- **File Processing** - Direct file path support
- **Format Detection** - Automatic format recognition
- **Error Handling** - Graceful handling of invalid images

## API Reference

### Methods

#### ExtractMetadataAsync(Stream imageStream)
Extracts metadata from an image stream asynchronously.

**Parameters:**
- `imageStream` (Stream): The image stream to extract metadata from

**Returns:**
- `Task<ImageMetadata>`: The extracted image metadata

**Exceptions:**
- `ArgumentNullException`: Thrown when the image stream is null
- `ArgumentException`: Thrown when the image stream is not readable

**Example:**
```csharp
using AzureImage.Utilities;

// Extract metadata from stream
using var fileStream = File.OpenRead("image.jpg");
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(fileStream);

Console.WriteLine($"Dimensions: {metadata.Width}x{metadata.Height}");
Console.WriteLine($"Format: {metadata.Format}");
Console.WriteLine($"Size: {metadata.Size} bytes");
Console.WriteLine($"Created: {metadata.CreationDate}");
```

#### ExtractMetadataAsync(string filePath)
Extracts metadata from an image file asynchronously.

**Parameters:**
- `filePath` (string): The path to the image file

**Returns:**
- `Task<ImageMetadata>`: The extracted image metadata

**Exceptions:**
- `ArgumentNullException`: Thrown when the file path is null or empty
- `FileNotFoundException`: Thrown when the file does not exist

**Example:**
```csharp
// Extract metadata from file
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync("path/to/image.png");

Console.WriteLine($"Image: {metadata.Width}x{metadata.Height}");
Console.WriteLine($"Format: {metadata.Format}");
Console.WriteLine($"File size: {metadata.Size:N0} bytes");
```

#### HasValidMetadataAsync(Stream imageStream)
Checks if an image stream has valid metadata.

**Parameters:**
- `imageStream` (Stream): The image stream to check

**Returns:**
- `Task<bool>`: True if the image has valid metadata, false otherwise

**Exceptions:**
- `ArgumentNullException`: Thrown when the image stream is null

**Example:**
```csharp
// Validate image metadata
using var stream = File.OpenRead("questionable-image.jpg");
bool isValid = await ImageMetadataExtractor.HasValidMetadataAsync(stream);

if (isValid)
{
    Console.WriteLine("Image has valid metadata");
    var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(stream);
    // Process metadata...
}
else
{
    Console.WriteLine("Image metadata is invalid or corrupted");
}
```

## ImageMetadata Class

The `ImageMetadata` class represents the extracted metadata from an image:

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Width` | `int` | Width of the image in pixels |
| `Height` | `int` | Height of the image in pixels |
| `Format` | `string` | Format of the image (e.g., "JPEG", "PNG") |
| `Size` | `long` | Size of the image in bytes |
| `CreationDate` | `DateTime` | Creation date of the image |
| `LastModified` | `DateTime` | Last modified date of the image |
| `Properties` | `Dictionary<string, string>` | Additional properties and EXIF data |

### Example Usage
```csharp
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync("image.jpg");

// Access basic properties
Console.WriteLine($"Dimensions: {metadata.Width} x {metadata.Height}");
Console.WriteLine($"Format: {metadata.Format}");
Console.WriteLine($"File Size: {metadata.Size:N0} bytes");

// Access timestamps
Console.WriteLine($"Created: {metadata.CreationDate:yyyy-MM-dd HH:mm:ss}");
Console.WriteLine($"Modified: {metadata.LastModified:yyyy-MM-dd HH:mm:ss}");

// Access additional properties
if (metadata.Properties.Any())
{
    Console.WriteLine("Additional Properties:");
    foreach (var prop in metadata.Properties)
    {
        Console.WriteLine($"  {prop.Key}: {prop.Value}");
    }
}
```

## Usage Examples

### Basic Metadata Extraction
```csharp
using AzureImage.Utilities;

// From file path
var metadata = await ImageMetadataExtractor.ExtractMetadataAsync("photo.jpg");
DisplayMetadata(metadata);

// From stream
using var httpClient = new HttpClient();
using var stream = await httpClient.GetStreamAsync("https://example.com/image.png");
var webMetadata = await ImageMetadataExtractor.ExtractMetadataAsync(stream);
DisplayMetadata(webMetadata);

void DisplayMetadata(ImageMetadata metadata)
{
    Console.WriteLine($"Image Information:");
    Console.WriteLine($"  Dimensions: {metadata.Width} x {metadata.Height} pixels");
    Console.WriteLine($"  Format: {metadata.Format}");
    Console.WriteLine($"  File Size: {metadata.Size:N0} bytes");
    Console.WriteLine($"  Aspect Ratio: {(double)metadata.Width / metadata.Height:F2}");
}
```

### Metadata Validation and Processing
```csharp
public async Task<bool> ProcessImageWithValidation(string imagePath)
{
    try
    {
        // First check if metadata is valid
        using var stream = File.OpenRead(imagePath);
        bool hasValidMetadata = await ImageMetadataExtractor.HasValidMetadataAsync(stream);
        
        if (!hasValidMetadata)
        {
            Console.WriteLine("Image has invalid metadata");
            return false;
        }

        // Extract full metadata
        var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(imagePath);
        
        // Validate dimensions
        if (metadata.Width <= 0 || metadata.Height <= 0)
        {
            Console.WriteLine("Invalid image dimensions");
            return false;
        }

        // Check file size
        if (metadata.Size > 10 * 1024 * 1024) // 10MB limit
        {
            Console.WriteLine("Image file too large");
            return false;
        }

        Console.WriteLine("Image validation successful");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing image: {ex.Message}");
        return false;
    }
}
```

### Batch Metadata Analysis
```csharp
public async Task AnalyzeImageDirectory(string directoryPath)
{
    var imageFiles = Directory.GetFiles(directoryPath, "*.*")
        .Where(f => ImageFormatConverter.IsValidImageFormat(Path.GetExtension(f)))
        .ToArray();

    var metadataList = new List<(string FileName, ImageMetadata Metadata)>();

    foreach (string filePath in imageFiles)
    {
        try
        {
            var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(filePath);
            metadataList.Add((Path.GetFileName(filePath), metadata));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to process {filePath}: {ex.Message}");
        }
    }

    // Generate summary report
    Console.WriteLine($"\nImage Directory Analysis Report");
    Console.WriteLine($"Total Images Processed: {metadataList.Count}");
    Console.WriteLine($"Total Size: {metadataList.Sum(m => m.Metadata.Size):N0} bytes");
    
    var formatGroups = metadataList.GroupBy(m => m.Metadata.Format);
    Console.WriteLine("\nFormat Distribution:");
    foreach (var group in formatGroups)
    {
        Console.WriteLine($"  {group.Key}: {group.Count()} files");
    }

    var avgDimensions = metadataList.Select(m => new { m.Metadata.Width, m.Metadata.Height });
    if (avgDimensions.Any())
    {
        Console.WriteLine($"\nAverage Dimensions: {avgDimensions.Average(d => d.Width):F0} x {avgDimensions.Average(d => d.Height):F0}");
    }
}
```

## Best Practices

### ✅ Do's
- **Always validate metadata** before processing images
- **Handle exceptions gracefully** when dealing with corrupted files
- **Use async methods** for better performance with I/O operations
- **Check stream position** when working with streams multiple times
- **Dispose of streams properly** using `using` statements
- **Validate file existence** before attempting to extract metadata

### ❌ Don'ts
- **Don't assume metadata is always available** - some images may have minimal metadata
- **Don't ignore exceptions** - handle them appropriately for your use case
- **Don't forget to reset stream position** when reading streams multiple times
- **Don't hardcode assumptions** about image properties
- **Don't leave streams open** without proper disposal

### Performance Considerations
- **Async operations** prevent UI blocking during metadata extraction
- **Stream processing** is memory efficient for large files
- **Caching results** can improve performance for repeated access
- **Batch processing** is more efficient than individual file processing

## Error Handling

Common exceptions and how to handle them:

```csharp
try
{
    var metadata = await ImageMetadataExtractor.ExtractMetadataAsync("image.jpg");
    // Process metadata...
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Image file not found: {ex.FileName}");
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Invalid parameter: {ex.ParamName}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid stream or file: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error extracting metadata: {ex.Message}");
}
```

## Current Implementation Status

⚠️ **Note**: The current implementation includes placeholder logic for metadata extraction. Full implementation with actual image processing libraries (like System.Drawing, ImageSharp, or similar) is planned for future releases.

### What's Currently Available
- ✅ API structure and interfaces
- ✅ Basic validation and error handling
- ✅ Async support
- ✅ Stream and file path support

### Planned Enhancements
- 🔄 Full EXIF data extraction
- 🔄 Advanced format detection
- 🔄 Color profile information
- 🔄 Camera settings and technical data
- 🔄 GPS location data extraction

## Related Utilities

- **[ImageFormatConverter](ImageFormatConverter.md)** - Handle image formats and MIME types
- **[ImageSizeValidator](ImageSizeValidator.md)** - Validate image dimensions and file sizes
- **[ImageQualityAnalyzer](ImageQualityAnalyzer.md)** - Analyze image quality metrics
- **[AspectRatioConverter](AspectRatioConverter.md)** - Work with aspect ratios and dimensions

## Troubleshooting

### Common Issues

**Q: Getting null or empty metadata**
A: Ensure the image file is valid and readable. Some images may have minimal metadata.

**Q: Stream position errors**
A: Reset the stream position to 0 before multiple read operations.

**Q: FileNotFoundException for existing files**
A: Check file permissions and ensure the path is correct and accessible.

**Q: Invalid metadata for seemingly valid images**
A: Some corrupted or unusual image files may not have readable metadata.

### Support

For more help with metadata extraction:
- Check the [Azure Image SDK Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- Review the [Best Practices Guide](../Best-Practices/Image-Processing.md)
- See [Troubleshooting Common Issues](../Troubleshooting/Common-Issues.md) 