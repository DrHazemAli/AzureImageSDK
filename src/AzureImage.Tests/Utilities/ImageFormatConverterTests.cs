using System;
using Xunit;

namespace AzureImage.Tests.Utilities
{
    public class ImageFormatConverterTests
    {
        [Theory]
        [InlineData(".jpg", "image/jpeg")]
        [InlineData(".jpeg", "image/jpeg")]
        [InlineData(".png", "image/png")]
        [InlineData(".gif", "image/gif")]
        [InlineData(".bmp", "image/bmp")]
        [InlineData(".webp", "image/webp")]
        [InlineData(".tiff", "image/tiff")]
        [InlineData(".svg", "image/svg+xml")]
        [InlineData(".ico", "image/x-icon")]
        public void GetMimeType_ValidExtension_ReturnsCorrectMimeType(string extension, string expectedMimeType)
        {
            // Act
            var mimeType = ImageFormatConverter.GetMimeType(extension);

            // Assert
            Assert.Equal(expectedMimeType, mimeType);
        }

        [Theory]
        [InlineData("jpg", "image/jpeg")]
        [InlineData("jpeg", "image/jpeg")]
        [InlineData("png", "image/png")]
        public void GetMimeType_ExtensionWithoutDot_ReturnsCorrectMimeType(string extension, string expectedMimeType)
        {
            // Act
            var mimeType = ImageFormatConverter.GetMimeType(extension);

            // Assert
            Assert.Equal(expectedMimeType, mimeType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(".invalid")]
        public void GetMimeType_InvalidExtension_ThrowsArgumentException(string extension)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageFormatConverter.GetMimeType(extension));
        }

        [Theory]
        [InlineData("image/jpeg", ".jpg")]
        [InlineData("image/png", ".png")]
        [InlineData("image/gif", ".gif")]
        [InlineData("image/bmp", ".bmp")]
        [InlineData("image/webp", ".webp")]
        [InlineData("image/tiff", ".tiff")]
        [InlineData("image/svg+xml", ".svg")]
        [InlineData("image/x-icon", ".ico")]
        public void GetFileExtension_ValidMimeType_ReturnsCorrectExtension(string mimeType, string expectedExtension)
        {
            // Act
            var extension = ImageFormatConverter.GetFileExtension(mimeType);

            // Assert
            Assert.Equal(expectedExtension, extension);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid/mime")]
        public void GetFileExtension_InvalidMimeType_ThrowsArgumentException(string mimeType)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ImageFormatConverter.GetFileExtension(mimeType));
        }

        [Theory]
        [InlineData(".jpg")]
        [InlineData(".png")]
        [InlineData(".gif")]
        [InlineData("image/jpeg")]
        [InlineData("image/png")]
        [InlineData("image/gif")]
        public void IsValidImageFormat_ValidFormat_ReturnsTrue(string format)
        {
            // Act
            var isValid = ImageFormatConverter.IsValidImageFormat(format);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(".invalid")]
        [InlineData("invalid/mime")]
        public void IsValidImageFormat_InvalidFormat_ReturnsFalse(string format)
        {
            // Act
            var isValid = ImageFormatConverter.IsValidImageFormat(format);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void GetSupportedFormats_ReturnsAllSupportedFormats()
        {
            // Act
            var formats = ImageFormatConverter.GetSupportedFormats();

            // Assert
            Assert.Contains(".jpg", formats);
            Assert.Contains(".png", formats);
            Assert.Contains(".gif", formats);
            Assert.Contains(".bmp", formats);
            Assert.Contains(".webp", formats);
            Assert.Contains(".tiff", formats);
            Assert.Contains(".svg", formats);
            Assert.Contains(".ico", formats);
        }

        [Fact]
        public void GetSupportedMimeTypes_ReturnsAllSupportedMimeTypes()
        {
            // Act
            var mimeTypes = ImageFormatConverter.GetSupportedMimeTypes();

            // Assert
            Assert.Contains("image/jpeg", mimeTypes);
            Assert.Contains("image/png", mimeTypes);
            Assert.Contains("image/gif", mimeTypes);
            Assert.Contains("image/bmp", mimeTypes);
            Assert.Contains("image/webp", mimeTypes);
            Assert.Contains("image/tiff", mimeTypes);
            Assert.Contains("image/svg+xml", mimeTypes);
            Assert.Contains("image/x-icon", mimeTypes);
        }
    }
} 