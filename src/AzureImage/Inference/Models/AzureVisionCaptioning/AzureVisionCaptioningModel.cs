using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AzureImage.Core.Exceptions;

namespace AzureImage.Inference.Models.AzureVisionCaptioning
{
    /// <summary>
    /// Azure AI Vision implementation of image captioning using Image Analysis 4.0 API.
    /// </summary>
    public class AzureVisionCaptioningModel : IImageCaptioningModel
    {
        private readonly HttpClient _httpClient;
        private readonly AzureVisionCaptioningOptions _options;

        /// <summary>
        /// Initializes a new instance of the AzureVisionCaptioningModel.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests</param>
        /// <param name="options">The Azure Vision captioning configuration options</param>
        public AzureVisionCaptioningModel(HttpClient httpClient, AzureVisionCaptioningOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            ValidateOptions();
        }

        /// <summary>
        /// Generates a caption for the provided image stream.
        /// </summary>
        public async Task<ImageCaptionResult> GenerateCaptionAsync(
            Stream imageStream, 
            ImageCaptionOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream));

            var requestOptions = options ?? new ImageCaptionOptions();
            
            using var content = new StreamContent(imageStream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var response = await SendAnalysisRequestAsync(content, "caption", requestOptions, cancellationToken);
            
            return ParseCaptionResponse(response);
        }

        /// <summary>
        /// Generates a caption for the provided image URL.
        /// </summary>
        public async Task<ImageCaptionResult> GenerateCaptionAsync(
            string imageUrl, 
            ImageCaptionOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));

            var requestOptions = options ?? new ImageCaptionOptions();
            
            var requestBody = new { url = imageUrl };
            var jsonContent = JsonSerializer.Serialize(requestBody);
            
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await SendAnalysisRequestAsync(content, "caption", requestOptions, cancellationToken);
            
            return ParseCaptionResponse(response);
        }

        /// <summary>
        /// Generates dense captions for multiple regions in the provided image stream.
        /// </summary>
        public async Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
            Stream imageStream, 
            ImageCaptionOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream));

            var requestOptions = options ?? new ImageCaptionOptions();
            
            using var content = new StreamContent(imageStream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var response = await SendAnalysisRequestAsync(content, "denseCaptions", requestOptions, cancellationToken);
            
            return ParseDenseCaptionResponse(response);
        }

        /// <summary>
        /// Generates dense captions for multiple regions in the provided image URL.
        /// </summary>
        public async Task<DenseCaptionResult> GenerateDenseCaptionsAsync(
            string imageUrl, 
            ImageCaptionOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));

            var requestOptions = options ?? new ImageCaptionOptions();
            
            var requestBody = new { url = imageUrl };
            var jsonContent = JsonSerializer.Serialize(requestBody);
            
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await SendAnalysisRequestAsync(content, "denseCaptions", requestOptions, cancellationToken);
            
            return ParseDenseCaptionResponse(response);
        }

        private async Task<string> SendAnalysisRequestAsync(
            HttpContent content, 
            string features, 
            ImageCaptionOptions options, 
            CancellationToken cancellationToken)
        {
            var queryParams = new List<string>
            {
                $"api-version={_options.ApiVersion}",
                $"features={features}",
                $"language={options.Language}"
            };

            if (options.GenderNeutralCaption)
            {
                queryParams.Add("gender-neutral-caption=true");
            }

            var url = $"{_options.Endpoint.TrimEnd('/')}/computervision/imageanalysis:analyze?{string.Join("&", queryParams)}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            request.Headers.Add("Ocp-Apim-Subscription-Key", _options.ApiKey);

            try
            {
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new AzureVisionException(
                        $"Azure Vision API request failed with status {response.StatusCode}: {responseContent}",
                        (int)response.StatusCode,
                        responseContent);
                }

                return responseContent;
            }
            catch (HttpRequestException ex)
            {
                throw new AzureVisionException("Failed to send request to Azure Vision API", ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new AzureVisionException("Request to Azure Vision API timed out", ex);
            }
        }

        private ImageCaptionResult ParseCaptionResponse(string jsonResponse)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;

                var result = new ImageCaptionResult
                {
                    ModelVersion = root.GetProperty("modelVersion").GetString()
                };

                // Parse metadata
                if (root.TryGetProperty("metadata", out var metadataElement))
                {
                    result.Metadata = new ImageMetadata
                    {
                        Width = metadataElement.GetProperty("width").GetInt32(),
                        Height = metadataElement.GetProperty("height").GetInt32()
                    };
                }

                // Parse caption
                if (root.TryGetProperty("captionResult", out var captionElement))
                {
                    result.Caption = new Caption
                    {
                        Text = captionElement.GetProperty("text").GetString(),
                        Confidence = captionElement.GetProperty("confidence").GetDouble()
                    };
                }

                return result;
            }
            catch (JsonException ex)
            {
                throw new AzureVisionException("Failed to parse Azure Vision API response", ex);
            }
        }

        private DenseCaptionResult ParseDenseCaptionResponse(string jsonResponse)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;

                var result = new DenseCaptionResult
                {
                    ModelVersion = root.GetProperty("modelVersion").GetString(),
                    Captions = new List<DenseCaption>()
                };

                // Parse metadata
                if (root.TryGetProperty("metadata", out var metadataElement))
                {
                    result.Metadata = new ImageMetadata
                    {
                        Width = metadataElement.GetProperty("width").GetInt32(),
                        Height = metadataElement.GetProperty("height").GetInt32()
                    };
                }

                // Parse dense captions
                if (root.TryGetProperty("denseCaptionsResult", out var denseCaptionsElement) &&
                    denseCaptionsElement.TryGetProperty("values", out var valuesElement))
                {
                    foreach (var captionElement in valuesElement.EnumerateArray())
                    {
                        var denseCaption = new DenseCaption
                        {
                            Text = captionElement.GetProperty("text").GetString(),
                            Confidence = captionElement.GetProperty("confidence").GetDouble()
                        };

                        // Parse bounding box
                        if (captionElement.TryGetProperty("boundingBox", out var boundingBoxElement))
                        {
                            denseCaption.BoundingBox = new BoundingBox
                            {
                                X = boundingBoxElement.GetProperty("x").GetInt32(),
                                Y = boundingBoxElement.GetProperty("y").GetInt32(),
                                Width = boundingBoxElement.GetProperty("w").GetInt32(),
                                Height = boundingBoxElement.GetProperty("h").GetInt32()
                            };
                        }

                        result.Captions.Add(denseCaption);
                    }
                }

                return result;
            }
            catch (JsonException ex)
            {
                throw new AzureVisionException("Failed to parse Azure Vision API response", ex);
            }
        }

        private void ValidateOptions()
        {
            if (string.IsNullOrWhiteSpace(_options.Endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(_options.Endpoint));

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(_options.ApiKey));

            if (string.IsNullOrWhiteSpace(_options.ApiVersion))
                throw new ArgumentException("API version cannot be null or empty", nameof(_options.ApiVersion));
        }
    }
} 