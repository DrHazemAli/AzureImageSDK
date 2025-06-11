using AzureImage.Inference.Models.GPTImage1;
using FluentAssertions;
using Xunit;

namespace AzureImage.Tests.Inference.Tests.Models.Tests.GPTImage1.Tests;

public class ImageEditingRequestTests
{
    [Fact]
    public void Validate_WithValidRequest_ShouldNotThrow()
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header
        var request = new ImageEditingRequest
        {
            Model = "gpt-image-1",
            Prompt = "Add a blue sky to this image",
            Image = imageBytes,
            ImageFileName = "test.png",
            Size = "1024x1024",
            Quality = "high"
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Fact]
    public void Validate_WithValidRequestAndMask_ShouldNotThrow()
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header
        var maskBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header
        var request = new ImageEditingRequest
        {
            Model = "gpt-image-1",
            Prompt = "Add a blue sky to this image",
            Image = imageBytes,
            ImageFileName = "test.png",
            Mask = maskBytes,
            MaskFileName = "mask.png",
            Size = "1024x1024",
            Quality = "high"
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidPrompt_ShouldThrowArgumentException(string prompt)
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = prompt,
            Image = imageBytes,
            ImageFileName = "test.png"
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
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Model = model,
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = "test.png"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Fact]
    public void Validate_WithNullImage_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = null,
            ImageFileName = "test.png"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Fact]
    public void Validate_WithEmptyImage_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = Array.Empty<byte>(),
            ImageFileName = "test.png"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WithInvalidImageFileName_ShouldThrowArgumentException(string fileName)
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = fileName
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Fact]
    public void Validate_WithMaskButNoMaskFileName_ShouldThrowArgumentException()
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var maskBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = "test.png",
            Mask = maskBytes,
            MaskFileName = null
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Theory]
    [InlineData("1024x1024")]
    [InlineData("1024x1536")]
    [InlineData("1536x1024")]
    public void Validate_WithValidSize_ShouldNotThrow(string size)
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = "test.png",
            Size = size
        };

        // Act & Assert
        var exception = Record.Exception(() => request.Validate());
        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("512x512")]
    [InlineData("2048x2048")]
    [InlineData("invalid-size")]
    public void Validate_WithInvalidSize_ShouldThrowArgumentException(string size)
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = "test.png",
            Size = size
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
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = "test.png",
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
    public void Validate_WithInvalidN_ShouldThrowArgumentException(int n)
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var request = new ImageEditingRequest
        {
            Prompt = "Test prompt",
            Image = imageBytes,
            ImageFileName = "test.png",
            N = n
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.Validate());
    }

    [Fact]
    public async Task FromFileAsync_WithValidFile_ShouldCreateRequest()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        await File.WriteAllBytesAsync(tempFile, imageBytes);

        try
        {
            // Act
            var request = await ImageEditingRequest.FromFileAsync(tempFile, "Test prompt");

            // Assert
            request.Image.Should().BeEquivalentTo(imageBytes);
            request.Prompt.Should().Be("Test prompt");
            request.ImageFileName.Should().Be(Path.GetFileName(tempFile));
            request.Mask.Should().BeNull();
            request.MaskFileName.Should().BeNull();
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task FromFileAsync_WithValidFileAndMask_ShouldCreateRequest()
    {
        // Arrange
        var tempImageFile = Path.GetTempFileName();
        var tempMaskFile = Path.GetTempFileName();
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var maskBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x01 };
        await File.WriteAllBytesAsync(tempImageFile, imageBytes);
        await File.WriteAllBytesAsync(tempMaskFile, maskBytes);

        try
        {
            // Act
            var request = await ImageEditingRequest.FromFileAsync(tempImageFile, "Test prompt", tempMaskFile);

            // Assert
            request.Image.Should().BeEquivalentTo(imageBytes);
            request.Mask.Should().BeEquivalentTo(maskBytes);
            request.Prompt.Should().Be("Test prompt");
            request.ImageFileName.Should().Be(Path.GetFileName(tempImageFile));
            request.MaskFileName.Should().Be(Path.GetFileName(tempMaskFile));
        }
        finally
        {
            File.Delete(tempImageFile);
            File.Delete(tempMaskFile);
        }
    }

    [Fact]
    public void FromBytes_WithValidData_ShouldCreateRequest()
    {
        // Arrange
        var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var maskBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x01 };

        // Act
        var request = ImageEditingRequest.FromBytes(imageBytes, "Test prompt", "test.png", maskBytes, "mask.png");

        // Assert
        request.Image.Should().BeEquivalentTo(imageBytes);
        request.Mask.Should().BeEquivalentTo(maskBytes);
        request.Prompt.Should().Be("Test prompt");
        request.ImageFileName.Should().Be("test.png");
        request.MaskFileName.Should().Be("mask.png");
    }

    [Fact]
    public void FromBytes_WithNullImageBytes_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            ImageEditingRequest.FromBytes(null, "Test prompt"));
    }

    [Fact]
    public void FromBytes_WithEmptyImageBytes_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            ImageEditingRequest.FromBytes(Array.Empty<byte>(), "Test prompt"));
    }

    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var request = new ImageEditingRequest();

        // Assert
        request.Model.Should().Be("gpt-image-1");
        request.Prompt.Should().Be(string.Empty);
        request.Image.Should().BeEquivalentTo(Array.Empty<byte>());
        request.ImageFileName.Should().Be("image.png");
        request.Mask.Should().BeNull();
        request.MaskFileName.Should().Be("mask.png");
        request.Size.Should().Be("1024x1024");
        request.N.Should().Be(1);
        request.Quality.Should().Be("high");
        request.OutputFormat.Should().Be("PNG");
        request.OutputCompression.Should().Be(100);
        request.User.Should().BeNull();
    }
} 