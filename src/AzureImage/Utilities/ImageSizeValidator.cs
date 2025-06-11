using System;

namespace AzureImage.Utilities
{
    /// <summary>
    /// Utility class for validating and scaling image dimensions.
    /// </summary>
    public static class ImageSizeValidator
    {
        /// <summary>
        /// Validates if the given dimensions are within the specified maximum bounds.
        /// </summary>
        /// <param name="width">The width to validate</param>
        /// <param name="height">The height to validate</param>
        /// <param name="maxWidth">The maximum allowed width</param>
        /// <param name="maxHeight">The maximum allowed height</param>
        /// <returns>True if the dimensions are valid, false otherwise</returns>
        public static bool IsValidSize(int width, int height, int maxWidth, int maxHeight)
        {
            if (width <= 0 || height <= 0)
                return false;

            if (maxWidth <= 0 || maxHeight <= 0)
                throw new ArgumentException("Maximum dimensions must be greater than 0");

            return width <= maxWidth && height <= maxHeight;
        }

        /// <summary>
        /// Validates if the given dimensions are within the specified constraints.
        /// </summary>
        /// <param name="width">The width to validate</param>
        /// <param name="height">The height to validate</param>
        /// <param name="constraints">The size constraints to validate against</param>
        /// <returns>True if the dimensions are valid, false otherwise</returns>
        public static bool IsWithinBounds(int width, int height, SizeConstraints constraints)
        {
            if (width <= 0 || height <= 0)
                return false;

            if (constraints == null)
                throw new ArgumentNullException(nameof(constraints));

            bool isValid = true;

            if (constraints.MaxWidth.HasValue)
                isValid &= width <= constraints.MaxWidth.Value;

            if (constraints.MaxHeight.HasValue)
                isValid &= height <= constraints.MaxHeight.Value;

            if (constraints.MinWidth.HasValue)
                isValid &= width >= constraints.MinWidth.Value;

            if (constraints.MinHeight.HasValue)
                isValid &= height >= constraints.MinHeight.Value;

            if (constraints.MaxAspectRatio.HasValue)
            {
                double aspectRatio = (double)width / height;
                isValid &= aspectRatio <= constraints.MaxAspectRatio.Value;
            }

            if (constraints.MinAspectRatio.HasValue)
            {
                double aspectRatio = (double)width / height;
                isValid &= aspectRatio >= constraints.MinAspectRatio.Value;
            }

            return isValid;
        }

        /// <summary>
        /// Scales the dimensions to fit within the maximum bounds while maintaining aspect ratio.
        /// </summary>
        /// <param name="width">The original width</param>
        /// <param name="height">The original height</param>
        /// <param name="maxWidth">The maximum allowed width</param>
        /// <param name="maxHeight">The maximum allowed height</param>
        /// <returns>A tuple containing the scaled width and height</returns>
        public static (int Width, int Height) ScaleToFit(int width, int height, int maxWidth, int maxHeight)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Dimensions must be greater than 0");

            if (maxWidth <= 0 || maxHeight <= 0)
                throw new ArgumentException("Maximum dimensions must be greater than 0");

            double aspectRatio = (double)width / height;
            int newWidth = width;
            int newHeight = height;

            if (width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = (int)Math.Round(newWidth / aspectRatio);
            }

            if (newHeight > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)Math.Round(newHeight * aspectRatio);
            }

            return (newWidth, newHeight);
        }

        /// <summary>
        /// Scales the dimensions to fill the target bounds while maintaining aspect ratio.
        /// </summary>
        /// <param name="width">The original width</param>
        /// <param name="height">The original height</param>
        /// <param name="targetWidth">The target width</param>
        /// <param name="targetHeight">The target height</param>
        /// <returns>A tuple containing the scaled width and height</returns>
        public static (int Width, int Height) ScaleToFill(int width, int height, int targetWidth, int targetHeight)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Dimensions must be greater than 0");

            if (targetWidth <= 0 || targetHeight <= 0)
                throw new ArgumentException("Target dimensions must be greater than 0");

            double aspectRatio = (double)width / height;
            double targetAspectRatio = (double)targetWidth / targetHeight;

            int newWidth = width;
            int newHeight = height;

            if (aspectRatio > targetAspectRatio)
            {
                // Image is wider than target
                newWidth = targetWidth;
                newHeight = (int)Math.Round(newWidth / aspectRatio);
            }
            else
            {
                // Image is taller than target
                newHeight = targetHeight;
                newWidth = (int)Math.Round(newHeight * aspectRatio);
            }

            return (newWidth, newHeight);
        }
    }

    /// <summary>
    /// Represents constraints for image dimensions.
    /// </summary>
    public class SizeConstraints
    {
        /// <summary>
        /// Gets or sets the maximum allowed width.
        /// </summary>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed height.
        /// </summary>
        public int? MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed width.
        /// </summary>
        public int? MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed height.
        /// </summary>
        public int? MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed aspect ratio (width/height).
        /// </summary>
        public double? MaxAspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed aspect ratio (width/height).
        /// </summary>
        public double? MinAspectRatio { get; set; }
    }
} 