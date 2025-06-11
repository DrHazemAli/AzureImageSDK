using AzureImage.Core;
using AzureImage.Inference.Models.StableImageUltra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace StableImageUltra.Sample;

class Program
{
    private static IConfiguration? _configuration;
    private static ILogger<Program>? _logger;

    static async Task Main(string[] args)
    {
        // Setup configuration and logging
        Setup();

        Console.WriteLine("🎨 StableImageUltra Sample Application");
        Console.WriteLine("====================================");

        try
        {
            // Validate configuration
            if (!ValidateConfiguration())
            {
                return;
            }

            // Show menu and handle user choice
            await ShowMenuAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Application error occurred");
            Console.WriteLine($"❌ Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    private static void Setup()
    {
        // Build configuration
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Setup logging
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        var serviceProvider = services.BuildServiceProvider();
        _logger = serviceProvider.GetService<ILogger<Program>>();
    }

    private static bool ValidateConfiguration()
    {
        var endpoint = _configuration?["StableImageUltra:Endpoint"];
        var apiKey = _configuration?["StableImageUltra:ApiKey"];

        if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("❌ Configuration Error:");
            Console.WriteLine("Please set your StableImageUltra endpoint and API key in appsettings.json or environment variables:");
            Console.WriteLine("- StableImageUltra:Endpoint");
            Console.WriteLine("- StableImageUltra:ApiKey");
            Console.WriteLine();
            Console.WriteLine("Example appsettings.json:");
            Console.WriteLine("""
            {
              "StableImageUltra": {
                "Endpoint": "https://your-endpoint.eastus.models.ai.azure.com",
                "ApiKey": "your-api-key-here"
              }
            }
            """);
            return false;
        }

        return true;
    }

    private static async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. 🖼️  Basic Image Generation");
            Console.WriteLine("2. 🎯 Advanced Image Generation");
            Console.WriteLine("3. 🌱 Reproducible Generation (with Seed)");
            Console.WriteLine("4. 🎨 Creative Prompts Gallery");
            Console.WriteLine("5. 📐 Different Image Sizes");
            Console.WriteLine("6. 🔄 Batch Generation");
            Console.WriteLine("7. ⚙️  Configuration Examples");
            Console.WriteLine("0. 🚪 Exit");
            Console.Write("\nEnter your choice (0-7): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await BasicImageGenerationAsync();
                    break;
                case "2":
                    await AdvancedImageGenerationAsync();
                    break;
                case "3":
                    await ReproducibleGenerationAsync();
                    break;
                case "4":
                    await CreativePromptsGalleryAsync();
                    break;
                case "5":
                    await DifferentImageSizesAsync();
                    break;
                case "6":
                    await BatchGenerationAsync();
                    break;
                case "7":
                    await ConfigurationExamplesAsync();
                    break;
                case "0":
                    Console.WriteLine("👋 Goodbye!");
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static async Task BasicImageGenerationAsync()
    {
        Console.WriteLine("\n🖼️  Basic Image Generation");
        Console.WriteLine("=========================");

        try
        {
            // Create model with basic configuration
            var model = StableImageUltraModel.Create(
                endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                apiKey: _configuration!["StableImageUltra:ApiKey"]!);

            // Create client
            using var client = AzureImageClient.Create();

            Console.Write("Enter your prompt: ");
            var prompt = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(prompt))
            {
                Console.WriteLine("❌ Prompt cannot be empty.");
                return;
            }

            Console.WriteLine($"\n🔄 Generating image for: \"{prompt}\"");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = prompt
            };

            var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, request);

            var fileName = $"basic_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            await response.SaveImageAsync(fileName);

            Console.WriteLine($"✅ Image generated successfully!");
            Console.WriteLine($"💾 Saved as: {fileName}");
            Console.WriteLine($"📊 Image data size: {response.Image.Length} characters");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Basic image generation failed");
            Console.WriteLine($"❌ Generation failed: {ex.Message}");
        }
    }

    private static async Task AdvancedImageGenerationAsync()
    {
        Console.WriteLine("\n🎯 Advanced Image Generation");
        Console.WriteLine("============================");

        try
        {
            var model = StableImageUltraModel.Create(
                endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                apiKey: _configuration!["StableImageUltra:ApiKey"]!);

            using var client = AzureImageClient.Create();

            Console.Write("Enter your prompt: ");
            var prompt = Console.ReadLine();

            Console.Write("Enter negative prompt (optional, press Enter to skip): ");
            var negativePrompt = Console.ReadLine();

            Console.Write("Enter size (default: 1024x1024): ");
            var size = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(size)) size = "1024x1024";

            Console.Write("Enter format (png/jpg/webp, default: png): ");
            var format = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(format)) format = "png";

            if (string.IsNullOrWhiteSpace(prompt))
            {
                Console.WriteLine("❌ Prompt cannot be empty.");
                return;
            }

            Console.WriteLine($"\n🔄 Generating advanced image...");
            Console.WriteLine($"📝 Prompt: {prompt}");
            if (!string.IsNullOrWhiteSpace(negativePrompt))
                Console.WriteLine($"🚫 Negative: {negativePrompt}");
            Console.WriteLine($"📐 Size: {size}");
            Console.WriteLine($"🎨 Format: {format}");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = prompt,
                NegativePrompt = string.IsNullOrWhiteSpace(negativePrompt) ? null : negativePrompt,
                Size = size,
                OutputFormat = format
            };

            var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, request);

            var fileName = $"advanced_{DateTime.Now:yyyyMMdd_HHmmss}.{format}";
            await response.SaveImageAsync(fileName);

            Console.WriteLine($"✅ Advanced image generated successfully!");
            Console.WriteLine($"💾 Saved as: {fileName}");

            if (response.Metadata != null)
            {
                Console.WriteLine("📊 Metadata:");
                Console.WriteLine($"   - Dimensions: {response.Metadata.Width}x{response.Metadata.Height}");
                Console.WriteLine($"   - Format: {response.Metadata.Format}");
                Console.WriteLine($"   - Model: {response.Metadata.Model}");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Advanced image generation failed");
            Console.WriteLine($"❌ Generation failed: {ex.Message}");
        }
    }

    private static async Task ReproducibleGenerationAsync()
    {
        Console.WriteLine("\n🌱 Reproducible Generation (with Seed)");
        Console.WriteLine("======================================");

        try
        {
            var model = StableImageUltraModel.Create(
                endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                apiKey: _configuration!["StableImageUltra:ApiKey"]!);

            using var client = AzureImageClient.Create();

            var prompt = "A cozy cottage in an enchanted forest with glowing mushrooms";
            var seed = 12345;

            Console.WriteLine($"📝 Prompt: {prompt}");
            Console.WriteLine($"🌱 Seed: {seed}");
            Console.WriteLine("\n🔄 Generating 3 images with the same seed (should be identical)...");

            for (int i = 1; i <= 3; i++)
            {
                var request = new ImageGenerationRequest
                {
                    Model = model.ModelName,
                    Prompt = prompt,
                    Seed = seed
                };

                var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                    model, request);

                var fileName = $"reproducible_{seed}_variation_{i}.png";
                await response.SaveImageAsync(fileName);

                Console.WriteLine($"✅ Image {i}/3 generated: {fileName}");
                
                if (response.Metadata?.Seed != null)
                {
                    Console.WriteLine($"   🌱 Confirmed seed: {response.Metadata.Seed}");
                }

                // Small delay to avoid rate limiting
                if (i < 3) await Task.Delay(1000);
            }

            Console.WriteLine("\n📋 Compare the generated images - they should be identical!");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Reproducible generation failed");
            Console.WriteLine($"❌ Generation failed: {ex.Message}");
        }
    }

    private static async Task CreativePromptsGalleryAsync()
    {
        Console.WriteLine("\n🎨 Creative Prompts Gallery");
        Console.WriteLine("===========================");

        var creativePrompts = new[]
        {
            new { Name = "Fantasy", Prompt = "A majestic dragon perched on a crystal castle, aurora borealis in the background, fantasy art style", NegativePrompt = "blurry, low quality" },
            new { Name = "Cyberpunk", Prompt = "Neon-lit alleyway in a cyberpunk city, rain reflections, holographic advertisements, detailed architecture", NegativePrompt = "daylight, nature, vintage" },
            new { Name = "Nature", Prompt = "Ancient redwood forest with sunbeams filtering through mist, moss-covered ground, serene atmosphere", NegativePrompt = "urban, technology, people" },
            new { Name = "Space", Prompt = "Astronaut floating near a colorful nebula, distant galaxies, cosmic beauty, space photography style", NegativePrompt = "earth, gravity, indoor" },
            new { Name = "Abstract", Prompt = "Flowing liquid gold and silver patterns, geometric fractals, ethereal energy, abstract digital art", NegativePrompt = "realistic, photographic, objects" }
        };

        Console.WriteLine("Available prompts:");
        for (int i = 0; i < creativePrompts.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {creativePrompts[i].Name}");
        }

        Console.Write($"\nChoose a prompt (1-{creativePrompts.Length}), or 0 for all: ");
        var choice = Console.ReadLine();

        try
        {
            var model = StableImageUltraModel.Create(
                endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                apiKey: _configuration!["StableImageUltra:ApiKey"]!);

            using var client = AzureImageClient.Create();

            if (choice == "0")
            {
                // Generate all prompts
                Console.WriteLine("\n🔄 Generating all creative prompts...");

                for (int i = 0; i < creativePrompts.Length; i++)
                {
                    var promptData = creativePrompts[i];
                    Console.WriteLine($"\n🎨 Generating {promptData.Name}...");

                    var request = new ImageGenerationRequest
                    {
                        Model = model.ModelName,
                        Prompt = promptData.Prompt,
                        NegativePrompt = promptData.NegativePrompt,
                        Size = "1024x1024"
                    };

                    var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                        model, request);

                    var fileName = $"gallery_{promptData.Name.ToLower()}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    await response.SaveImageAsync(fileName);

                    Console.WriteLine($"✅ {promptData.Name} saved as: {fileName}");

                    // Delay to avoid rate limiting
                    if (i < creativePrompts.Length - 1) await Task.Delay(2000);
                }
            }
            else if (int.TryParse(choice, out int index) && index >= 1 && index <= creativePrompts.Length)
            {
                // Generate single prompt
                var promptData = creativePrompts[index - 1];
                Console.WriteLine($"\n🔄 Generating {promptData.Name}...");
                Console.WriteLine($"📝 Prompt: {promptData.Prompt}");

                var request = new ImageGenerationRequest
                {
                    Model = model.ModelName,
                    Prompt = promptData.Prompt,
                    NegativePrompt = promptData.NegativePrompt,
                    Size = "1024x1024"
                };

                var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                    model, request);

                var fileName = $"gallery_{promptData.Name.ToLower()}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                await response.SaveImageAsync(fileName);

                Console.WriteLine($"✅ {promptData.Name} generated successfully!");
                Console.WriteLine($"💾 Saved as: {fileName}");
            }
            else
            {
                Console.WriteLine("❌ Invalid choice.");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Creative prompts generation failed");
            Console.WriteLine($"❌ Generation failed: {ex.Message}");
        }
    }

    private static async Task DifferentImageSizesAsync()
    {
        Console.WriteLine("\n📐 Different Image Sizes");
        Console.WriteLine("========================");

        var sizes = new[]
        {
            "512x512",
            "768x768", 
            "1024x1024",
            "1024x768",
            "768x1024"
        };

        Console.WriteLine("Available sizes:");
        for (int i = 0; i < sizes.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {sizes[i]}");
        }

        Console.Write($"\nChoose a size (1-{sizes.Length}): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out int index) || index < 1 || index > sizes.Length)
        {
            Console.WriteLine("❌ Invalid choice.");
            return;
        }

        var selectedSize = sizes[index - 1];

        Console.Write("Enter your prompt: ");
        var prompt = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(prompt))
        {
            Console.WriteLine("❌ Prompt cannot be empty.");
            return;
        }

        try
        {
            var model = StableImageUltraModel.Create(
                endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                apiKey: _configuration!["StableImageUltra:ApiKey"]!);

            using var client = AzureImageClient.Create();

            Console.WriteLine($"\n🔄 Generating {selectedSize} image...");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = prompt,
                Size = selectedSize
            };

            var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, request);

            var fileName = $"size_{selectedSize.Replace("x", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            await response.SaveImageAsync(fileName);

            Console.WriteLine($"✅ {selectedSize} image generated successfully!");
            Console.WriteLine($"💾 Saved as: {fileName}");

            if (response.Metadata != null)
            {
                Console.WriteLine($"📊 Actual dimensions: {response.Metadata.Width}x{response.Metadata.Height}");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Size-specific generation failed");
            Console.WriteLine($"❌ Generation failed: {ex.Message}");
        }
    }

    private static async Task BatchGenerationAsync()
    {
        Console.WriteLine("\n🔄 Batch Generation");
        Console.WriteLine("===================");

        var prompts = new[]
        {
            "A serene mountain lake at sunrise",
            "A bustling marketplace in an ancient city",
            "A futuristic spacecraft landing on an alien planet"
        };

        Console.WriteLine("Batch prompts:");
        for (int i = 0; i < prompts.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {prompts[i]}");
        }

        Console.WriteLine($"\n🔄 Generating {prompts.Length} images concurrently...");

        try
        {
            var model = StableImageUltraModel.Create(
                endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                apiKey: _configuration!["StableImageUltra:ApiKey"]!);

            using var client = AzureImageClient.Create();

            var tasks = prompts.Select(async (prompt, index) =>
            {
                var request = new ImageGenerationRequest
                {
                    Model = model.ModelName,
                    Prompt = prompt,
                    Size = "768x768"
                };

                var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                    model, request);

                var fileName = $"batch_{index + 1}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                await response.SaveImageAsync(fileName);

                return new { Index = index + 1, FileName = fileName, Prompt = prompt };
            });

            var results = await Task.WhenAll(tasks);

            Console.WriteLine("✅ Batch generation completed!");
            foreach (var result in results)
            {
                Console.WriteLine($"   {result.Index}. {result.FileName}");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Batch generation failed");
            Console.WriteLine($"❌ Batch generation failed: {ex.Message}");
        }
    }

    private static async Task ConfigurationExamplesAsync()
    {
        Console.WriteLine("\n⚙️  Configuration Examples");
        Console.WriteLine("=========================");

        Console.WriteLine("1. 🚀 High Performance Model (Fast, smaller images)");
        Console.WriteLine("2. 🎨 High Quality Model (Slower, larger images)");
        Console.WriteLine("3. 🔄 Custom Retry Model (More resilient)");

        Console.Write("\nChoose configuration (1-3): ");
        var choice = Console.ReadLine();

        var prompt = "A majestic eagle soaring over snow-capped mountains";

        try
        {
            StableImageUltraModel model;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n🚀 Creating High Performance Model...");
                    model = StableImageUltraModel.Create(
                        endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                        apiKey: _configuration!["StableImageUltra:ApiKey"]!,
                        options =>
                        {
                            options.ModelName = "Stable-Image-Ultra-Fast";
                            options.DefaultSize = "512x512";
                            options.DefaultOutputFormat = "jpg";
                            options.Timeout = TimeSpan.FromMinutes(2);
                            options.MaxRetryAttempts = 1;
                        });
                    break;

                case "2":
                    Console.WriteLine("\n🎨 Creating High Quality Model...");
                    model = StableImageUltraModel.Create(
                        endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                        apiKey: _configuration!["StableImageUltra:ApiKey"]!,
                        options =>
                        {
                            options.ModelName = "Stable-Image-Ultra-HQ";
                            options.DefaultSize = "1024x1024";
                            options.DefaultOutputFormat = "png";
                            options.Timeout = TimeSpan.FromMinutes(10);
                            options.MaxRetryAttempts = 2;
                        });
                    break;

                case "3":
                    Console.WriteLine("\n🔄 Creating Custom Retry Model...");
                    model = StableImageUltraModel.Create(
                        endpoint: _configuration!["StableImageUltra:Endpoint"]!,
                        apiKey: _configuration!["StableImageUltra:ApiKey"]!,
                        options =>
                        {
                            options.ModelName = "Stable-Image-Ultra-Resilient";
                            options.DefaultSize = "768x768";
                            options.Timeout = TimeSpan.FromMinutes(3);
                            options.MaxRetryAttempts = 5;
                            options.RetryDelay = TimeSpan.FromSeconds(3);
                        });
                    break;

                default:
                    Console.WriteLine("❌ Invalid choice.");
                    return;
            }

            using var client = AzureImageClient.Create();

            Console.WriteLine($"📝 Model: {model.ModelName}");
            Console.WriteLine($"📐 Default Size: {model.DefaultSize}");
            Console.WriteLine($"🎨 Default Format: {model.DefaultOutputFormat}");
            Console.WriteLine($"⏱️  Timeout: {model.Timeout}");
            Console.WriteLine($"🔄 Max Retries: {model.MaxRetryAttempts}");

            Console.WriteLine($"\n🔄 Generating with custom configuration...");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = prompt,
                Size = model.DefaultSize,
                OutputFormat = model.DefaultOutputFormat
            };

            var response = await client.GenerateImageAsync<ImageGenerationRequest, ImageGenerationResponse>(
                model, request);

            var fileName = $"config_{choice}_{DateTime.Now:yyyyMMdd_HHmmss}.{model.DefaultOutputFormat}";
            await response.SaveImageAsync(fileName);

            Console.WriteLine($"✅ Image generated with custom configuration!");
            Console.WriteLine($"💾 Saved as: {fileName}");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Configuration example failed");
            Console.WriteLine($"❌ Generation failed: {ex.Message}");
        }
    }
} 