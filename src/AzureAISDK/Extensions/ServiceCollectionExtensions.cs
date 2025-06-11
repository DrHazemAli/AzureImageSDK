using AzureAISDK.Core;
using AzureAISDK.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AzureAISDK.Extensions;

/// <summary>
/// Extension methods for registering Azure AI SDK services with dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Azure AI SDK services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAzureAI(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Register the model HTTP client service
        services.AddSingleton<ModelHttpClientService>(provider =>
        {
            var loggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger<ModelHttpClientService>();
            return new ModelHttpClientService(logger: logger);
        });

        // Register main client
        services.AddSingleton<IAzureAIClient>(provider =>
        {
            var loggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
            return new AzureAIClient(loggerFactory);
        });

        return services;
    }
} 