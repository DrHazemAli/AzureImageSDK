using System;
using AzureImage.Inference.Models;

namespace AzureImage.Inference.Models.DALLE3;

/// <summary>
/// DALL-E 3 model implementation for image generation
/// Note: DALL-E 3 does not support image editing
/// </summary>
public class DALLE3Model : IImageGenerationModel
{
    private readonly DALLE3Options _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DALLE3Model"/> class
    /// </summary>
    /// <param name="options">The model configuration options</param>
    public DALLE3Model(DALLE3Options options)
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
    /// Gets the deployment name for this model
    /// </summary>
    public string DeploymentName => _options.DeploymentName;

    /// <summary>
    /// Gets the endpoint path for image generation for this model
    /// </summary>
    public string ImageGenerationEndpoint => $"openai/deployments/{_options.DeploymentName}/images/generations";

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
    /// Gets the default quality for this model
    /// </summary>
    public string DefaultQuality => _options.DefaultQuality;

    /// <summary>
    /// Gets the default style for this model
    /// </summary>
    public string DefaultStyle => _options.DefaultStyle;

    /// <summary>
    /// Gets the default response format for this model
    /// </summary>
    public string DefaultResponseFormat => _options.DefaultResponseFormat;

    /// <summary>
    /// Creates a new DALLE3Model with the specified configuration
    /// </summary>
    /// <param name="endpoint">The model endpoint</param>
    /// <param name="apiKey">The API key</param>
    /// <param name="deploymentName">The deployment name</param>
    /// <param name="configure">Optional configuration action</param>
    /// <returns>A new DALLE3Model instance</returns>
    public static DALLE3Model Create(string endpoint, string apiKey, string deploymentName, Action<DALLE3Options>? configure = null)
    {
        var options = new DALLE3Options
        {
            Endpoint = endpoint,
            ApiKey = apiKey,
            DeploymentName = deploymentName
        };

        configure?.Invoke(options);

        return new DALLE3Model(options);
    }
} 