using AzureImage.Inference.Models.GPTImage1;
using FluentAssertions;
using Xunit;

namespace AzureImage.Tests.Inference.Tests.Models.Tests.GPTImage1.Tests;

public class ImageGenerationRequestTests
{
    [Fact]
    public void Validate_WithValidRequest_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Model = "gpt-image-1",
            Prompt = "A beautiful landscape with mountains and a lake",
            Size = "1024x1024",
            Quality = "high",
            OutputFormat = "PNG",
            N = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("1024x1024")]
    [InlineData("1024x1536")]
    [InlineData("1536x1024")]
    [InlineData("1024X1024")]
    [InlineData("1024X1536")]
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
    [InlineData("512x512")]
    [InlineData("2048x2048")]
    [InlineData("1024x2048")]
    [InlineData("invalid-size")]
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
    [InlineData("low")]
    [InlineData("medium")]
    [InlineData("high")]
    [InlineData("LOW")]
    [InlineData("MEDIUM")]
    [InlineData("HIGH")]
    public void Validate_WithValidQuality_ShouldNotThrow(string quality)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            Quality = quality
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("ultra")]
    [InlineData("standard")]
    [InlineData("")]
    [InlineData("invalid-quality")]
    public void Validate_WithInvalidQuality_ShouldThrowArgumentException(string quality)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            Quality = quality
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("PNG")]
    [InlineData("JPEG")]
    [InlineData("png")]
    [InlineData("jpeg")]
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

    [Theory]
    [InlineData("WEBP")]
    [InlineData("GIF")]
    [InlineData("BMP")]
    [InlineData("invalid-format")]
    public void Validate_WithInvalidOutputFormat_ShouldThrowArgumentException(string outputFormat)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            OutputFormat = outputFormat
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Validate_WithValidN_ShouldNotThrow(int n)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            N = n
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(100)]
    public void Validate_WithInvalidN_ShouldThrowArgumentException(int n)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            N = n
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Validate_WithValidOutputCompression_ShouldNotThrow(int compression)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            OutputCompression = compression
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(200)]
    public void Validate_WithInvalidOutputCompression_ShouldThrowArgumentException(int compression)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            OutputCompression = compression
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidPrompt_ShouldThrowArgumentException(string prompt)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = prompt
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidModel_ShouldThrowArgumentException(string model)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Model = model,
            Prompt = "Test prompt"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var request = new ImageGenerationRequest();

        // Assert
        request.Model.Should().Be("gpt-image-1");
        request.Prompt.Should().Be(string.Empty);
        request.Size.Should().Be("1024x1024");
        request.N.Should().Be(1);
        request.Quality.Should().Be("high");
        request.OutputFormat.Should().Be("PNG");
        request.OutputCompression.Should().Be(100);
        request.User.Should().BeNull();
    }

    [Fact]
    public void Validate_WithNullN_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            N = null
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void Validate_WithNullOutputCompression_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            OutputCompression = null
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void Validate_WithNullOutputFormat_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            OutputFormat = null
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }
} 