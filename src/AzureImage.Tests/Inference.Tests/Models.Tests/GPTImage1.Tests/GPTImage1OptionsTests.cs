using AzureImage.Inference.Models.GPTImage1;
using FluentAssertions;
using Xunit;

namespace AzureImage.Tests.Inference.Tests.Models.Tests.GPTImage1.Tests;

public class GPTImage1OptionsTests
{
    [Fact]
    public void Validate_WithValidOptions_ShouldNotThrow()
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "gpt-image-1-deployment",
            ApiVersion = "2025-04-01-preview",
            ModelName = "gpt-image-1"
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidEndpoint_ShouldThrowArgumentException(string endpoint)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = endpoint,
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidApiKey_ShouldThrowArgumentException(string apiKey)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = apiKey,
            DeploymentName = "test-deployment"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidDeploymentName_ShouldThrowArgumentException(string deploymentName)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = deploymentName
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("ftp://invalid.com")]
    [InlineData("invalid-url")]
    public void Validate_WithInvalidEndpointFormat_ShouldThrowArgumentException(string endpoint)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = endpoint,
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("1024x1024")]
    [InlineData("1024x1536")]
    [InlineData("1536x1024")]
    public void Validate_WithValidDefaultSize_ShouldNotThrow(string size)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultSize = size
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("512x512")]
    [InlineData("2048x2048")]
    [InlineData("1024x2048")]
    [InlineData("invalid-size")]
    public void Validate_WithInvalidDefaultSize_ShouldThrowArgumentException(string size)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultSize = size
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("low")]
    [InlineData("medium")]
    [InlineData("high")]
    [InlineData("LOW")]
    [InlineData("MEDIUM")]
    [InlineData("HIGH")]
    public void Validate_WithValidDefaultQuality_ShouldNotThrow(string quality)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultQuality = quality
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("ultra")]
    [InlineData("standard")]
    [InlineData("invalid-quality")]
    public void Validate_WithInvalidDefaultQuality_ShouldThrowArgumentException(string quality)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultQuality = quality
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("PNG")]
    [InlineData("JPEG")]
    [InlineData("png")]
    [InlineData("jpeg")]
    public void Validate_WithValidDefaultOutputFormat_ShouldNotThrow(string format)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultOutputFormat = format
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("WEBP")]
    [InlineData("GIF")]
    [InlineData("invalid-format")]
    public void Validate_WithInvalidDefaultOutputFormat_ShouldThrowArgumentException(string format)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultOutputFormat = format
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Validate_WithValidDefaultCompression_ShouldNotThrow(int compression)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultCompression = compression
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(200)]
    public void Validate_WithInvalidDefaultCompression_ShouldThrowArgumentException(int compression)
    {
        // Arrange
        var options = new GPTImage1Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultCompression = compression
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var options = new GPTImage1Options();

        // Assert
        options.Endpoint.Should().Be(string.Empty);
        options.ApiKey.Should().Be(string.Empty);
        options.DeploymentName.Should().Be(string.Empty);
        options.ApiVersion.Should().Be("2025-04-01-preview");
        options.ModelName.Should().Be("gpt-image-1");
        options.DefaultSize.Should().Be("1024x1024");
        options.DefaultQuality.Should().Be("high");
        options.DefaultOutputFormat.Should().Be("PNG");
        options.DefaultCompression.Should().Be(100);
        options.Timeout.Should().Be(TimeSpan.FromMinutes(5));
        options.MaxRetryAttempts.Should().Be(3);
        options.RetryDelay.Should().Be(TimeSpan.FromSeconds(1));
    }
} 