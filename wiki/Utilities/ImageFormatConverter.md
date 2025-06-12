# ImageFormatConverter

The **ImageFormatConverter** utility class provides functionality for handling image formats and MIME types conversions. This class is essential for working with different image formats and ensuring compatibility across various systems and APIs.

## Overview

The `ImageFormatConverter` is a static utility class that helps developers:
- Convert between file extensions and MIME types
- Validate image format support
- Get lists of supported formats
- Ensure format compatibility for image processing operations

## Features

### ✅ Format Support
- **JPEG** (`.jpg`, `.jpeg`) - `image/jpeg`
- **PNG** (`.png`) - `image/png`
- **GIF** (`.gif`) - `image/gif`
- **BMP** (`.bmp`) - `image/bmp`
- **WebP** (`.webp`) - `image/webp`
- **TIFF** (`.tiff`) - `image/tiff`
- **SVG** (`.svg`) - `image/svg+xml`
- **ICO** (`.ico`) - `image/x-icon`

### 🔄 Conversion Capabilities
- File extension to MIME type conversion
- MIME type to file extension conversion
- Format validation
- Supported format enumeration

## API Reference

### Methods

#### GetMimeType(string fileExtension)
Converts a file extension to its corresponding MIME type.

**Parameters:**
- `fileExtension` (string): The file extension (e.g., ".jpg", ".png", "jpg")

**Returns:**
- `string`: The corresponding MIME type

**Exceptions:**
- `ArgumentException`: Thrown when the file extension is null, empty, or unsupported

**Example:**
```csharp
using AzureImage.Utilities;

// Get MIME type from extension
string mimeType = ImageFormatConverter.GetMimeType(".jpg");
Console.WriteLine(mimeType); // Output: image/jpeg

// Works without leading dot
string mimeType2 = ImageFormatConverter.GetMimeType("png");
Console.WriteLine(mimeType2); // Output: image/png
```

#### GetFileExtension(string mimeType)
Converts a MIME type to its corresponding file extension.

**Parameters:**
- `mimeType` (string): The MIME type (e.g., "image/jpeg", "image/png")

**Returns:**
- `string`: The corresponding file extension (with leading dot)

**Exceptions:**
- `ArgumentException`: Thrown when the MIME type is null, empty, or unsupported

**Example:**
```csharp
// Get file extension from MIME type
string extension = ImageFormatConverter.GetFileExtension("image/jpeg");
Console.WriteLine(extension); // Output: .jpg

string extension2 = ImageFormatConverter.GetFileExtension("image/png");
Console.WriteLine(extension2); // Output: .png
```

#### IsValidImageFormat(string format)
Validates whether a given format (file extension or MIME type) is supported.

**Parameters:**
- `format` (string): The format to validate (can be file extension or MIME type)

**Returns:**
- `bool`: True if the format is supported, false otherwise

**Example:**
```csharp
// Validate file extensions
bool isValid1 = ImageFormatConverter.IsValidImageFormat(".jpg"); // true
bool isValid2 = ImageFormatConverter.IsValidImageFormat("png");  // true
bool isValid3 = ImageFormatConverter.IsValidImageFormat(".xyz"); // false

// Validate MIME types
bool isValid4 = ImageFormatConverter.IsValidImageFormat("image/jpeg"); // true
bool isValid5 = ImageFormatConverter.IsValidImageFormat("image/png");  // true
bool isValid6 = ImageFormatConverter.IsValidImageFormat("text/plain"); // false
```

#### GetSupportedFormats()
Gets an array of all supported file extensions.

**Returns:**
- `string[]`: Array of supported file extensions (with leading dots)

**Example:**
```csharp
string[] formats = ImageFormatConverter.GetSupportedFormats();
foreach (string format in formats)
{
    Console.WriteLine(format);
}
// Output: .jpg, .jpeg, .png, .gif, .bmp, .webp, .tiff, .svg, .ico
```

#### GetSupportedMimeTypes()
Gets an array of all supported MIME types.

**Returns:**
- `string[]`: Array of supported MIME types (distinct values)

**Example:**
```csharp
string[] mimeTypes = ImageFormatConverter.GetSupportedMimeTypes();
foreach (string mimeType in mimeTypes)
{
    Console.WriteLine(mimeType);
}
// Output: image/jpeg, image/png, image/gif, etc.
```

## Usage Examples

### Basic Format Conversion
```csharp
using AzureImage.Utilities;

// Convert file extension to MIME type
string mimeType = ImageFormatConverter.GetMimeType(".jpg");
Console.WriteLine($"MIME type for .jpg: {mimeType}");

// Convert MIME type to file extension  
string extension = ImageFormatConverter.GetFileExtension("image/png");
Console.WriteLine($"Extension for image/png: {extension}");
```

### Format Validation
```csharp
// Check if format is supported before processing
string userFormat = ".webp";
if (ImageFormatConverter.IsValidImageFormat(userFormat))
{
    string mimeType = ImageFormatConverter.GetMimeType(userFormat);
    Console.WriteLine($"Processing {userFormat} as {mimeType}");
}
else
{
    Console.WriteLine($"Unsupported format: {userFormat}");
}
```

### Working with HTTP Content
```csharp
// Determine file extension from HTTP Content-Type header
string contentType = "image/jpeg";
if (ImageFormatConverter.IsValidImageFormat(contentType))
{
    string extension = ImageFormatConverter.GetFileExtension(contentType);
    string fileName = $"downloaded_image{extension}";
    Console.WriteLine($"Saving as: {fileName}");
}
```

### Format Enumeration
```csharp
// Display all supported formats
Console.WriteLine("Supported Image Formats:");
string[] formats = ImageFormatConverter.GetSupportedFormats();
string[] mimeTypes = ImageFormatConverter.GetSupportedMimeTypes();

for (int i = 0; i < formats.Length; i++)
{
    string mimeType = ImageFormatConverter.GetMimeType(formats[i]);
    Console.WriteLine($"{formats[i]} -> {mimeType}");
}
```

### File Upload Validation
```csharp
public bool ValidateUploadedFile(string fileName, string contentType)
{
    // Get extension from filename
    string extension = Path.GetExtension(fileName);
    
    // Validate both extension and MIME type
    bool extensionValid = ImageFormatConverter.IsValidImageFormat(extension);
    bool mimeTypeValid = ImageFormatConverter.IsValidImageFormat(contentType);
    
    // Ensure they match
    if (extensionValid && mimeTypeValid)
    {
        string expectedMimeType = ImageFormatConverter.GetMimeType(extension);
        return contentType.Equals(expectedMimeType, StringComparison.OrdinalIgnoreCase);
    }
    
    return false;
}
```

## Best Practices

### ✅ Do's
- **Always validate formats** before processing images
- **Use case-insensitive comparisons** when working with user input
- **Handle exceptions** gracefully when converting unknown formats
- **Check both extension and MIME type** for uploaded files
- **Use the utility for consistency** across your application

### ❌ Don'ts
- **Don't assume formats are supported** without validation
- **Don't hardcode format mappings** - use the utility instead
- **Don't ignore case sensitivity** in format strings
- **Don't mix file extensions and MIME types** in the same context

### Performance Considerations
- The utility uses static dictionaries for **O(1) lookup performance**
- **Thread-safe** for concurrent usage
- **Memory efficient** with pre-computed mappings
- **No external dependencies** required

## Error Handling

The utility throws `ArgumentException` for unsupported formats:

```csharp
try
{
    string mimeType = ImageFormatConverter.GetMimeType(".xyz");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    // Error: Unsupported file extension: .xyz
}
```

## Integration Examples

### ASP.NET Core File Upload
```csharp
[HttpPost]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    if (file != null)
    {
        string extension = Path.GetExtension(file.FileName);
        
        if (ImageFormatConverter.IsValidImageFormat(extension))
        {
            string expectedMimeType = ImageFormatConverter.GetMimeType(extension);
            
            if (file.ContentType == expectedMimeType)
            {
                // Process valid image file
                return Ok("Image uploaded successfully");
            }
        }
    }
    
    return BadRequest("Invalid image format");
}
```

### Azure Blob Storage
```csharp
public async Task<string> UploadImageToBlobAsync(Stream imageStream, string fileName)
{
    string extension = Path.GetExtension(fileName);
    
    if (!ImageFormatConverter.IsValidImageFormat(extension))
    {
        throw new ArgumentException("Unsupported image format");
    }
    
    string contentType = ImageFormatConverter.GetMimeType(extension);
    
    // Upload to blob storage with correct content type
    var blobClient = containerClient.GetBlobClient(fileName);
    await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType });
    
    return blobClient.Uri.ToString();
}
```

## Related Utilities

- **[ImageSizeValidator](ImageSizeValidator.md)** - Validate image dimensions and file sizes
- **[ImageMetadataExtractor](ImageMetadataExtractor.md)** - Extract comprehensive image metadata
- **[ImageQualityAnalyzer](ImageQualityAnalyzer.md)** - Analyze image quality metrics
- **[AspectRatioConverter](AspectRatioConverter.md)** - Work with aspect ratios and dimensions

## Troubleshooting

### Common Issues

**Q: Getting "Unsupported file extension" error**
A: Ensure the extension includes the leading dot (`.jpg` not `jpg`) or use the format without it.

**Q: MIME type conversion fails**
A: Verify the MIME type string is exactly correct (case-sensitive).

**Q: Format validation returns false for valid formats**
A: Check for typos or unsupported formats. Use `GetSupportedFormats()` to see all supported formats.

### Support

For more help with image format handling:
- Check the [Azure Image SDK Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- Review the [Best Practices Guide](../Best-Practices/Image-Processing.md)
- See [Troubleshooting Common Issues](../Troubleshooting/Common-Issues.md) 