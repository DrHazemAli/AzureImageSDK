using System;

namespace AzureImage.Inference.Models.AzureVisionCaptioning
{
    /// <summary>
    /// Configuration options for Azure AI Vision Image Captioning service.
    /// </summary>
    public class AzureVisionCaptioningOptions
    {
        /// <summary>
        /// Gets or sets the Azure AI Vision endpoint URL.
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Azure AI Vision API key for authentication.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the API version to use. Default is "2024-02-01".
        /// </summary>
        public string ApiVersion { get; set; } = "2024-02-01";

        /// <summary>
        /// Gets or sets the deployment name (if using a custom deployment).
        /// </summary>
        public string DeploymentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the request timeout in seconds. Default is 30 seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed requests. Default is 3.
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// Gets or sets whether to enable detailed logging of API requests and responses.
        /// </summary>
        public bool EnableLogging { get; set; } = false;

        /// <summary>
        /// Validates the configuration options.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when required options are missing or invalid</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(Endpoint));

            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(ApiKey));

            if (string.IsNullOrWhiteSpace(ApiVersion))
                throw new ArgumentException("API version cannot be null or empty", nameof(ApiVersion));

            if (TimeoutSeconds <= 0)
                throw new ArgumentException("Timeout must be greater than 0", nameof(TimeoutSeconds));

            if (MaxRetryAttempts < 0)
                throw new ArgumentException("Max retry attempts cannot be negative", nameof(MaxRetryAttempts));

            // Validate endpoint format
            if (!Uri.TryCreate(Endpoint, UriKind.Absolute, out var uri) || 
                (uri.Scheme != "https" && uri.Scheme != "http"))
            {
                throw new ArgumentException("Endpoint must be a valid HTTP or HTTPS URL", nameof(Endpoint));
            }
        }
    }
} 