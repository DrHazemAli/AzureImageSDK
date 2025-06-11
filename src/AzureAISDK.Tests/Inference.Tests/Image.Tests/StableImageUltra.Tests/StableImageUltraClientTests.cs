using AzureAISDK.Core;
using AzureAISDK.Core.Services;
using AzureAISDK.Inference.Image.StableImageUltra;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AzureAISDK.Tests.Inference.Tests.Image.Tests.StableImageUltra.Tests;

public class StableImageUltraModelTests
{
    [Fact]
    public void Constructor_WithValidOptions_ShouldCreateModel()
    {
        // Arrange
        var options = new StableImageUltraOptions
        {
            Endpoint = "https://test.endpoint.com",
            ApiKey = "test-key",
            ModelName = "Test-Model"
        };

        // Act
        var model = new StableImageUltraModel(options);

        // Assert
        model.Should().NotBeNull();
        model.ModelName.Should().Be("Test-Model");
        model.Endpoint.Should().Be("https://test.endpoint.com");
        model.ApiKey.Should().Be("test-key");
        model.ApiVersion.Should().Be("2024-05-01-preview");
        model.ImageGenerationEndpoint.Should().Be("images/generations");
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new StableImageUltraModel(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_WithInvalidEndpoint_ShouldThrowArgumentException(string endpoint)
    {
        // Arrange
        var options = new StableImageUltraOptions
        {
            Endpoint = endpoint,
            ApiKey = "test-key"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new StableImageUltraModel(options));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_WithInvalidApiKey_ShouldThrowArgumentException(string apiKey)
    {
        // Arrange
        var options = new StableImageUltraOptions
        {
            Endpoint = "https://test.endpoint.com",
            ApiKey = apiKey
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new StableImageUltraModel(options));
    }

    [Fact]
    public void Create_WithBasicParameters_ShouldCreateModel()
    {
        // Arrange & Act
        var model = StableImageUltraModel.Create(
            "https://test.endpoint.com", 
            "test-key");

        // Assert
        model.Should().NotBeNull();
        model.Endpoint.Should().Be("https://test.endpoint.com");
        model.ApiKey.Should().Be("test-key");
        model.ModelName.Should().Be("Stable-Image-Ultra");
    }

    [Fact]
    public void Create_WithConfiguration_ShouldApplyConfiguration()
    {
        // Arrange & Act
        var model = StableImageUltraModel.Create(
            "https://test.endpoint.com", 
            "test-key",
            options =>
            {
                options.ModelName = "Custom-Model";
                options.DefaultSize = "512x512";
                options.DefaultOutputFormat = "jpg";
                options.Timeout = TimeSpan.FromMinutes(10);
            });

        // Assert
        model.Should().NotBeNull();
        model.ModelName.Should().Be("Custom-Model");
        model.DefaultSize.Should().Be("512x512");
        model.DefaultOutputFormat.Should().Be("jpg");
        model.Timeout.Should().Be(TimeSpan.FromMinutes(10));
    }
}

public class AzureAIClientWithModelTests
{
    private readonly Mock<ILogger<AzureAIClient>> _mockLogger;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public AzureAIClientWithModelTests()
    {
        _mockLogger = new Mock<ILogger<AzureAIClient>>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLoggerFactory.Setup(x => x.CreateLogger<AzureAIClient>()).Returns(_mockLogger.Object);
    }

    [Fact]
    public void Create_WithLoggerFactory_ShouldCreateClient()
    {
        // Act
        using var client = AzureAIClient.Create(_mockLoggerFactory.Object);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void Create_WithoutLoggerFactory_ShouldCreateClient()
    {
        // Act
        using var client = AzureAIClient.Create();

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public async Task GenerateImageAsync_WithNullModel_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var client = AzureAIClient.Create();
        var request = new ImageGenerationRequest { Prompt = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(null!, request));
    }

    [Fact]
    public async Task GenerateImageAsync_WithNullRequest_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var client = AzureAIClient.Create();
        var model = StableImageUltraModel.Create("https://test.endpoint.com", "test-key");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, (ImageGenerationRequest)null!));
    }
} 