# ImageQualityAnalyzer

The **ImageQualityAnalyzer** utility class provides functionality for analyzing various image quality metrics including sharpness, brightness, contrast, and overall quality scores. This utility is essential for automated image quality assessment, content filtering, and image processing pipelines.

## Overview

The `ImageQualityAnalyzer` is a static utility class that helps developers:
- Calculate image sharpness metrics
- Analyze brightness and contrast levels
- Generate comprehensive quality assessments
- Validate image quality for specific use cases
- Implement automated quality control systems

## Features

### 📊 Quality Metrics
- **Sharpness** - Measure image clarity and focus quality
- **Brightness** - Analyze overall luminance levels
- **Contrast** - Evaluate tonal range and dynamic range
- **Overall Score** - Composite quality assessment
- **Batch Analysis** - Process multiple images efficiently

### 🔍 Analysis Capabilities
- **Stream Processing** - Analyze from any readable stream
- **Async Operations** - Non-blocking quality analysis
- **Normalized Scores** - All metrics return values between 0-1
- **Error Handling** - Graceful handling of invalid images

## API Reference

### Methods

#### CalculateSharpnessAsync(Stream imageStream)
Calculates the sharpness of an image asynchronously.

**Parameters:**
- `imageStream` (Stream): The image stream to analyze

**Returns:**
- `Task<double>`: A value between 0 and 1 representing the image sharpness (1 = very sharp, 0 = very blurry)

**Exceptions:**
- `ArgumentNullException`: Thrown when the image stream is null
- `ArgumentException`: Thrown when the image stream is not readable

**Example:**
```csharp
using AzureImage.Utilities;

// Calculate sharpness from stream
using var fileStream = File.OpenRead("image.jpg");
double sharpness = await ImageQualityAnalyzer.CalculateSharpnessAsync(fileStream);

Console.WriteLine($"Sharpness: {sharpness:P2}"); // Output: Sharpness: 85.50%

// Interpret results
if (sharpness > 0.7)
    Console.WriteLine("Image is sharp");
else if (sharpness > 0.4)
    Console.WriteLine("Image has moderate sharpness");
else
    Console.WriteLine("Image is blurry");
```

#### CalculateBrightnessAsync(Stream imageStream)
Calculates the brightness of an image asynchronously.

**Parameters:**
- `imageStream` (Stream): The image stream to analyze

**Returns:**
- `Task<double>`: A value between 0 and 1 representing the image brightness (1 = very bright, 0 = very dark)

**Exceptions:**
- `ArgumentNullException`: Thrown when the image stream is null
- `ArgumentException`: Thrown when the image stream is not readable

**Example:**
```csharp
// Calculate brightness
using var stream = File.OpenRead("photo.png");
double brightness = await ImageQualityAnalyzer.CalculateBrightnessAsync(stream);

Console.WriteLine($"Brightness: {brightness:P2}"); // Output: Brightness: 62.30%

// Interpret results
if (brightness > 0.8)
    Console.WriteLine("Image is very bright");
else if (brightness < 0.2)
    Console.WriteLine("Image is very dark");
else
    Console.WriteLine("Image has normal brightness");
```

#### CalculateContrastAsync(Stream imageStream)
Calculates the contrast of an image asynchronously.

**Parameters:**
- `imageStream` (Stream): The image stream to analyze

**Returns:**
- `Task<double>`: A value between 0 and 1 representing the image contrast (1 = high contrast, 0 = low contrast)

**Exceptions:**
- `ArgumentNullException`: Thrown when the image stream is null
- `ArgumentException`: Thrown when the image stream is not readable

**Example:**
```csharp
// Calculate contrast
using var stream = File.OpenRead("landscape.jpg");
double contrast = await ImageQualityAnalyzer.CalculateContrastAsync(stream);

Console.WriteLine($"Contrast: {contrast:P2}"); // Output: Contrast: 74.10%

// Interpret results
if (contrast > 0.7)
    Console.WriteLine("Image has high contrast");
else if (contrast < 0.3)
    Console.WriteLine("Image has low contrast");
else
    Console.WriteLine("Image has moderate contrast");
```

#### AnalyzeQualityAsync(Stream imageStream)
Analyzes multiple quality metrics of an image asynchronously.

**Parameters:**
- `imageStream` (Stream): The image stream to analyze

**Returns:**
- `Task<ImageQualityMetrics>`: An object containing various quality metrics

**Exceptions:**
- `ArgumentNullException`: Thrown when the image stream is null
- `ArgumentException`: Thrown when the image stream is not readable

**Example:**
```csharp
// Comprehensive quality analysis
using var stream = File.OpenRead("image.jpg");
var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);

Console.WriteLine($"Quality Analysis Results:");
Console.WriteLine($"  Sharpness: {metrics.Sharpness:P2}");
Console.WriteLine($"  Brightness: {metrics.Brightness:P2}");
Console.WriteLine($"  Contrast: {metrics.Contrast:P2}");
Console.WriteLine($"  Overall Score: {metrics.OverallScore:P2}");

// Overall quality assessment
if (metrics.OverallScore > 0.8)
    Console.WriteLine("Excellent image quality");
else if (metrics.OverallScore > 0.6)
    Console.WriteLine("Good image quality");
else if (metrics.OverallScore > 0.4)
    Console.WriteLine("Fair image quality");
else
    Console.WriteLine("Poor image quality");
```

## ImageQualityMetrics Class

The `ImageQualityMetrics` class represents the quality analysis results:

### Properties

| Property | Type | Description | Range |
|----------|------|-------------|-------|
| `Sharpness` | `double` | Sharpness score | 0.0 - 1.0 |
| `Brightness` | `double` | Brightness score | 0.0 - 1.0 |
| `Contrast` | `double` | Contrast score | 0.0 - 1.0 |
| `OverallScore` | `double` | Overall quality score (average of all metrics) | 0.0 - 1.0 |

### Example Usage
```csharp
var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(imageStream);

// Individual metrics
Console.WriteLine($"Sharpness: {metrics.Sharpness:F3}");
Console.WriteLine($"Brightness: {metrics.Brightness:F3}");
Console.WriteLine($"Contrast: {metrics.Contrast:F3}");

// Overall assessment
Console.WriteLine($"Overall Quality: {metrics.OverallScore:P1}");
```

## Usage Examples

### Basic Quality Analysis
```csharp
using AzureImage.Utilities;

public async Task AnalyzeImageQuality(string imagePath)
{
    using var stream = File.OpenRead(imagePath);
    var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
    
    Console.WriteLine($"Quality Analysis for {Path.GetFileName(imagePath)}:");
    Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    Console.WriteLine($"Sharpness:      {GetQualityBar(metrics.Sharpness)} {metrics.Sharpness:P1}");
    Console.WriteLine($"Brightness:     {GetQualityBar(metrics.Brightness)} {metrics.Brightness:P1}");
    Console.WriteLine($"Contrast:       {GetQualityBar(metrics.Contrast)} {metrics.Contrast:P1}");
    Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    Console.WriteLine($"Overall Score:  {GetQualityBar(metrics.OverallScore)} {metrics.OverallScore:P1}");
}

private string GetQualityBar(double score)
{
    int bars = (int)Math.Round(score * 10);
    return new string('█', bars) + new string('░', 10 - bars);
}
```

### Batch Image Quality Assessment
```csharp
public async Task<QualityReport> AssessImageDirectory(string directoryPath)
{
    var imageFiles = Directory.GetFiles(directoryPath, "*.*")
        .Where(f => ImageFormatConverter.IsValidImageFormat(Path.GetExtension(f)))
        .ToArray();

    var report = new QualityReport();
    var results = new List<(string FileName, ImageQualityMetrics Metrics)>();

    foreach (string filePath in imageFiles)
    {
        try
        {
            using var stream = File.OpenRead(filePath);
            var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
            results.Add((Path.GetFileName(filePath), metrics));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to analyze {filePath}: {ex.Message}");
        }
    }

    // Generate report
    report.TotalImages = results.Count;
    report.AverageSharpness = results.Average(r => r.Metrics.Sharpness);
    report.AverageBrightness = results.Average(r => r.Metrics.Brightness);
    report.AverageContrast = results.Average(r => r.Metrics.Contrast);
    report.AverageOverallScore = results.Average(r => r.Metrics.OverallScore);
    
    // Quality categories
    report.ExcellentQuality = results.Count(r => r.Metrics.OverallScore > 0.8);
    report.GoodQuality = results.Count(r => r.Metrics.OverallScore > 0.6 && r.Metrics.OverallScore <= 0.8);
    report.FairQuality = results.Count(r => r.Metrics.OverallScore > 0.4 && r.Metrics.OverallScore <= 0.6);
    report.PoorQuality = results.Count(r => r.Metrics.OverallScore <= 0.4);

    // Top and bottom performers
    report.BestImage = results.OrderByDescending(r => r.Metrics.OverallScore).First();
    report.WorstImage = results.OrderBy(r => r.Metrics.OverallScore).First();

    return report;
}

public class QualityReport
{
    public int TotalImages { get; set; }
    public double AverageSharpness { get; set; }
    public double AverageBrightness { get; set; }
    public double AverageContrast { get; set; }
    public double AverageOverallScore { get; set; }
    public int ExcellentQuality { get; set; }
    public int GoodQuality { get; set; }
    public int FairQuality { get; set; }
    public int PoorQuality { get; set; }
    public (string FileName, ImageQualityMetrics Metrics) BestImage { get; set; }
    public (string FileName, ImageQualityMetrics Metrics) WorstImage { get; set; }
}
```

### Quality-Based Image Filtering
```csharp
public async Task<List<string>> FilterImagesByQuality(
    string[] imagePaths, 
    double minimumQuality = 0.6,
    QualityFilter filter = null)
{
    var acceptedImages = new List<string>();
    filter ??= new QualityFilter();

    foreach (string imagePath in imagePaths)
    {
        try
        {
            using var stream = File.OpenRead(imagePath);
            var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);

            bool passes = true;

            // Check overall quality threshold
            if (metrics.OverallScore < minimumQuality)
                passes = false;

            // Apply specific filters
            if (filter.MinSharpness.HasValue && metrics.Sharpness < filter.MinSharpness.Value)
                passes = false;

            if (filter.MaxBrightness.HasValue && metrics.Brightness > filter.MaxBrightness.Value)
                passes = false;

            if (filter.MinBrightness.HasValue && metrics.Brightness < filter.MinBrightness.Value)
                passes = false;

            if (filter.MinContrast.HasValue && metrics.Contrast < filter.MinContrast.Value)
                passes = false;

            if (passes)
            {
                acceptedImages.Add(imagePath);
                Console.WriteLine($"✓ {Path.GetFileName(imagePath)} - Quality: {metrics.OverallScore:P1}");
            }
            else
            {
                Console.WriteLine($"✗ {Path.GetFileName(imagePath)} - Quality: {metrics.OverallScore:P1} (rejected)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠ Failed to analyze {imagePath}: {ex.Message}");
        }
    }

    return acceptedImages;
}

public class QualityFilter
{
    public double? MinSharpness { get; set; }
    public double? MinBrightness { get; set; }
    public double? MaxBrightness { get; set; }
    public double? MinContrast { get; set; }
}
```

### Real-time Quality Monitoring
```csharp
public async Task MonitorImageQuality(string watchDirectory)
{
    using var watcher = new FileSystemWatcher(watchDirectory)
    {
        Filter = "*.*",
        EnableRaisingEvents = true
    };

    watcher.Created += async (sender, e) =>
    {
        if (ImageFormatConverter.IsValidImageFormat(Path.GetExtension(e.FullPath)))
        {
            await Task.Delay(500); // Allow file to be fully written
            await AnalyzeNewImage(e.FullPath);
        }
    };

    Console.WriteLine($"Monitoring {watchDirectory} for new images...");
    Console.ReadKey();
}

private async Task AnalyzeNewImage(string filePath)
{
    try
    {
        using var stream = File.OpenRead(filePath);
        var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
        
        string fileName = Path.GetFileName(filePath);
        string status = metrics.OverallScore switch
        {
            > 0.8 => "🟢 EXCELLENT",
            > 0.6 => "🟡 GOOD",
            > 0.4 => "🟠 FAIR",
            _ => "🔴 POOR"
        };

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {fileName}: {status} (Score: {metrics.OverallScore:P1})");
        
        // Log detailed metrics if quality is poor
        if (metrics.OverallScore <= 0.4)
        {
            Console.WriteLine($"  └─ Sharpness: {metrics.Sharpness:P1}, Brightness: {metrics.Brightness:P1}, Contrast: {metrics.Contrast:P1}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to analyze {filePath}: {ex.Message}");
    }
}
```

### Web API Integration
```csharp
[ApiController]
[Route("api/[controller]")]
public class ImageQualityController : ControllerBase
{
    [HttpPost("analyze")]
    public async Task<ActionResult<QualityAnalysisResult>> AnalyzeImageQuality(IFormFile image)
    {
        if (image?.Length > 0)
        {
            using var stream = image.OpenReadStream();
            
            try
            {
                var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(stream);
                
                var result = new QualityAnalysisResult
                {
                    FileName = image.FileName,
                    FileSize = image.Length,
                    Sharpness = metrics.Sharpness,
                    Brightness = metrics.Brightness,
                    Contrast = metrics.Contrast,
                    OverallScore = metrics.OverallScore,
                    QualityRating = GetQualityRating(metrics.OverallScore),
                    Recommendations = GenerateRecommendations(metrics)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to analyze image quality: {ex.Message}");
            }
        }

        return BadRequest("No image file provided");
    }

    private string GetQualityRating(double score) => score switch
    {
        > 0.8 => "Excellent",
        > 0.6 => "Good",
        > 0.4 => "Fair",
        _ => "Poor"
    };

    private List<string> GenerateRecommendations(ImageQualityMetrics metrics)
    {
        var recommendations = new List<string>();

        if (metrics.Sharpness < 0.5)
            recommendations.Add("Consider using a higher resolution or applying sharpening filters");

        if (metrics.Brightness < 0.3)
            recommendations.Add("Image appears too dark - consider increasing brightness or exposure");

        if (metrics.Brightness > 0.8)
            recommendations.Add("Image appears too bright - consider reducing exposure or brightness");

        if (metrics.Contrast < 0.4)
            recommendations.Add("Low contrast detected - consider increasing contrast or adjusting levels");

        return recommendations;
    }
}

public class QualityAnalysisResult
{
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public double Sharpness { get; set; }
    public double Brightness { get; set; }
    public double Contrast { get; set; }
    public double OverallScore { get; set; }
    public string QualityRating { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = new();
}
```

## Best Practices

### ✅ Do's
- **Use comprehensive analysis** with `AnalyzeQualityAsync()` for complete assessment
- **Set appropriate thresholds** based on your specific use case requirements
- **Handle exceptions gracefully** when analyzing potentially corrupted images
- **Consider context** - different image types have different quality expectations
- **Use batch processing** for efficiency when analyzing multiple images
- **Cache results** when analyzing the same images repeatedly

### ❌ Don'ts
- **Don't rely solely on overall score** - check individual metrics for specific issues
- **Don't use fixed thresholds** for all image types - different content has different quality expectations
- **Don't ignore edge cases** - very small or unusual images may not analyze correctly
- **Don't forget to dispose streams** properly after analysis
- **Don't assume linear scaling** - quality perception isn't always linear with metric values

### Performance Considerations
- **Async operations** prevent blocking when analyzing large images
- **Stream processing** is memory efficient for large files
- **Batch processing** reduces overhead when analyzing multiple images
- **Consider image size** - very large images may take longer to analyze

## Error Handling

Common exceptions and how to handle them:

```csharp
try
{
    var metrics = await ImageQualityAnalyzer.AnalyzeQualityAsync(imageStream);
    // Process metrics...
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Invalid parameter: {ex.ParamName}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid stream: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Analysis failed: {ex.Message}");
}
```

## Current Implementation Status

⚠️ **Note**: The current implementation includes placeholder logic for quality analysis. Full implementation with actual image processing algorithms is planned for future releases.

### What's Currently Available
- ✅ API structure and interfaces
- ✅ Quality metrics classes
- ✅ Async support
- ✅ Stream processing
- ✅ Basic validation and error handling

### Planned Enhancements
- 🔄 Advanced sharpness detection using Laplacian variance and gradient-based methods
- 🔄 Histogram-based brightness and contrast analysis
- 🔄 Noise detection and quality assessment
- 🔄 Color analysis and saturation metrics
- 🔄 BRISQUE and other no-reference quality metrics
- 🔄 Machine learning-based quality assessment

## Quality Metrics Interpretation

### Sharpness (0.0 - 1.0)
- **0.8 - 1.0**: Very sharp, crisp details
- **0.6 - 0.8**: Good sharpness, acceptable for most uses
- **0.4 - 0.6**: Moderate sharpness, may need enhancement
- **0.2 - 0.4**: Somewhat blurry, noticeable loss of detail
- **0.0 - 0.2**: Very blurry, significant detail loss

### Brightness (0.0 - 1.0)
- **0.8 - 1.0**: Very bright, may be overexposed
- **0.6 - 0.8**: Bright, well-lit image
- **0.4 - 0.6**: Normal brightness levels
- **0.2 - 0.4**: Dark, may be underexposed
- **0.0 - 0.2**: Very dark, significant underexposure

### Contrast (0.0 - 1.0)
- **0.8 - 1.0**: High contrast, dramatic tonal range
- **0.6 - 0.8**: Good contrast, clear differentiation
- **0.4 - 0.6**: Moderate contrast, acceptable range
- **0.2 - 0.4**: Low contrast, flat appearance
- **0.0 - 0.2**: Very low contrast, washed out

## Related Utilities

- **[ImageFormatConverter](ImageFormatConverter.md)** - Handle image formats and MIME types
- **[ImageMetadataExtractor](ImageMetadataExtractor.md)** - Extract comprehensive image metadata
- **[ImageSizeValidator](ImageSizeValidator.md)** - Validate image dimensions and file sizes
- **[AspectRatioConverter](AspectRatioConverter.md)** - Work with aspect ratios and dimensions

## Troubleshooting

### Common Issues

**Q: All quality metrics return 0.5**
A: This indicates the placeholder implementation is active. Full implementation is planned for future releases.

**Q: Quality analysis seems inconsistent**
A: Different image types and content may require different analysis parameters and thresholds.

**Q: Large images take too long to analyze**
A: Consider resizing images for analysis or implementing progressive analysis for very large files.

**Q: Stream position errors during analysis**
A: Ensure the stream is seekable or reset the position before analysis.

### Support

For more help with image quality analysis:
- Check the [Azure Image SDK Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- Review the [Best Practices Guide](../Best-Practices/Image-Processing.md)
- See [Troubleshooting Common Issues](../Troubleshooting/Common-Issues.md) 