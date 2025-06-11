using System;
using AzureImage.Inference.Models;

namespace AzureImage.Inference.Models.StableImageUltra;

/// <summary>
/// StableImageUltra model implementation for image generation
/// </summary>
public class StableImageUltraModel : IImageGenerationModel
{
    private readonly StableImageUltraOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="StableImageUltraModel"/> class
    /// </summary>
    /// <param name="options">The model configuration options</param>
    public StableImageUltraModel(StableImageUltraOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _options.Validate();
    }

    /// <summary>
    /// Gets the model name/identifier
    /// </summary>
    public string ModelName => _options.ModelName;

    /// <summary>
    /// Gets the endpoint for this model
    /// </summary>
    public string Endpoint => _options.Endpoint;

    /// <summary>
    /// Gets the API version for this model
    /// </summary>
    public string ApiVersion => _options.ApiVersion;

    /// <summary>
    /// Gets the API key for authentication
    /// </summary>
    public string ApiKey => _options.ApiKey;

    /// <summary>
    /// Gets the endpoint path for image generation for this model
    /// </summary>
    public string ImageGenerationEndpoint => "images/generations";

    /// <summary>
    /// Gets the timeout for requests to this model
    /// </summary>
    public TimeSpan Timeout => _options.Timeout;

    /// <summary>
    /// Gets the maximum retry attempts for this model
    /// </summary>
    public int MaxRetryAttempts => _options.MaxRetryAttempts;

    /// <summary>
    /// Gets the retry delay for this model
    /// </summary>
    public TimeSpan RetryDelay => _options.RetryDelay;

    /// <summary>
    /// Gets the default size for this model
    /// </summary>
    public string DefaultSize => _options.DefaultSize;

    /// <summary>
    /// Gets the default output format for this model
    /// </summary>
    public string DefaultOutputFormat => _options.DefaultOutputFormat;

    /// <summary>
    /// Creates a new StableImageUltraModel with the specified configuration
    /// </summary>
    /// <param name="endpoint">The model endpoint</param>
    /// <param name="apiKey">The API key</param>
    /// <param name="configure">Optional configuration action</param>
    /// <returns>A new StableImageUltraModel instance</returns>
    public static StableImageUltraModel Create(string endpoint, string apiKey, Action<StableImageUltraOptions>? configure = null)
    {
        var options = new StableImageUltraOptions
        {
            Endpoint = endpoint,
            ApiKey = apiKey
        };

        configure?.Invoke(options);

        return new StableImageUltraModel(options);
    }
} 