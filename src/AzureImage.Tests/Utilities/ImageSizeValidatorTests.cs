using System;
using Xunit;

namespace AzureImage.Tests.Utilities
{
    public class ImageSizeValidatorTests
    {
        [Theory]
        [InlineData(100, 100, 200, 200, true)]
        [InlineData(200, 200, 200, 200, true)]
        [InlineData(201, 200, 200, 200, false)]
        [InlineData(200, 201, 200, 200, false)]
        public void IsValidSize_ValidatesDimensionsCorrectly(int width, int height, int maxWidth, int maxHeight, bool expected)
        {
            // Act
            var isValid = ImageSizeValidator.IsValidSize(width, height, maxWidth, maxHeight);

            // Assert
            Assert.Equal(expected, isValid);
        }

        [Theory]
        [InlineData(0, 100, 200, 200)]
        [InlineData(100, 0, 200, 200)]
        [InlineData(100, 100, 0, 200)]
        [InlineData(100, 100, 200, 0)]
        public void IsValidSize_InvalidDimensions_ThrowsArgumentException(int width, int height, int maxWidth, int maxHeight)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageSizeValidator.IsValidSize(width, height, maxWidth, maxHeight));
        }

        [Fact]
        public void IsWithinBounds_ValidConstraints_ValidatesCorrectly()
        {
            // Arrange
            var constraints = new SizeConstraints
            {
                MaxWidth = 200,
                MaxHeight = 200,
                MinWidth = 100,
                MinHeight = 100,
                MaxAspectRatio = 2.0,
                MinAspectRatio = 0.5
            };

            // Act & Assert
            Assert.True(ImageSizeValidator.IsWithinBounds(150, 150, constraints)); // Within all bounds
            Assert.False(ImageSizeValidator.IsWithinBounds(250, 150, constraints)); // Exceeds max width
            Assert.False(ImageSizeValidator.IsWithinBounds(150, 250, constraints)); // Exceeds max height
            Assert.False(ImageSizeValidator.IsWithinBounds(50, 150, constraints)); // Below min width
            Assert.False(ImageSizeValidator.IsWithinBounds(150, 50, constraints)); // Below min height
            Assert.False(ImageSizeValidator.IsWithinBounds(180, 50, constraints)); // Exceeds max aspect ratio
            Assert.False(ImageSizeValidator.IsWithinBounds(50, 180, constraints)); // Below min aspect ratio
        }

        [Fact]
        public void IsWithinBounds_NullConstraints_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ImageSizeValidator.IsWithinBounds(100, 100, null));
        }

        [Theory]
        [InlineData(100, 100, 200, 200, 100, 100)] // No scaling needed
        [InlineData(300, 200, 200, 200, 200, 133)] // Scale by width
        [InlineData(200, 300, 200, 200, 133, 200)] // Scale by height
        [InlineData(300, 300, 200, 200, 200, 200)] // Scale by both
        public void ScaleToFit_ScalesCorrectly(int width, int height, int maxWidth, int maxHeight, int expectedWidth, int expectedHeight)
        {
            // Act
            var (newWidth, newHeight) = ImageSizeValidator.ScaleToFit(width, height, maxWidth, maxHeight);

            // Assert
            Assert.Equal(expectedWidth, newWidth);
            Assert.Equal(expectedHeight, newHeight);
        }

        [Theory]
        [InlineData(0, 100, 200, 200)]
        [InlineData(100, 0, 200, 200)]
        [InlineData(100, 100, 0, 200)]
        [InlineData(100, 100, 200, 0)]
        public void ScaleToFit_InvalidDimensions_ThrowsArgumentException(int width, int height, int maxWidth, int maxHeight)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageSizeValidator.ScaleToFit(width, height, maxWidth, maxHeight));
        }

        [Theory]
        [InlineData(100, 100, 200, 200, 200, 200)] // Square to square
        [InlineData(200, 100, 200, 200, 200, 100)] // Wide to square
        [InlineData(100, 200, 200, 200, 100, 200)] // Tall to square
        [InlineData(400, 200, 200, 200, 200, 100)] // Very wide to square
        [InlineData(200, 400, 200, 200, 100, 200)] // Very tall to square
        public void ScaleToFill_ScalesCorrectly(int width, int height, int targetWidth, int targetHeight, int expectedWidth, int expectedHeight)
        {
            // Act
            var (newWidth, newHeight) = ImageSizeValidator.ScaleToFill(width, height, targetWidth, targetHeight);

            // Assert
            Assert.Equal(expectedWidth, newWidth);
            Assert.Equal(expectedHeight, newHeight);
        }

        [Theory]
        [InlineData(0, 100, 200, 200)]
        [InlineData(100, 0, 200, 200)]
        [InlineData(100, 100, 0, 200)]
        [InlineData(100, 100, 200, 0)]
        public void ScaleToFill_InvalidDimensions_ThrowsArgumentException(int width, int height, int targetWidth, int targetHeight)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageSizeValidator.ScaleToFill(width, height, targetWidth, targetHeight));
        }
    }
} 