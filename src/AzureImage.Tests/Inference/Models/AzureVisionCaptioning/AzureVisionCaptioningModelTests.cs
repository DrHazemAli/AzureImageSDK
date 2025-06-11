using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureImage.Core.Exceptions;
using AzureImage.Inference.Models;
using AzureImage.Inference.Models.AzureVisionCaptioning;
using Moq;
using Moq.Protected;
using Xunit;

namespace AzureImage.Tests.Inference.Models.AzureVisionCaptioning
{
    public class AzureVisionCaptioningModelTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly AzureVisionCaptioningOptions _options;
        private readonly AzureVisionCaptioningModel _model;

        public AzureVisionCaptioningModelTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            
            _options = new AzureVisionCaptioningOptions
            {
                Endpoint = "https://test-vision.cognitiveservices.azure.com",
                ApiKey = "test-api-key",
                ApiVersion = "2024-02-01"
            };

            _model = new AzureVisionCaptioningModel(_httpClient, _options);
        }

        [Fact]
        public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AzureVisionCaptioningModel(null, _options));
        }

        [Fact]
        public void Constructor_WithNullOptions_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AzureVisionCaptioningModel(_httpClient, null));
        }

        [Fact]
        public void Constructor_WithInvalidOptions_ThrowsArgumentException()
        {
            // Arrange
            var invalidOptions = new AzureVisionCaptioningOptions
            {
                Endpoint = "",
                ApiKey = "test-key"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new AzureVisionCaptioningModel(_httpClient, invalidOptions));
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithImageStream_ReturnsValidResult()
        {
            // Arrange
            var responseJson = @"{
                ""modelVersion"": ""2024-02-01"",
                ""metadata"": {
                    ""width"": 1024,
                    ""height"": 768
                },
                ""captionResult"": {
                    ""text"": ""A beautiful sunset over mountains"",
                    ""confidence"": 0.8925
                }
            }";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            using var imageStream = new MemoryStream(Encoding.UTF8.GetBytes("fake image data"));

            // Act
            var result = await _model.GenerateCaptionAsync(imageStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2024-02-01", result.ModelVersion);
            Assert.NotNull(result.Caption);
            Assert.Equal("A beautiful sunset over mountains", result.Caption.Text);
            Assert.Equal(0.8925, result.Caption.Confidence, 4);
            Assert.NotNull(result.Metadata);
            Assert.Equal(1024, result.Metadata.Width);
            Assert.Equal(768, result.Metadata.Height);
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithImageUrl_ReturnsValidResult()
        {
            // Arrange
            var responseJson = @"{
                ""modelVersion"": ""2024-02-01"",
                ""metadata"": {
                    ""width"": 800,
                    ""height"": 600
                },
                ""captionResult"": {
                    ""text"": ""A cat sitting on a windowsill"",
                    ""confidence"": 0.9156
                }
            }";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            var imageUrl = "https://example.com/image.jpg";

            // Act
            var result = await _model.GenerateCaptionAsync(imageUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2024-02-01", result.ModelVersion);
            Assert.NotNull(result.Caption);
            Assert.Equal("A cat sitting on a windowsill", result.Caption.Text);
            Assert.Equal(0.9156, result.Caption.Confidence, 4);
        }

        [Fact]
        public async Task GenerateDenseCaptionsAsync_WithImageStream_ReturnsValidResult()
        {
            // Arrange
            var responseJson = @"{
                ""modelVersion"": ""2024-02-01"",
                ""metadata"": {
                    ""width"": 1200,
                    ""height"": 800
                },
                ""denseCaptionsResult"": {
                    ""values"": [
                        {
                            ""text"": ""A person standing in a field"",
                            ""confidence"": 0.8734,
                            ""boundingBox"": {
                                ""x"": 100,
                                ""y"": 50,
                                ""w"": 200,
                                ""h"": 300
                            }
                        },
                        {
                            ""text"": ""A blue sky with clouds"",
                            ""confidence"": 0.7892,
                            ""boundingBox"": {
                                ""x"": 0,
                                ""y"": 0,
                                ""w"": 1200,
                                ""h"": 400
                            }
                        }
                    ]
                }
            }";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            using var imageStream = new MemoryStream(Encoding.UTF8.GetBytes("fake image data"));

            // Act
            var result = await _model.GenerateDenseCaptionsAsync(imageStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2024-02-01", result.ModelVersion);
            Assert.NotNull(result.Captions);
            Assert.Equal(2, result.Captions.Count);
            
            var firstCaption = result.Captions[0];
            Assert.Equal("A person standing in a field", firstCaption.Text);
            Assert.Equal(0.8734, firstCaption.Confidence, 4);
            Assert.NotNull(firstCaption.BoundingBox);
            Assert.Equal(100, firstCaption.BoundingBox.X);
            Assert.Equal(50, firstCaption.BoundingBox.Y);
            Assert.Equal(200, firstCaption.BoundingBox.Width);
            Assert.Equal(300, firstCaption.BoundingBox.Height);
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithNullImageStream_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _model.GenerateCaptionAsync((Stream)null));
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithNullImageUrl_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _model.GenerateCaptionAsync((string)null));
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithEmptyImageUrl_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _model.GenerateCaptionAsync(""));
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithHttpError_ThrowsAzureVisionException()
        {
            // Arrange
            var errorResponse = @"{
                ""error"": {
                    ""code"": ""InvalidRequest"",
                    ""message"": ""The provided image URL is not accessible""
                }
            }";

            SetupHttpResponse(HttpStatusCode.BadRequest, errorResponse);

            var imageUrl = "https://invalid-url.com/image.jpg";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AzureVisionException>(() => 
                _model.GenerateCaptionAsync(imageUrl));
            
            Assert.Equal(400, exception.StatusCode);
            Assert.Contains("InvalidRequest", exception.ResponseContent);
        }

        [Fact]
        public async Task GenerateCaptionAsync_WithGenderNeutralOption_SendsCorrectRequest()
        {
            // Arrange
            var responseJson = @"{
                ""modelVersion"": ""2024-02-01"",
                ""captionResult"": {
                    ""text"": ""A person walking in the park"",
                    ""confidence"": 0.85
                }
            }";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            var options = new ImageCaptionOptions
            {
                GenderNeutralCaption = true,
                Language = "es"
            };

            using var imageStream = new MemoryStream(Encoding.UTF8.GetBytes("fake image data"));

            // Act
            await _model.GenerateCaptionAsync(imageStream, options);

            // Assert
            VerifyHttpRequest(request =>
            {
                var requestUri = request.RequestUri.ToString();
                Assert.Contains("gender-neutral-caption=true", requestUri);
                Assert.Contains("language=es", requestUri);
                Assert.Contains("features=caption", requestUri);
            });
        }

        [Fact]
        public async Task GenerateDenseCaptionsAsync_WithOptions_SendsCorrectRequest()
        {
            // Arrange
            var responseJson = @"{
                ""modelVersion"": ""2024-02-01"",
                ""denseCaptionsResult"": {
                    ""values"": []
                }
            }";

            SetupHttpResponse(HttpStatusCode.OK, responseJson);

            var imageUrl = "https://example.com/image.jpg";

            // Act
            await _model.GenerateDenseCaptionsAsync(imageUrl);

            // Assert
            VerifyHttpRequest(request =>
            {
                var requestUri = request.RequestUri.ToString();
                Assert.Contains("features=denseCaptions", requestUri);
                Assert.Contains("api-version=2024-02-01", requestUri);
            });
        }

        private void SetupHttpResponse(HttpStatusCode statusCode, string content)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        private void VerifyHttpRequest(Action<HttpRequestMessage> requestValidator)
        {
            _mockHttpMessageHandler
                .Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req => 
                    {
                        requestValidator(req);
                        return true;
                    }),
                    ItExpr.IsAny<CancellationToken>());
        }
    }
} 