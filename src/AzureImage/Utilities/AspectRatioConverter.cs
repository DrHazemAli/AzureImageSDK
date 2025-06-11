using System;
using System.Globalization;

namespace AzureImage.Utilities
{
    /// <summary>
    /// Utility class for converting aspect ratio strings to width and height dimensions.
    /// </summary>
    public static class AspectRatioConverter
    {
        /// <summary>
        /// Converts an aspect ratio string (e.g., "16:9", "4:3") to width and height dimensions.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio string in the format "width:height"</param>
        /// <param name="targetWidth">The target width to calculate the height for</param>
        /// <returns>A tuple containing the width and height</returns>
        /// <exception cref="ArgumentException">Thrown when the aspect ratio string is invalid</exception>
        public static (int Width, int Height) ConvertToDimensions(string aspectRatio, int targetWidth)
        {
            if (string.IsNullOrWhiteSpace(aspectRatio))
                throw new ArgumentException("Aspect ratio cannot be null or empty", nameof(aspectRatio));

            if (targetWidth <= 0)
                throw new ArgumentException("Target width must be greater than 0", nameof(targetWidth));

            var parts = aspectRatio.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException("Aspect ratio must be in the format 'width:height'", nameof(aspectRatio));

            if (!double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double widthRatio) ||
                !double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double heightRatio))
            {
                throw new ArgumentException("Invalid aspect ratio numbers", nameof(aspectRatio));
            }

            if (widthRatio <= 0 || heightRatio <= 0)
                throw new ArgumentException("Aspect ratio numbers must be greater than 0", nameof(aspectRatio));

            int height = (int)Math.Round(targetWidth * (heightRatio / widthRatio));
            return (targetWidth, height);
        }

        /// <summary>
        /// Converts an aspect ratio string (e.g., "16:9", "4:3") to width and height dimensions.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio string in the format "width:height"</param>
        /// <param name="targetHeight">The target height to calculate the width for</param>
        /// <returns>A tuple containing the width and height</returns>
        /// <exception cref="ArgumentException">Thrown when the aspect ratio string is invalid</exception>
        public static (int Width, int Height) ConvertToDimensionsFromHeight(string aspectRatio, int targetHeight)
        {
            if (string.IsNullOrWhiteSpace(aspectRatio))
                throw new ArgumentException("Aspect ratio cannot be null or empty", nameof(aspectRatio));

            if (targetHeight <= 0)
                throw new ArgumentException("Target height must be greater than 0", nameof(targetHeight));

            var parts = aspectRatio.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException("Aspect ratio must be in the format 'width:height'", nameof(aspectRatio));

            if (!double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double widthRatio) ||
                !double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double heightRatio))
            {
                throw new ArgumentException("Invalid aspect ratio numbers", nameof(aspectRatio));
            }

            if (widthRatio <= 0 || heightRatio <= 0)
                throw new ArgumentException("Aspect ratio numbers must be greater than 0", nameof(aspectRatio));

            int width = (int)Math.Round(targetHeight * (widthRatio / heightRatio));
            return (width, targetHeight);
        }

        /// <summary>
        /// Validates if a string is a valid aspect ratio format.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio string to validate</param>
        /// <returns>True if the aspect ratio string is valid, false otherwise</returns>
        public static bool IsValidAspectRatio(string aspectRatio)
        {
            if (string.IsNullOrWhiteSpace(aspectRatio))
                return false;

            var parts = aspectRatio.Split(':');
            if (parts.Length != 2)
                return false;

            if (!double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double widthRatio) ||
                !double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double heightRatio))
            {
                return false;
            }

            return widthRatio > 0 && heightRatio > 0;
        }
    }
} 