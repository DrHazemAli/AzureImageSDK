using System;
using AzureImage.Inference.Models;

namespace AzureImage.Inference.Models.StableImageCore;

/// <summary>
/// StableImageCore model implementation for image generation
/// </summary>
public class StableImageCoreModel : IImageGenerationModel
{
    private readonly StableImageCoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="StableImageCoreModel"/> class
    /// </summary>
    /// <param name="options">The model configuration options</param>
    public StableImageCoreModel(StableImageCoreOptions options)
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
    /// Creates a new StableImageCoreModel with the specified configuration
    /// </summary>
    /// <param name="endpoint">The model endpoint</param>
    /// <param name="apiKey">The API key</param>
    /// <param name="configure">Optional configuration action</param>
    /// <returns>A new StableImageCoreModel instance</returns>
    public static StableImageCoreModel Create(string endpoint, string apiKey, Action<StableImageCoreOptions>? configure = null)
    {
        var options = new StableImageCoreOptions
        {
            Endpoint = endpoint,
            ApiKey = apiKey
        };

        configure?.Invoke(options);

        return new StableImageCoreModel(options);
    }
} 