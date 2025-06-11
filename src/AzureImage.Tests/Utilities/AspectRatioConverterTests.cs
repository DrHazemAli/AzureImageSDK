using System;
using AzureImage.Utilities;
using Xunit;

namespace AzureImage.Tests.Utilities
{
    public class AspectRatioConverterTests
    {
        [Theory]
        [InlineData("16:9", 1920, 1080)]
        [InlineData("4:3", 1024, 768)]
        [InlineData("1:1", 512, 512)]
        [InlineData("2:1", 1000, 500)]
        public void ConvertToDimensions_ValidInput_ReturnsCorrectDimensions(string aspectRatio, int targetWidth, int expectedHeight)
        {
            // Act
            var (width, height) = AspectRatioConverter.ConvertToDimensions(aspectRatio, targetWidth);

            // Assert
            Assert.Equal(targetWidth, width);
            Assert.Equal(expectedHeight, height);
        }

        [Theory]
        [InlineData("16:9", 1080, 1920)]
        [InlineData("4:3", 768, 1024)]
        [InlineData("1:1", 512, 512)]
        [InlineData("2:1", 500, 1000)]
        public void ConvertToDimensionsFromHeight_ValidInput_ReturnsCorrectDimensions(string aspectRatio, int targetHeight, int expectedWidth)
        {
            // Act
            var (width, height) = AspectRatioConverter.ConvertToDimensionsFromHeight(aspectRatio, targetHeight);

            // Assert
            Assert.Equal(expectedWidth, width);
            Assert.Equal(targetHeight, height);
        }

        [Theory]
        [InlineData("16:9")]
        [InlineData("4:3")]
        [InlineData("1:1")]
        [InlineData("2:1")]
        public void IsValidAspectRatio_ValidInput_ReturnsTrue(string aspectRatio)
        {
            // Act
            var isValid = AspectRatioConverter.IsValidAspectRatio(aspectRatio);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("16")]
        [InlineData("16:")]
        [InlineData(":9")]
        [InlineData("16:9:1")]
        [InlineData("0:9")]
        [InlineData("16:0")]
        [InlineData("-16:9")]
        public void IsValidAspectRatio_InvalidInput_ReturnsFalse(string aspectRatio)
        {
            // Act
            var isValid = AspectRatioConverter.IsValidAspectRatio(aspectRatio);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("16")]
        [InlineData("16:")]
        [InlineData(":9")]
        [InlineData("16:9:1")]
        public void ConvertToDimensions_InvalidAspectRatio_ThrowsArgumentException(string aspectRatio)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AspectRatioConverter.ConvertToDimensions(aspectRatio, 1920));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ConvertToDimensions_InvalidTargetWidth_ThrowsArgumentException(int targetWidth)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AspectRatioConverter.ConvertToDimensions("16:9", targetWidth));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("16")]
        [InlineData("16:")]
        [InlineData(":9")]
        [InlineData("16:9:1")]
        public void ConvertToDimensionsFromHeight_InvalidAspectRatio_ThrowsArgumentException(string aspectRatio)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AspectRatioConverter.ConvertToDimensionsFromHeight(aspectRatio, 1080));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ConvertToDimensionsFromHeight_InvalidTargetHeight_ThrowsArgumentException(int targetHeight)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AspectRatioConverter.ConvertToDimensionsFromHeight("16:9", targetHeight));
        }
    }
} 