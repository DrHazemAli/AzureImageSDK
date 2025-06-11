# Configuration Guide

This guide covers all configuration options available in the AzureImage SDK.

## Basic Configuration

### Client Options

```csharp
var options = new AzureImageOptions
{
    Endpoint = "your-endpoint",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-15-preview",
    RetryPolicy = RetryPolicy.Default,
    Timeout = TimeSpan.FromSeconds(30)
};
```

### Configuration Properties

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| Endpoint | string | Azure AI Foundry endpoint URL | Required |
| ApiKey | string | API key for authentication | Required |
| ApiVersion | string | API version to use | "2024-02-15-preview" |
| RetryPolicy | RetryPolicy | Retry policy configuration | Default |
| Timeout | TimeSpan | Request timeout | 30 seconds |

## Advanced Configuration

### Retry Policy

```csharp
var options = new AzureImageOptions
{
    Endpoint = "your-endpoint",
    ApiKey = "your-api-key",
    RetryPolicy = new RetryPolicy
    {
        MaxRetries = 3,
        RetryDelay = TimeSpan.FromSeconds(1),
        MaxDelay = TimeSpan.FromSeconds(10)
    }
};
```

### Custom HttpClient

```csharp
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");

var options = new AzureImageOptions
{
    Endpoint = "your-endpoint",
    ApiKey = "your-api-key",
    HttpClient = httpClient
};
```

## ASP.NET Core Configuration

### Using appsettings.json

```json
{
  "AzureImage": {
    "Endpoint": "your-endpoint",
    "ApiKey": "your-api-key",
    "ApiVersion": "2024-02-15-preview",
    "Timeout": "00:00:30"
  }
}
```

### Service Configuration

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAzureImage(options =>
    {
        options.Endpoint = Configuration["AzureImage:Endpoint"];
        options.ApiKey = Configuration["AzureImage:ApiKey"];
        options.ApiVersion = Configuration["AzureImage:ApiVersion"];
        options.Timeout = TimeSpan.Parse(Configuration["AzureImage:Timeout"]);
    });
}
```

## Environment Variables

The SDK can be configured using environment variables:

```bash
AZURE_IMAGE_ENDPOINT=your-endpoint
AZURE_IMAGE_API_KEY=your-api-key
AZURE_IMAGE_API_VERSION=2024-02-15-preview
AZURE_IMAGE_TIMEOUT=00:00:30
```

## Model-Specific Configuration

### StableImageUltra Configuration

```csharp
var ultraOptions = new StableImageUltraOptions
{
    Endpoint = "your-ultra-endpoint",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-15-preview",
    DefaultSize = new ImageSize(1024, 1024),
    DefaultQuality = ImageQuality.High
};
```

### StableImageCore Configuration

```csharp
var coreOptions = new StableImageCoreOptions
{
    Endpoint = "your-core-endpoint",
    ApiKey = "your-api-key",
    ApiVersion = "2024-02-15-preview",
    DefaultSize = new ImageSize(512, 512),
    DefaultQuality = ImageQuality.Standard
};
```

## Security Considerations

### API Key Management

- Store API keys in secure configuration management systems
- Use environment variables or Azure Key Vault in production
- Rotate API keys regularly
- Use different API keys for different environments

### Network Security

- Use HTTPS for all communications
- Configure appropriate firewall rules
- Use private endpoints when available
- Monitor and log all API access

## Best Practices

1. **Configuration Management**
   - Use different configurations for different environments
   - Never commit sensitive information to source control
   - Use secure configuration management systems

2. **Performance**
   - Configure appropriate timeouts
   - Use retry policies for transient failures
   - Monitor and adjust configuration based on usage patterns

3. **Error Handling**
   - Configure appropriate retry policies
   - Implement proper error handling
   - Log configuration-related issues

## Troubleshooting

Common configuration issues and solutions:

1. **Invalid Endpoint**
   - Verify the endpoint URL format
   - Check network connectivity
   - Ensure the endpoint is accessible

2. **Authentication Failures**
   - Verify API key is correct
   - Check API key permissions
   - Ensure API key is not expired

3. **Timeout Issues**
   - Adjust timeout settings
   - Check network latency
   - Monitor request duration

## Next Steps

- [Quick Start Guide](Quick-Start.md)
- [API Reference](../API-Reference/IAzureImageClient.md)
- [Best Practices](../Best-Practices/Performance.md)
- [Troubleshooting](../Troubleshooting/Common-Issues.md)

## Support

If you need help with configuration:

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples) 