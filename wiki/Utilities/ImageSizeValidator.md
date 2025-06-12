# ImageSizeValidator

The **ImageSizeValidator** utility class provides comprehensive functionality for validating and scaling image dimensions. It includes constraint-based validation, aspect ratio checking, and intelligent scaling algorithms for maintaining proportions while fitting within specified bounds.

## Overview

The `ImageSizeValidator` is a static utility class that helps developers:
- Validate image dimensions against maximum/minimum constraints
- Check aspect ratio requirements
- Scale images to fit within bounds while maintaining aspect ratio
- Scale images to fill target dimensions with proper aspect ratio handling
- Apply complex constraint validation with multiple criteria

## Features

### ✅ Validation Capabilities
- **Simple Size Validation** - Check against maximum width/height
- **Constraint-Based Validation** - Complex multi-criteria validation
- **Aspect Ratio Validation** - Ensure aspect ratios fall within acceptable ranges
- **Boundary Checking** - Validate minimum and maximum size requirements

### 🔧 Scaling Operations
- **Scale to Fit** - Maintain aspect ratio while fitting within bounds
- **Scale to Fill** - Maintain aspect ratio while filling target dimensions
- **Proportional Scaling** - Intelligent dimension calculation
- **Constraint Preservation** - Ensure scaled results meet requirements

## API Reference

### Methods

#### IsValidSize(int width, int height, int maxWidth, int maxHeight)
Validates if the given dimensions are within the specified maximum bounds.

**Parameters:**
- `width` (int): The width to validate
- `height` (int): The height to validate
- `maxWidth` (int): The maximum allowed width
- `maxHeight` (int): The maximum allowed height

**Returns:**
- `bool`: True if the dimensions are valid, false otherwise

**Exceptions:**
- `ArgumentException`: Thrown when maximum dimensions are not greater than 0

**Example:**
```csharp
using AzureImage.Utilities;

// Simple size validation
bool isValid = ImageSizeValidator.IsValidSize(1920, 1080, 2048, 2048);
Console.WriteLine($"1920x1080 within 2048x2048: {isValid}"); // True

bool isTooLarge = ImageSizeValidator.IsValidSize(3000, 2000, 2048, 2048);
Console.WriteLine($"3000x2000 within 2048x2048: {isTooLarge}"); // False

// Invalid dimensions return false
bool invalidDimensions = ImageSizeValidator.IsValidSize(0, 1080, 2048, 2048);
Console.WriteLine($"0x1080 is valid: {invalidDimensions}"); // False
```

#### IsWithinBounds(int width, int height, SizeConstraints constraints)
Validates if the given dimensions are within the specified constraints.

**Parameters:**
- `width` (int): The width to validate
- `height` (int): The height to validate
- `constraints` (SizeConstraints): The size constraints to validate against

**Returns:**
- `bool`: True if the dimensions are valid, false otherwise

**Exceptions:**
- `ArgumentNullException`: Thrown when constraints is null

**Example:**
```csharp
// Create complex constraints
var constraints = new SizeConstraints
{
    MinWidth = 100,
    MaxWidth = 2048,
    MinHeight = 100,
    MaxHeight = 2048,
    MinAspectRatio = 0.5,  // 1:2 (portrait)
    MaxAspectRatio = 2.0   // 2:1 (landscape)
};

// Test various dimensions
bool validLandscape = ImageSizeValidator.IsWithinBounds(1920, 1080, constraints);
Console.WriteLine($"1920x1080 within constraints: {validLandscape}"); // True

bool validPortrait = ImageSizeValidator.IsWithinBounds(1080, 1920, constraints);
Console.WriteLine($"1080x1920 within constraints: {validPortrait}"); // True

bool tooWide = ImageSizeValidator.IsWithinBounds(2048, 512, constraints);
Console.WriteLine($"2048x512 within constraints: {tooWide}"); // False (aspect ratio > 2.0)
```

#### ScaleToFit(int width, int height, int maxWidth, int maxHeight)
Scales the dimensions to fit within the maximum bounds while maintaining aspect ratio.

**Parameters:**
- `width` (int): The original width
- `height` (int): The original height
- `maxWidth` (int): The maximum allowed width
- `maxHeight` (int): The maximum allowed height

**Returns:**
- `(int Width, int Height)`: A tuple containing the scaled width and height

**Exceptions:**
- `ArgumentException`: Thrown when dimensions or maximum dimensions are not greater than 0

**Example:**
```csharp
// Scale large image to fit within bounds
var (scaledWidth, scaledHeight) = ImageSizeValidator.ScaleToFit(3000, 2000, 1024, 1024);
Console.WriteLine($"3000x2000 scaled to fit 1024x1024: {scaledWidth}x{scaledHeight}");
// Output: 3000x2000 scaled to fit 1024x1024: 1024x683

// Small images remain unchanged
var (unchanged1, unchanged2) = ImageSizeValidator.ScaleToFit(800, 600, 1024, 1024);
Console.WriteLine($"800x600 scaled to fit 1024x1024: {unchanged1}x{unchanged2}");
// Output: 800x600 scaled to fit 1024x1024: 800x600

// Portrait scaling
var (portraitW, portraitH) = ImageSizeValidator.ScaleToFit(1000, 2000, 512, 1024);
Console.WriteLine($"1000x2000 scaled to fit 512x1024: {portraitW}x{portraitH}");
// Output: 1000x2000 scaled to fit 512x1024: 512x1024
```

#### ScaleToFill(int width, int height, int targetWidth, int targetHeight)
Scales the dimensions to fill the target bounds while maintaining aspect ratio.

**Parameters:**
- `width` (int): The original width
- `height` (int): The original height
- `targetWidth` (int): The target width
- `targetHeight` (int): The target height

**Returns:**
- `(int Width, int Height)`: A tuple containing the scaled width and height

**Exceptions:**
- `ArgumentException`: Thrown when dimensions or target dimensions are not greater than 0

**Example:**
```csharp
// Scale to fill target dimensions
var (fillWidth, fillHeight) = ImageSizeValidator.ScaleToFill(1920, 1080, 800, 600);
Console.WriteLine($"1920x1080 scaled to fill 800x600: {fillWidth}x{fillHeight}");
// Output: 1920x1080 scaled to fill 800x600: 800x450

// Different aspect ratios
var (tallFillW, tallFillH) = ImageSizeValidator.ScaleToFill(1080, 1920, 500, 500);
Console.WriteLine($"1080x1920 scaled to fill 500x500: {tallFillW}x{tallFillH}");
// Output: 1080x1920 scaled to fill 500x500: 281x500
```

## SizeConstraints Class

The `SizeConstraints` class allows for complex validation criteria:

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `MaxWidth` | `int?` | Maximum allowed width (null = no limit) |
| `MaxHeight` | `int?` | Maximum allowed height (null = no limit) |
| `MinWidth` | `int?` | Minimum required width (null = no limit) |
| `MinHeight` | `int?` | Minimum required height (null = no limit) |
| `MaxAspectRatio` | `double?` | Maximum allowed aspect ratio (width/height) |
| `MinAspectRatio` | `double?` | Minimum allowed aspect ratio (width/height) |

### Example Usage
```csharp
// Create flexible constraints
var webImageConstraints = new SizeConstraints
{
    MinWidth = 320,        // Minimum for mobile
    MaxWidth = 1920,       // Maximum for web
    MinHeight = 240,       // Minimum height
    MaxHeight = 1080,      // Full HD max
    MinAspectRatio = 0.75, // 3:4 portrait minimum
    MaxAspectRatio = 1.78  // 16:9 landscape maximum
};

// Social media constraints
var instagramConstraints = new SizeConstraints
{
    MinWidth = 320,
    MaxWidth = 1080,
    MinHeight = 320,
    MaxHeight = 1080,
    MinAspectRatio = 0.8,  // Slightly portrait
    MaxAspectRatio = 1.91  // Stories format
};
```

## Usage Examples

### Image Upload Validation
```csharp
public class ImageUploadValidator
{
    private readonly SizeConstraints _uploadConstraints;

    public ImageUploadValidator()
    {
        _uploadConstraints = new SizeConstraints
        {
            MinWidth = 100,
            MaxWidth = 4096,
            MinHeight = 100,
            MaxHeight = 4096,
            MinAspectRatio = 0.1,  // Very portrait
            MaxAspectRatio = 10.0  // Very landscape
        };
    }

    public ValidationResult ValidateImageDimensions(int width, int height, long fileSize = 0)
    {
        var result = new ValidationResult();

        // Basic dimension validation
        if (width <= 0 || height <= 0)
        {
            result.IsValid = false;
            result.Errors.Add("Invalid dimensions: Width and height must be greater than 0");
            return result;
        }

        // Constraint validation
        if (!ImageSizeValidator.IsWithinBounds(width, height, _uploadConstraints))
        {
            result.IsValid = false;
            
            // Provide specific feedback
            if (width < _uploadConstraints.MinWidth || height < _uploadConstraints.MinHeight)
                result.Errors.Add($"Image too small: Minimum size is {_uploadConstraints.MinWidth}x{_uploadConstraints.MinHeight}");
            
            if (width > _uploadConstraints.MaxWidth || height > _uploadConstraints.MaxHeight)
                result.Errors.Add($"Image too large: Maximum size is {_uploadConstraints.MaxWidth}x{_uploadConstraints.MaxHeight}");

            double aspectRatio = (double)width / height;
            if (aspectRatio < _uploadConstraints.MinAspectRatio || aspectRatio > _uploadConstraints.MaxAspectRatio)
                result.Errors.Add($"Invalid aspect ratio: Must be between {_uploadConstraints.MinAspectRatio:F2} and {_uploadConstraints.MaxAspectRatio:F2}");
        }
        else
        {
            result.IsValid = true;
            result.Message = "Image dimensions are valid";
            
            // Calculate suggested thumbnail size
            var (thumbWidth, thumbHeight) = ImageSizeValidator.ScaleToFit(width, height, 300, 300);
            result.SuggestedThumbnailSize = $"{thumbWidth}x{thumbHeight}";
        }

        return result;
    }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public string SuggestedThumbnailSize { get; set; } = string.Empty;
}
```

### Responsive Image Scaling
```csharp
public class ResponsiveImageScaler
{
    private readonly Dictionary<string, (int Width, int Height)> _breakpoints;

    public ResponsiveImageScaler()
    {
        _breakpoints = new Dictionary<string, (int, int)>
        {
            ["mobile"] = (320, 480),
            ["tablet"] = (768, 1024),
            ["desktop"] = (1200, 800),
            ["large"] = (1920, 1080)
        };
    }

    public Dictionary<string, (int Width, int Height)> GenerateResponsiveSizes(int originalWidth, int originalHeight)
    {
        var responsiveSizes = new Dictionary<string, (int Width, int Height)>();

        foreach (var breakpoint in _breakpoints)
        {
            // Scale to fit within each breakpoint while maintaining aspect ratio
            var (scaledWidth, scaledHeight) = ImageSizeValidator.ScaleToFit(
                originalWidth, 
                originalHeight, 
                breakpoint.Value.Width, 
                breakpoint.Value.Height
            );

            responsiveSizes[breakpoint.Key] = (scaledWidth, scaledHeight);
        }

        return responsiveSizes;
    }

    public void DisplayResponsiveSizes(int originalWidth, int originalHeight)
    {
        Console.WriteLine($"Original Image: {originalWidth}x{originalHeight}");
        Console.WriteLine("Responsive Breakpoints:");
        Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

        var sizes = GenerateResponsiveSizes(originalWidth, originalHeight);
        
        foreach (var size in sizes)
        {
            var aspectRatio = (double)size.Value.Width / size.Value.Height;
            Console.WriteLine($"{size.Key.PadRight(8)}: {size.Value.Width}x{size.Value.Height} (ratio: {aspectRatio:F2})");
        }
    }
}
```

### Batch Image Processing with Constraints
```csharp
public class BatchImageProcessor
{
    public async Task ProcessImageBatch(string[] imagePaths, SizeConstraints constraints)
    {
        var results = new List<ProcessingResult>();

        foreach (string imagePath in imagePaths)
        {
            var result = new ProcessingResult { ImagePath = imagePath };
            
            try
            {
                // Get original dimensions (this would typically come from image metadata)
                var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(imagePath);
                result.OriginalSize = $"{metadata.Width}x{metadata.Height}";

                // Validate against constraints
                if (ImageSizeValidator.IsWithinBounds(metadata.Width, metadata.Height, constraints))
                {
                    result.Status = "Valid - No processing needed";
                    result.ProcessedSize = result.OriginalSize;
                }
                else
                {
                    // Calculate scaled dimensions
                    var (newWidth, newHeight) = ImageSizeValidator.ScaleToFit(
                        metadata.Width, 
                        metadata.Height, 
                        constraints.MaxWidth ?? metadata.Width,
                        constraints.MaxHeight ?? metadata.Height
                    );

                    result.ProcessedSize = $"{newWidth}x{newHeight}";
                    result.Status = "Scaled to fit constraints";
                    result.ScalingRatio = (double)newWidth / metadata.Width;
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Status = $"Error: {ex.Message}";
            }

            results.Add(result);
        }

        // Display summary
        DisplayProcessingSummary(results);
    }

    private void DisplayProcessingSummary(List<ProcessingResult> results)
    {
        Console.WriteLine("\n" + "=".PadRight(60, '='));
        Console.WriteLine("BATCH PROCESSING SUMMARY");
        Console.WriteLine("=".PadRight(60, '='));

        var successful = results.Where(r => r.Success).ToList();
        var failed = results.Where(r => !r.Success).ToList();

        Console.WriteLine($"Total Images: {results.Count}");
        Console.WriteLine($"Successful: {successful.Count}");
        Console.WriteLine($"Failed: {failed.Count}");

        if (successful.Any())
        {
            Console.WriteLine("\nSuccessful Processing:");
            Console.WriteLine("-".PadRight(60, '-'));
            
            foreach (var result in successful.Take(10)) // Show first 10
            {
                string fileName = Path.GetFileName(result.ImagePath);
                Console.WriteLine($"{fileName}:");
                Console.WriteLine($"  Original: {result.OriginalSize}");
                Console.WriteLine($"  Processed: {result.ProcessedSize}");
                Console.WriteLine($"  Status: {result.Status}");
                if (result.ScalingRatio < 1)
                    Console.WriteLine($"  Scaled to: {result.ScalingRatio:P1} of original size");
                Console.WriteLine();
            }

            if (successful.Count > 10)
                Console.WriteLine($"... and {successful.Count - 10} more successful");
        }

        if (failed.Any())
        {
            Console.WriteLine("\nFailed Processing:");
            Console.WriteLine("-".PadRight(60, '-'));
            
            foreach (var result in failed)
            {
                Console.WriteLine($"{Path.GetFileName(result.ImagePath)}: {result.Status}");
            }
        }
    }
}

public class ProcessingResult
{
    public string ImagePath { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string OriginalSize { get; set; } = string.Empty;
    public string ProcessedSize { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double ScalingRatio { get; set; } = 1.0;
}
```

### Dynamic Constraint Creation
```csharp
public static class ConstraintPresets
{
    public static SizeConstraints WebOptimized => new()
    {
        MinWidth = 320,
        MaxWidth = 1920,
        MinHeight = 240,
        MaxHeight = 1080,
        MinAspectRatio = 0.5,
        MaxAspectRatio = 2.5
    };

    public static SizeConstraints MobileOptimized => new()
    {
        MinWidth = 280,
        MaxWidth = 828,
        MinHeight = 280,
        MaxHeight = 1792,
        MinAspectRatio = 0.4,
        MaxAspectRatio = 2.0
    };

    public static SizeConstraints SocialMedia => new()
    {
        MinWidth = 400,
        MaxWidth = 1080,
        MinHeight = 400,
        MaxHeight = 1080,
        MinAspectRatio = 0.8,
        MaxAspectRatio = 1.91
    };

    public static SizeConstraints PrintReady => new()
    {
        MinWidth = 2400,  // 8" at 300 DPI
        MaxWidth = 7200,  // 24" at 300 DPI
        MinHeight = 3600, // 12" at 300 DPI
        MaxHeight = 10800 // 36" at 300 DPI
    };

    public static SizeConstraints CreateCustom(
        int? minWidth = null, int? maxWidth = null,
        int? minHeight = null, int? maxHeight = null,
        double? minAspectRatio = null, double? maxAspectRatio = null)
    {
        return new SizeConstraints
        {
            MinWidth = minWidth,
            MaxWidth = maxWidth,
            MinHeight = minHeight,
            MaxHeight = maxHeight,
            MinAspectRatio = minAspectRatio,
            MaxAspectRatio = maxAspectRatio
        };
    }
}

// Usage examples
public void DemonstratePresets()
{
    int testWidth = 1920, testHeight = 1080;

    // Test against different presets
    var presets = new Dictionary<string, SizeConstraints>
    {
        ["Web"] = ConstraintPresets.WebOptimized,
        ["Mobile"] = ConstraintPresets.MobileOptimized,
        ["Social"] = ConstraintPresets.SocialMedia,
        ["Print"] = ConstraintPresets.PrintReady
    };

    Console.WriteLine($"Testing {testWidth}x{testHeight} against presets:");
    Console.WriteLine();

    foreach (var preset in presets)
    {
        bool isValid = ImageSizeValidator.IsWithinBounds(testWidth, testHeight, preset.Value);
        string status = isValid ? "✓ VALID" : "✗ INVALID";
        
        Console.WriteLine($"{preset.Key.PadRight(8)}: {status}");
        
        if (!isValid)
        {
            // Show what the scaled size would be
            var maxW = preset.Value.MaxWidth ?? testWidth;
            var maxH = preset.Value.MaxHeight ?? testHeight;
            var (scaledW, scaledH) = ImageSizeValidator.ScaleToFit(testWidth, testHeight, maxW, maxH);
            Console.WriteLine($"         Scaled: {scaledW}x{scaledH}");
        }
        Console.WriteLine();
    }
}
```

## Best Practices

### ✅ Do's
- **Use appropriate constraints** for your specific use case
- **Consider aspect ratio requirements** when validating dimensions
- **Provide meaningful error messages** when validation fails
- **Scale proportionally** to maintain image quality
- **Test edge cases** like very small or very large images
- **Cache constraint objects** when using them repeatedly

### ❌ Don'ts
- **Don't ignore aspect ratios** when scaling images
- **Don't use overly restrictive constraints** without good reason
- **Don't forget to validate input dimensions** are positive
- **Don't scale up small images unnecessarily** - it reduces quality
- **Don't hardcode dimension limits** - make them configurable

### Performance Considerations
- **Constraint validation is fast** - O(1) operations
- **Scaling calculations are lightweight** - simple arithmetic
- **Batch processing is efficient** - minimal memory overhead
- **Consider caching** frequently used constraint objects

## Error Handling

Common scenarios and how to handle them:

```csharp
try
{
    var constraints = new SizeConstraints { MaxWidth = 1024, MaxHeight = 1024 };
    bool isValid = ImageSizeValidator.IsWithinBounds(width, height, constraints);
    
    if (!isValid)
    {
        // Calculate what the valid size would be
        var (validWidth, validHeight) = ImageSizeValidator.ScaleToFit(width, height, 1024, 1024);
        Console.WriteLine($"Image must be scaled to {validWidth}x{validHeight}");
    }
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid dimensions: {ex.Message}");
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Missing constraints: {ex.ParamName}");
}
```

## Integration Examples

### ASP.NET Core File Upload with Validation
```csharp
[HttpPost]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    if (file?.Length > 0)
    {
        // Extract dimensions (pseudo-code - would use actual image library)
        var (width, height) = await GetImageDimensions(file);
        
        var constraints = ConstraintPresets.WebOptimized;
        
        if (ImageSizeValidator.IsWithinBounds(width, height, constraints))
        {
            // Image is valid, process upload
            return Ok(new { message = "Image uploaded successfully", dimensions = $"{width}x{height}" });
        }
        else
        {
            // Calculate what size it should be
            var (newWidth, newHeight) = ImageSizeValidator.ScaleToFit(
                width, height, 
                constraints.MaxWidth ?? width, 
                constraints.MaxHeight ?? height
            );
            
            return BadRequest(new 
            { 
                message = "Image dimensions invalid",
                original = $"{width}x{height}",
                required = $"Max {constraints.MaxWidth}x{constraints.MaxHeight}",
                suggested = $"{newWidth}x{newHeight}"
            });
        }
    }
    
    return BadRequest("No file uploaded");
}
```

## Related Utilities

- **[ImageFormatConverter](ImageFormatConverter.md)** - Handle image formats and MIME types
- **[ImageMetadataExtractor](ImageMetadataExtractor.md)** - Extract comprehensive image metadata
- **[ImageQualityAnalyzer](ImageQualityAnalyzer.md)** - Analyze image quality metrics
- **[AspectRatioConverter](AspectRatioConverter.md)** - Work with aspect ratios and dimensions

## Troubleshooting

### Common Issues

**Q: ScaleToFit returns the same dimensions**
A: The original image already fits within the specified bounds, so no scaling is needed.

**Q: Aspect ratio validation always fails**
A: Check that your min/max aspect ratio values are reasonable (e.g., 0.5 to 2.0 for most images).

**Q: Constraint validation is too restrictive**
A: Review your constraint values and consider if they're appropriate for your use case.

**Q: Scaled dimensions seem incorrect**
A: Verify that scaling maintains aspect ratio by calculating width/height ratio before and after.

### Support

For more help with image size validation:
- Check the [Azure Image SDK Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- Review the [Best Practices Guide](../Best-Practices/Image-Processing.md)
- See [Troubleshooting Common Issues](../Troubleshooting/Common-Issues.md) 