using AzureImage.Core;
using AzureImage.Inference.Models.StableImageUltra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SampleProject;

class Program
{
    static async Task Main(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Setup dependency injection
        var services = new ServiceCollection();
        
        // Add logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        var serviceProvider = services.BuildServiceProvider();

        try
        {
            Console.WriteLine("🎨 Azure AI SDK Sample - Model-Based Architecture");
            Console.WriteLine("================================================");

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            // Example 1: Create StableImageUltra model and generate image
            await GenerateWithStableImageUltra(configuration, loggerFactory);

            // Example 2: Advanced configuration with custom options
            await GenerateWithAdvancedOptions(configuration, loggerFactory);

            // Example 3: Multiple models with different configurations
            await GenerateWithMultipleConfigurations(configuration, loggerFactory);

            Console.WriteLine("\n✅ All examples completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner error: {ex.InnerException.Message}");
            }
        }
        finally
        {
            serviceProvider.Dispose();
        }
    }

    static async Task GenerateWithStableImageUltra(IConfiguration configuration, ILoggerFactory? loggerFactory)
    {
        Console.WriteLine("\n🖼️  Example 1: StableImageUltra Basic Usage");
        Console.WriteLine("------------------------------------------");

        try
        {
            // Create the Azure AI client
            using var client = AzureImageClient.Create(loggerFactory);

            // Create StableImageUltra model with configuration
            var model = StableImageUltraModel.Create(
                endpoint: configuration["AzureImage:StableImageUltra:Endpoint"]!,
                apiKey: configuration["AzureImage:StableImageUltra:ApiKey"]!,
                options =>
                {
                    options.ApiVersion = "2024-05-01-preview";
                    options.DefaultSize = "1024x1024";
                    options.DefaultOutputFormat = "png";
                    options.Timeout = TimeSpan.FromMinutes(3);
                });

            // Create model-specific request
            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = "A serene mountain landscape at sunset with a crystal clear lake reflecting the colors of the sky",
                Size = model.DefaultSize,
                OutputFormat = model.DefaultOutputFormat
            };

            Console.WriteLine($"📝 Model: {model.ModelName}");
            Console.WriteLine($"🔗 Endpoint: {model.Endpoint}");
            Console.WriteLine($"📝 Prompt: {request.Prompt}");
            Console.WriteLine("🔄 Generating image...");

            var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, request);

            Console.WriteLine("✅ Image generated successfully!");
            Console.WriteLine($"📊 Image size: {response.Image.Length} base64 characters");

            // Save the image
            var fileName = $"stable_image_ultra_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            await response.SaveImageAsync(fileName);
            Console.WriteLine($"💾 Image saved as: {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to generate with StableImageUltra: {ex.Message}");
        }
    }

    static async Task GenerateWithAdvancedOptions(IConfiguration configuration, ILoggerFactory? loggerFactory)
    {
        Console.WriteLine("\n🎯 Example 2: Advanced StableImageUltra Configuration");
        Console.WriteLine("---------------------------------------------------");

        try
        {
            using var client = AzureImageClient.Create(loggerFactory);

            // Create model with advanced configuration
            var model = StableImageUltraModel.Create(
                endpoint: configuration["AzureImage:StableImageUltra:Endpoint"]!,
                apiKey: configuration["AzureImage:StableImageUltra:ApiKey"]!,
                options =>
                {
                    options.ModelName = "Stable-Image-Ultra-v2"; // Custom model version
                    options.ApiVersion = "2024-05-01-preview";
                    options.DefaultSize = "1024x1024";
                    options.DefaultOutputFormat = "webp";
                    options.Timeout = TimeSpan.FromMinutes(5);
                    options.MaxRetryAttempts = 5;
                    options.RetryDelay = TimeSpan.FromSeconds(2);
                });

            // Create advanced request with StableImageUltra-specific parameters
            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = "A futuristic cyberpunk cityscape with neon lights, flying cars, and holographic advertisements in a rainy night",
                NegativePrompt = "blurry, low quality, distorted, ugly, bad anatomy, watermark",
                Size = "1024x1024",
                OutputFormat = "png",
                Seed = 42 // For reproducible results
            };

            Console.WriteLine($"📝 Model: {model.ModelName}");
            Console.WriteLine($"📝 Prompt: {request.Prompt}");
            Console.WriteLine($"🚫 Negative prompt: {request.NegativePrompt}");
            Console.WriteLine($"📐 Size: {request.Size}");
            Console.WriteLine($"🎨 Format: {request.OutputFormat}");
            Console.WriteLine($"🌱 Seed: {request.Seed}");
            Console.WriteLine("🔄 Generating image...");

            var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, request);

            Console.WriteLine("✅ Image generated successfully!");
            
            if (response.Metadata != null)
            {
                Console.WriteLine($"📊 Metadata:");
                Console.WriteLine($"   - Width: {response.Metadata.Width}");
                Console.WriteLine($"   - Height: {response.Metadata.Height}");
                Console.WriteLine($"   - Format: {response.Metadata.Format}");
                Console.WriteLine($"   - Model: {response.Metadata.Model}");
                Console.WriteLine($"   - Seed: {response.Metadata.Seed}");
            }

            var fileName = $"advanced_stable_image_ultra_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            await response.SaveImageAsync(fileName);
            Console.WriteLine($"💾 Image saved as: {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to generate with advanced options: {ex.Message}");
        }
    }

    static async Task GenerateWithMultipleConfigurations(IConfiguration configuration, ILoggerFactory? loggerFactory)
    {
        Console.WriteLine("\n🔧 Example 3: Multiple Model Configurations");
        Console.WriteLine("------------------------------------------");

        try
        {
            using var client = AzureImageClient.Create(loggerFactory);

            // Model 1: High quality, slower
            var highQualityModel = StableImageUltraModel.Create(
                endpoint: configuration["AzureImage:StableImageUltra:Endpoint"]!,
                apiKey: configuration["AzureImage:StableImageUltra:ApiKey"]!,
                options =>
                {
                    options.ModelName = "Stable-Image-Ultra-HQ";
                    options.DefaultSize = "1024x1024";
                    options.DefaultOutputFormat = "png";
                    options.Timeout = TimeSpan.FromMinutes(10);
                    options.MaxRetryAttempts = 3;
                });

            // Model 2: Fast generation, lower quality
            var fastModel = StableImageUltraModel.Create(
                endpoint: configuration["AzureImage:StableImageUltra:Endpoint"]!,
                apiKey: configuration["AzureImage:StableImageUltra:ApiKey"]!,
                options =>
                {
                    options.ModelName = "Stable-Image-Ultra-Fast";
                    options.DefaultSize = "512x512";
                    options.DefaultOutputFormat = "jpg";
                    options.Timeout = TimeSpan.FromMinutes(2);
                    options.MaxRetryAttempts = 2;
                });

            var prompt = "A peaceful zen garden with cherry blossoms, stone lanterns, and a small pond";

            // Generate with high quality model
            Console.WriteLine("🎨 Generating with High Quality model...");
            var hqRequest = new ImageGenerationRequest
            {
                Model = highQualityModel.ModelName,
                Prompt = prompt,
                Size = highQualityModel.DefaultSize,
                OutputFormat = highQualityModel.DefaultOutputFormat
            };

            var hqResponse = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                highQualityModel, hqRequest);

            var hqFileName = $"hq_zen_garden_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            await hqResponse.SaveImageAsync(hqFileName);
            Console.WriteLine($"💾 High quality image saved as: {hqFileName}");

            // Generate with fast model
            Console.WriteLine("⚡ Generating with Fast model...");
            var fastRequest = new ImageGenerationRequest
            {
                Model = fastModel.ModelName,
                Prompt = prompt,
                Size = fastModel.DefaultSize,
                OutputFormat = fastModel.DefaultOutputFormat
            };

            var fastResponse = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                fastModel, fastRequest);

            var fastFileName = $"fast_zen_garden_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            await fastResponse.SaveImageAsync(fastFileName);
            Console.WriteLine($"💾 Fast image saved as: {fastFileName}");

            Console.WriteLine("✅ Both images generated successfully with different model configurations!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed with multiple configurations: {ex.Message}");
        }
    }
} 