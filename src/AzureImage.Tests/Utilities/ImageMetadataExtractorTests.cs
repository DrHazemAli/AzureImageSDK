using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AzureImage.Tests.Utilities
{
    public class ImageMetadataExtractorTests
    {
        [Fact]
        public async Task ExtractMetadataAsync_NullStream_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                ImageMetadataExtractor.ExtractMetadataAsync(null));
        }

        [Fact]
        public async Task ExtractMetadataAsync_NonReadableStream_ThrowsArgumentException()
        {
            // Arrange
            using var stream = new MemoryStream();
            stream.Close(); // Make the stream non-readable

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                ImageMetadataExtractor.ExtractMetadataAsync(stream));
        }

        [Fact]
        public async Task ExtractMetadataAsync_ValidStream_ReturnsMetadata()
        {
            // Arrange
            using var stream = new MemoryStream();
            // TODO: Add test image data to the stream

            // Act
            var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(stream);

            // Assert
            Assert.NotNull(metadata);
            Assert.Equal(stream.Length, metadata.Size);
            Assert.NotNull(metadata.Properties);
        }

        [Fact]
        public async Task ExtractMetadataAsync_NullFilePath_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                ImageMetadataExtractor.ExtractMetadataAsync(null));
        }

        [Fact]
        public async Task ExtractMetadataAsync_NonExistentFile_ThrowsFileNotFoundException()
        {
            // Arrange
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".jpg");

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => 
                ImageMetadataExtractor.ExtractMetadataAsync(filePath));
        }

        [Fact]
        public async Task ExtractMetadataAsync_ValidFile_ReturnsMetadata()
        {
            // Arrange
            var filePath = Path.Combine(Path.GetTempPath(), "test_image.jpg");
            try
            {
                // TODO: Create a test image file
                File.WriteAllBytes(filePath, new byte[100]);

                // Act
                var metadata = await ImageMetadataExtractor.ExtractMetadataAsync(filePath);

                // Assert
                Assert.NotNull(metadata);
                Assert.Equal(100, metadata.Size);
                Assert.NotNull(metadata.Properties);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Fact]
        public async Task HasValidMetadataAsync_NullStream_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                ImageMetadataExtractor.HasValidMetadataAsync(null));
        }

        [Fact]
        public async Task HasValidMetadataAsync_InvalidStream_ReturnsFalse()
        {
            // Arrange
            using var stream = new MemoryStream();
            // TODO: Add invalid image data to the stream

            // Act
            var hasValidMetadata = await ImageMetadataExtractor.HasValidMetadataAsync(stream);

            // Assert
            Assert.False(hasValidMetadata);
        }

        [Fact]
        public async Task HasValidMetadataAsync_ValidStream_ReturnsTrue()
        {
            // Arrange
            using var stream = new MemoryStream();
            // TODO: Add valid image data to the stream

            // Act
            var hasValidMetadata = await ImageMetadataExtractor.HasValidMetadataAsync(stream);

            // Assert
            Assert.True(hasValidMetadata);
        }
    }
} 