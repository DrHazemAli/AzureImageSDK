using AzureImage.Inference.Models.StableImageCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace StableImageCore.Sample;

class Program
{
    private static StableImageCoreOptions? _options;

    static async Task Main(string[] args)
    {
        Console.WriteLine("StableImageCore Sample Application");
        Console.WriteLine("===================================");

        try
        {
            // Load configuration
            await LoadConfigurationAsync();

            if (_options == null)
            {
                Console.WriteLine("Failed to load configuration. Please check appsettings.json");
                return;
            }

            // Validate configuration
            _options.Validate();

            // Create model instance
            var model = new StableImageCoreModel(_options);

            Console.WriteLine($"Initialized StableImageCore model:");
            Console.WriteLine($"- Endpoint: {model.Endpoint}");
            Console.WriteLine($"- Model: {model.ModelName}");
            Console.WriteLine($"- API Version: {model.ApiVersion}");
            Console.WriteLine();

            // Run various examples
            await RunBasicImageGenerationExample(model);
            await RunAdvancedConfigurationExample();
            await RunNegativePromptExample(model);
            await RunSeedBasedGenerationExample(model);
            await RunDifferentSizesExample(model);
            await RunErrorHandlingExample();

            Console.WriteLine("All examples completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task LoadConfigurationAsync()
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var configSection = configuration.GetSection("StableImageCore");
            
            _options = new StableImageCoreOptions
            {
                Endpoint = configSection["Endpoint"] ?? string.Empty,
                ApiKey = configSection["ApiKey"] ?? string.Empty,
                ApiVersion = configSection["ApiVersion"] ?? "2024-05-01-preview",
                ModelName = configSection["ModelName"] ?? "Stable-Image-Core"
            };

            Console.WriteLine("Configuration loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load configuration: {ex.Message}");
            throw;
        }
    }

    static async Task RunBasicImageGenerationExample(StableImageCoreModel model)
    {
        Console.WriteLine("=== Basic Image Generation Example ===");
        
        try
        {
            var request = new ImageGenerationRequest
            {
                Prompt = "A serene mountain landscape with a crystal clear lake reflecting the snow-capped peaks at sunset",
                Size = "1024x1024",
                OutputFormat = "png"
            };

            Console.WriteLine($"Generating image with prompt: '{request.Prompt}'");
            Console.WriteLine($"Size: {request.Size}, Format: {request.OutputFormat}");

            // Note: This would be used with an HTTP client in a real implementation
            ValidateRequest(request);
            
            Console.WriteLine("✓ Request validation passed");
            Console.WriteLine("  (In a real implementation, this would make an API call)");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Basic example failed: {ex.Message}");
        }
    }

    static async Task RunAdvancedConfigurationExample()
    {
        Console.WriteLine("=== Advanced Configuration Example ===");
        
        try
        {
            var model = StableImageCoreModel.Create(
                endpoint: _options!.Endpoint,
                apiKey: _options.ApiKey,
                configure: options =>
                {
                    options.Timeout = TimeSpan.FromMinutes(10);
                    options.MaxRetryAttempts = 5;
                    options.RetryDelay = TimeSpan.FromSeconds(2);
                    options.DefaultSize = "1920x1080";
                    options.DefaultOutputFormat = "jpeg";
                });

            Console.WriteLine("Advanced model configuration:");
            Console.WriteLine($"- Timeout: {model.Timeout}");
            Console.WriteLine($"- Max Retries: {model.MaxRetryAttempts}");
            Console.WriteLine($"- Retry Delay: {model.RetryDelay}");
            Console.WriteLine($"- Default Size: {model.DefaultSize}");
            Console.WriteLine($"- Default Format: {model.DefaultOutputFormat}");
            Console.WriteLine("✓ Advanced configuration successful");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Advanced configuration failed: {ex.Message}");
        }
    }

    static async Task RunNegativePromptExample(StableImageCoreModel model)
    {
        Console.WriteLine("=== Negative Prompt Example ===");
        
        try
        {
            var request = new ImageGenerationRequest
            {
                Prompt = "A peaceful garden with beautiful flowers and butterflies",
                NegativePrompt = "people, buildings, cars, pollution, noise",
                Size = "1024x1024",
                OutputFormat = "png"
            };

            Console.WriteLine($"Prompt: '{request.Prompt}'");
            Console.WriteLine($"Negative Prompt: '{request.NegativePrompt}'");

            ValidateRequest(request);
            
            Console.WriteLine("✓ Negative prompt example validation passed");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Negative prompt example failed: {ex.Message}");
        }
    }

    static async Task RunSeedBasedGenerationExample(StableImageCoreModel model)
    {
        Console.WriteLine("=== Seed-Based Generation Example ===");
        
        try
        {
            var baseSeed = 12345;
            var prompt = "Abstract digital art with flowing patterns and vibrant colors";

            // Generate multiple variations with different seeds
            for (int i = 0; i < 3; i++)
            {
                var request = new ImageGenerationRequest
                {
                    Prompt = prompt,
                    Seed = baseSeed + i,
                    Size = "512x512",
                    OutputFormat = "png"
                };

                Console.WriteLine($"Variation {i + 1} - Seed: {request.Seed}");
                ValidateRequest(request);
            }
            
            Console.WriteLine("✓ Seed-based generation examples validated");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Seed-based generation failed: {ex.Message}");
        }
    }

    static async Task RunDifferentSizesExample(StableImageCoreModel model)
    {
        Console.WriteLine("=== Different Sizes Example ===");
        
        try
        {
            var prompt = "A futuristic cityscape at night with neon lights";
            var sizes = new[] { "512x512", "1024x1024", "1920x1080", "768x1024" };

            foreach (var size in sizes)
            {
                var request = new ImageGenerationRequest
                {
                    Prompt = prompt,
                    Size = size,
                    OutputFormat = "png"
                };

                Console.WriteLine($"Testing size: {size}");
                ValidateRequest(request);
            }
            
            Console.WriteLine("✓ Different sizes examples validated");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Different sizes example failed: {ex.Message}");
        }
    }

    static async Task RunErrorHandlingExample()
    {
        Console.WriteLine("=== Error Handling Example ===");
        
        // Test various error scenarios
        Console.WriteLine("Testing invalid configurations and requests...");

        // Test invalid endpoint
        try
        {
            var invalidOptions = new StableImageCoreOptions
            {
                Endpoint = "invalid-url",
                ApiKey = "test-key"
            };
            invalidOptions.Validate();
            Console.WriteLine("✗ Should have failed with invalid endpoint");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("✓ Correctly caught invalid endpoint error");
        }

        // Test empty API key
        try
        {
            var invalidOptions = new StableImageCoreOptions
            {
                Endpoint = "https://valid-endpoint.com",
                ApiKey = ""
            };
            invalidOptions.Validate();
            Console.WriteLine("✗ Should have failed with empty API key");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("✓ Correctly caught empty API key error");
        }

        // Test invalid request size
        try
        {
            var invalidRequest = new ImageGenerationRequest
            {
                Prompt = "Test prompt",
                Size = "invalid-size"
            };
            invalidRequest.Validate();
            Console.WriteLine("✗ Should have failed with invalid size");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("✓ Correctly caught invalid size error");
        }

        // Test invalid output format
        try
        {
            var invalidRequest = new ImageGenerationRequest
            {
                Prompt = "Test prompt",
                OutputFormat = "invalid-format"
            };
            invalidRequest.Validate();
            Console.WriteLine("✗ Should have failed with invalid format");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("✓ Correctly caught invalid format error");
        }

        Console.WriteLine("✓ Error handling examples completed");
        Console.WriteLine();
    }

    static void ValidateRequest(ImageGenerationRequest request)
    {
        request.Validate();
        
        // In a real implementation, you would:
        // 1. Create an HTTP client
        // 2. Send the request to the API
        // 3. Handle the response
        // 4. Process the generated image
        
        Console.WriteLine($"  ✓ Request validated: {request.Model}");
    }

    static async Task SimulateImageGeneration(ImageGenerationRequest request, string outputPath)
    {
        // This would be replaced with actual API calls in a real implementation
        Console.WriteLine($"  Simulating API call for: {request.Prompt}");
        Console.WriteLine($"  Output would be saved to: {outputPath}");
        
        // Simulate processing time
        await Task.Delay(100);
        
        // In a real implementation:
        // var response = await apiClient.GenerateImageAsync(request);
        // await response.SaveImageAsync(outputPath);
        
        Console.WriteLine($"  ✓ Simulated generation complete");
    }
}
