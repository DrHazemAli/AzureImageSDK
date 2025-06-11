using AzureAISDK.Inference.Image.StableImageUltra;
using FluentAssertions;
using Xunit;

namespace AzureAISDK.Tests.Inference.Tests.Image.Tests.StableImageUltra.Tests;

public class ImageGenerationResponseTests
{
    [Fact]
    public void GetImageBytes_WithValidBase64_ShouldReturnBytes()
    {
        // Arrange
        var testBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var base64String = Convert.ToBase64String(testBytes);
        var response = new ImageGenerationResponse
        {
            Image = base64String
        };

        // Act
        var result = response.GetImageBytes();

        // Assert
        result.Should().Equal(testBytes);
    }

    [Fact]
    public void GetImageBytes_WithEmptyImage_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var response = new ImageGenerationResponse
        {
            Image = ""
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => response.GetImageBytes());
    }

    [Fact]
    public void GetImageBytes_WithNullImage_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var response = new ImageGenerationResponse
        {
            Image = null!
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => response.GetImageBytes());
    }

    [Fact]
    public void GetImageBytes_WithInvalidBase64_ShouldThrowFormatException()
    {
        // Arrange
        var response = new ImageGenerationResponse
        {
            Image = "invalid-base64-string"
        };

        // Act & Assert
        Assert.Throws<FormatException>(() => response.GetImageBytes());
    }

    [Fact]
    public async Task SaveImageAsync_WithValidData_ShouldSaveFile()
    {
        // Arrange
        var testBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }; // PNG header
        var base64String = Convert.ToBase64String(testBytes);
        var response = new ImageGenerationResponse
        {
            Image = base64String
        };

        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            await response.SaveImageAsync(tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var savedBytes = await File.ReadAllBytesAsync(tempFile);
            savedBytes.Should().Equal(testBytes);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SaveImageAsync_WithInvalidFilePath_ShouldThrowArgumentException(string filePath)
    {
        // Arrange
        var response = new ImageGenerationResponse
        {
            Image = Convert.ToBase64String(new byte[] { 1, 2, 3 })
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => response.SaveImageAsync(filePath));
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var response = new ImageGenerationResponse();

        // Assert
        response.Image.Should().Be(string.Empty);
        response.Metadata.Should().BeNull();
    }

    [Fact]
    public void Metadata_WithAllProperties_ShouldBeSetCorrectly()
    {
        // Arrange
        var metadata = new ImageMetadata
        {
            Width = 1024,
            Height = 1024,
            Format = "png",
            Seed = 12345,
            Model = "Stable-Image-Ultra"
        };

        var response = new ImageGenerationResponse
        {
            Image = "test-image-data",
            Metadata = metadata
        };

        // Act & Assert
        response.Metadata.Should().NotBeNull();
        response.Metadata!.Width.Should().Be(1024);
        response.Metadata.Height.Should().Be(1024);
        response.Metadata.Format.Should().Be("png");
        response.Metadata.Seed.Should().Be(12345);
        response.Metadata.Model.Should().Be("Stable-Image-Ultra");
    }
} 