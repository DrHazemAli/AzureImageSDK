using AzureImage.Inference.Models.DALLE3;
using FluentAssertions;
using Xunit;

namespace AzureImage.Tests.Inference.Tests.Models.Tests.DALLE3.Tests;

public class ImageGenerationRequestTests
{
    [Fact]
    public void Validate_WithValidRequest_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "A beautiful landscape with mountains and a lake",
            Size = "1024x1024",
            Quality = "standard",
            Style = "vivid",
            N = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("1024x1024")]
    [InlineData("1792x1024")]
    [InlineData("1024x1792")]
    [InlineData("1024X1024")]
    [InlineData("1792X1024")]
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
    [InlineData("1024x1536")]
    [InlineData("1536x1024")]
    [InlineData("2048x2048")]
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
    [InlineData("standard")]
    [InlineData("hd")]
    [InlineData("STANDARD")]
    [InlineData("HD")]
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
    [InlineData("low")]
    [InlineData("medium")]
    [InlineData("high")]
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
    [InlineData("natural")]
    [InlineData("vivid")]
    [InlineData("NATURAL")]
    [InlineData("VIVID")]
    public void Validate_WithValidStyle_ShouldNotThrow(string style)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            Style = style
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("realistic")]
    [InlineData("artistic")]
    [InlineData("")]
    [InlineData("invalid-style")]
    public void Validate_WithInvalidStyle_ShouldThrowArgumentException(string style)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            Style = style
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("url")]
    [InlineData("b64_json")]
    [InlineData("URL")]
    [InlineData("B64_JSON")]
    public void Validate_WithValidResponseFormat_ShouldNotThrow(string responseFormat)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            ResponseFormat = responseFormat
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("json")]
    [InlineData("base64")]
    [InlineData("image")]
    [InlineData("invalid-format")]
    public void Validate_WithInvalidResponseFormat_ShouldThrowArgumentException(string responseFormat)
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            ResponseFormat = responseFormat
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Fact]
    public void Validate_WithValidN_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            N = 1
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(-1)]
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

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var request = new ImageGenerationRequest();

        // Assert
        request.Prompt.Should().Be(string.Empty);
        request.Size.Should().Be("1024x1024");
        request.N.Should().Be(1);
        request.Quality.Should().Be("standard");
        request.Style.Should().Be("vivid");
        request.ResponseFormat.Should().Be("url");
    }

    [Fact]
    public void Validate_WithNullResponseFormat_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            ResponseFormat = null
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void Validate_WithEmptyResponseFormat_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "Test prompt",
            ResponseFormat = ""
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void Validate_WithHDQualityAndLandscapeSize_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "A cinematic landscape shot",
            Size = "1792x1024",
            Quality = "hd",
            Style = "natural",
            ResponseFormat = "b64_json"
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void Validate_WithVividStyleAndPortraitSize_ShouldNotThrow()
    {
        // Arrange
        var request = new ImageGenerationRequest
        {
            Prompt = "A vibrant portrait",
            Size = "1024x1792",
            Quality = "standard",
            Style = "vivid",
            ResponseFormat = "url"
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }
} 