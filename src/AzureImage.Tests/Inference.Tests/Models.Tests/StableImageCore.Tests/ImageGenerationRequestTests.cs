using AzureImage.Inference.Models.StableImageCore;
using FluentAssertions;
using Xunit;

namespace AzureImage.Tests.Inference.Tests.Models.Tests.StableImageCore.Tests;

public class ImageGenerationRequestTests
{
    [Fact]
    public void Validate_WithValidRequest_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Model = "Stable-Image-Core",
            Prompt = "A beautiful landscape",
            Size = "1024x1024",
            OutputFormat = "png"
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("1024x1024")]
    [InlineData("512x512")]
    [InlineData("256x256")]
    [InlineData("1920x1080")]
    public void Validate_WithValidSize_ShouldNotThrow(string size)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            Size = size
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("1024")]
    [InlineData("1024x")]
    [InlineData("x1024")]
    [InlineData("1024x1024x1024")]
    [InlineData("0x0")]
    [InlineData("-1024x1024")]
    public void Validate_WithInvalidSize_ShouldThrowArgumentException(string size)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            Size = size
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("png")]
    [InlineData("jpg")]
    [InlineData("jpeg")]
    [InlineData("webp")]
    public void Validate_WithValidOutputFormat_ShouldNotThrow(string outputFormat)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            OutputFormat = outputFormat
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var request = new ImageGenerationRequest();

        // Assert
        request.Model.Should().Be("Stable-Image-Core");
        request.Prompt.Should().Be(string.Empty);
        request.Size.Should().Be("1024x1024");
        request.OutputFormat.Should().Be("png");
        request.NegativePrompt.Should().BeNull();
        request.Seed.Should().BeNull();
    }
} 