# AspectRatioConverter

The `AspectRatioConverter` is a utility class that helps convert aspect ratio strings to width and height dimensions. This is particularly useful when working with image generation and manipulation, where you need to maintain specific aspect ratios while working with different dimensions.

## Usage

```csharp
using AzureImage.Utilities;

// Convert aspect ratio to dimensions based on target width
var (width, height) = AspectRatioConverter.ConvertToDimensions("16:9", 1920);
// Result: width = 1920, height = 1080

// Convert aspect ratio to dimensions based on target height
var (width, height) = AspectRatioConverter.ConvertToDimensionsFromHeight("16:9", 1080);
// Result: width = 1920, height = 1080

// Validate an aspect ratio string
bool isValid = AspectRatioConverter.IsValidAspectRatio("16:9");
// Result: true
```

## Methods

### ConvertToDimensions

```csharp
public static (int Width, int Height) ConvertToDimensions(string aspectRatio, int targetWidth)
```

Converts an aspect ratio string to width and height dimensions based on a target width.

#### Parameters

- `aspectRatio` (string): The aspect ratio string in the format "width:height" (e.g., "16:9", "4:3")
- `targetWidth` (int): The target width to calculate the height for

#### Returns

A tuple containing the width and height dimensions.

#### Exceptions

- `ArgumentException`: Thrown when:
  - The aspect ratio string is null, empty, or invalid
  - The target width is less than or equal to 0
  - The aspect ratio numbers are invalid or less than or equal to 0

### ConvertToDimensionsFromHeight

```csharp
public static (int Width, int Height) ConvertToDimensionsFromHeight(string aspectRatio, int targetHeight)
```

Converts an aspect ratio string to width and height dimensions based on a target height.

#### Parameters

- `aspectRatio` (string): The aspect ratio string in the format "width:height" (e.g., "16:9", "4:3")
- `targetHeight` (int): The target height to calculate the width for

#### Returns

A tuple containing the width and height dimensions.

#### Exceptions

- `ArgumentException`: Thrown when:
  - The aspect ratio string is null, empty, or invalid
  - The target height is less than or equal to 0
  - The aspect ratio numbers are invalid or less than or equal to 0

### IsValidAspectRatio

```csharp
public static bool IsValidAspectRatio(string aspectRatio)
```

Validates if a string is a valid aspect ratio format.

#### Parameters

- `aspectRatio` (string): The aspect ratio string to validate

#### Returns

`true` if the aspect ratio string is valid, `false` otherwise.

## Examples

### Common Aspect Ratios

```csharp
// 16:9 (Widescreen)
var dimensions = AspectRatioConverter.ConvertToDimensions("16:9", 1920);
// Result: (1920, 1080)

// 4:3 (Standard)
var dimensions = AspectRatioConverter.ConvertToDimensions("4:3", 1024);
// Result: (1024, 768)

// 1:1 (Square)
var dimensions = AspectRatioConverter.ConvertToDimensions("1:1", 512);
// Result: (512, 512)

// 2:1 (Ultrawide)
var dimensions = AspectRatioConverter.ConvertToDimensions("2:1", 1000);
// Result: (1000, 500)
```

### Error Handling

```csharp
try
{
    var dimensions = AspectRatioConverter.ConvertToDimensions("invalid", 1920);
}
catch (ArgumentException ex)
{
    // Handle invalid aspect ratio
}

try
{
    var dimensions = AspectRatioConverter.ConvertToDimensions("16:9", 0);
}
catch (ArgumentException ex)
{
    // Handle invalid target width
}
```

## Best Practices

1. Always validate aspect ratio strings before using them in production code
2. Handle exceptions appropriately when working with user input
3. Consider rounding errors when working with non-standard aspect ratios
4. Use the appropriate method based on whether you're working with a target width or height 