using AzureImage.Inference.Models.DALLE3;
using FluentAssertions;
using Xunit;

namespace AzureImage.Tests.Inference.Tests.Models.Tests.DALLE3.Tests;

public class DALLE3OptionsTests
{
    [Fact]
    public void Validate_WithValidOptions_ShouldNotThrow()
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "dall-e-3-deployment",
            ApiVersion = "2024-02-01",
            ModelName = "dall-e-3"
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
        var options = new DALLE3Options
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
        var options = new DALLE3Options
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
        var options = new DALLE3Options
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
        var options = new DALLE3Options
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
    [InlineData("1792x1024")]
    [InlineData("1024x1792")]
    [InlineData("1024X1024")]
    [InlineData("1792X1024")]
    public void Validate_WithValidDefaultSize_ShouldNotThrow(string size)
    {
        // Arrange
        var options = new DALLE3Options
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
    [InlineData("1024x1536")]
    [InlineData("1536x1536")]
    [InlineData("invalid-size")]
    public void Validate_WithInvalidDefaultSize_ShouldThrowArgumentException(string size)
    {
        // Arrange
        var options = new DALLE3Options
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
    [InlineData("standard")]
    [InlineData("hd")]
    [InlineData("STANDARD")]
    [InlineData("HD")]
    public void Validate_WithValidDefaultQuality_ShouldNotThrow(string quality)
    {
        // Arrange
        var options = new DALLE3Options
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
    [InlineData("low")]
    [InlineData("medium")]
    [InlineData("high")]
    [InlineData("invalid-quality")]
    public void Validate_WithInvalidDefaultQuality_ShouldThrowArgumentException(string quality)
    {
        // Arrange
        var options = new DALLE3Options
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
    [InlineData("natural")]
    [InlineData("vivid")]
    [InlineData("NATURAL")]
    [InlineData("VIVID")]
    public void Validate_WithValidDefaultStyle_ShouldNotThrow(string style)
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultStyle = style
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("realistic")]
    [InlineData("artistic")]
    [InlineData("invalid-style")]
    public void Validate_WithInvalidDefaultStyle_ShouldThrowArgumentException(string style)
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultStyle = style
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Theory]
    [InlineData("url")]
    [InlineData("b64_json")]
    [InlineData("URL")]
    [InlineData("B64_JSON")]
    public void Validate_WithValidDefaultResponseFormat_ShouldNotThrow(string format)
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultResponseFormat = format
        };

        // Act & Assert
        var exception = Record.Exception(() => options.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("json")]
    [InlineData("base64")]
    [InlineData("image")]
    [InlineData("invalid-format")]
    public void Validate_WithInvalidDefaultResponseFormat_ShouldThrowArgumentException(string format)
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            DefaultResponseFormat = format
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Fact]
    public void Validate_WithNegativeTimeout_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            Timeout = TimeSpan.FromSeconds(-1)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Fact]
    public void Validate_WithNegativeMaxRetryAttempts_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            MaxRetryAttempts = -1
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Fact]
    public void Validate_WithNegativeRetryDelay_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new DALLE3Options
        {
            Endpoint = "https://test.openai.azure.com/",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment",
            RetryDelay = TimeSpan.FromSeconds(-1)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => options.Validate());
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var options = new DALLE3Options();

        // Assert
        options.Endpoint.Should().Be(string.Empty);
        options.ApiKey.Should().Be(string.Empty);
        options.DeploymentName.Should().Be(string.Empty);
        options.ApiVersion.Should().Be("2024-02-01");
        options.ModelName.Should().Be("dall-e-3");
        options.DefaultSize.Should().Be("1024x1024");
        options.DefaultQuality.Should().Be("standard");
        options.DefaultStyle.Should().Be("vivid");
        options.DefaultResponseFormat.Should().Be("url");
        options.Timeout.Should().Be(TimeSpan.FromMinutes(5));
        options.MaxRetryAttempts.Should().Be(3);
        options.RetryDelay.Should().Be(TimeSpan.FromSeconds(1));
    }
} 