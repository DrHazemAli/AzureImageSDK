using System;
using AzureImage.Core;
using AzureImage.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AzureImage.Extensions;

/// <summary>
/// Extension methods for registering Azure Image SDK services with dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Azure Image SDK services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAzureImage(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Register the model HTTP client service
        services.AddSingleton<ModelHttpClientService>(provider =>
        {
            var loggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger(typeof(ModelHttpClientService).FullName ?? nameof(ModelHttpClientService));
            return new ModelHttpClientService(logger: new Microsoft.Extensions.Logging.Abstractions.NullLogger<ModelHttpClientService>());
        });

        // Register main client
        services.AddSingleton<IAzureImageClient>(provider =>
        {
            var loggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
            return new AzureImageClient(loggerFactory);
        });

        return services;
    }
} 