using System;
using AzureImage.Inference.Models;

namespace AzureImage.Inference.Models.GPTImage1;

/// <summary>
/// GPT-Image-1 model implementation for image generation and editing
/// </summary>
public class GPTImage1Model : IImageGenerationModel, IImageEditingModel
{
    private readonly GPTImage1Options _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="GPTImage1Model"/> class
    /// </summary>
    /// <param name="options">The model configuration options</param>
    public GPTImage1Model(GPTImage1Options options)
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
    /// Gets the endpoint path for image editing for this model
    /// </summary>
    public string ImageEditingEndpoint => $"openai/deployments/{_options.DeploymentName}/images/edits";

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
    /// Gets the default output format for this model
    /// </summary>
    public string DefaultOutputFormat => _options.DefaultOutputFormat;

    /// <summary>
    /// Gets the default compression level for this model
    /// </summary>
    public int DefaultCompression => _options.DefaultCompression;

    /// <summary>
    /// Creates a new GPTImage1Model with the specified configuration
    /// </summary>
    /// <param name="endpoint">The model endpoint</param>
    /// <param name="apiKey">The API key</param>
    /// <param name="deploymentName">The deployment name</param>
    /// <param name="configure">Optional configuration action</param>
    /// <returns>A new GPTImage1Model instance</returns>
    public static GPTImage1Model Create(string endpoint, string apiKey, string deploymentName, Action<GPTImage1Options>? configure = null)
    {
        var options = new GPTImage1Options
        {
            Endpoint = endpoint,
            ApiKey = apiKey,
            DeploymentName = deploymentName
        };

        configure?.Invoke(options);

        return new GPTImage1Model(options);
    }
} 