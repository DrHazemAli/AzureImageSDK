using AzureImage.Inference.Models.DALLE3;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace DALLE3.Sample;

class Program
{
    private static readonly HttpClient httpClient = new();
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("🎨 DALL-E 3 Azure Image SDK Sample");
        Console.WriteLine("==================================");
        Console.WriteLine();

        try
        {
            // Load configuration
            var config = LoadConfiguration();
            
            // Create the DALL-E 3 model
            var model = CreateDALLE3Model(config);
            
            Console.WriteLine($"✅ DALL-E 3 Model configured:");
            Console.WriteLine($"   Endpoint: {model.Endpoint}");
            Console.WriteLine($"   Deployment: {model.DeploymentName}");
            Console.WriteLine($"   API Version: {model.ApiVersion}");
            Console.WriteLine();

            // Demonstrate different capabilities
            await RunImageGenerationSamples(model);
            
            Console.WriteLine("🎉 All samples completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"   Inner Exception: {ex.InnerException.Message}");
            }
        }
        
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static async Task RunImageGenerationSamples(DALLE3Model model)
    {
        Console.WriteLine("🖼️  DALL-E 3 Image Generation Samples");
        Console.WriteLine("=====================================");
        Console.WriteLine();

        // Sample 1: Basic image generation
        await BasicImageGeneration(model);
        
        // Sample 2: High-quality with different style
        await HighQualityImageGeneration(model);
        
        // Sample 3: Different aspect ratios
        await DifferentAspectRatios(model);
        
        // Sample 4: Response format comparison
        await ResponseFormatComparison(model);
        
        Console.WriteLine();
    }

    private static async Task BasicImageGeneration(DALLE3Model model)
    {
        try
        {
            Console.WriteLine("1️⃣  Basic DALL-E 3 Generation");
            Console.WriteLine("   Generating with vivid style...");

            var request = new ImageGenerationRequest
            {
                Prompt = "A futuristic cityscape at sunset with flying cars and neon lights",
                Size = "1024x1024",
                Quality = "standard",
                Style = "vivid",
                ResponseFormat = "url"
            };

            // Validate the request
            request.Validate();
            Console.WriteLine("   ✅ Request validated successfully");
            Console.WriteLine($"      Prompt: {request.Prompt}");
            Console.WriteLine($"      Size: {request.Size}");
            Console.WriteLine($"      Style: {request.Style}");
            Console.WriteLine($"      Quality: {request.Quality}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in basic generation: {ex.Message}");
        }
    }

    private static async Task HighQualityImageGeneration(DALLE3Model model)
    {
        try
        {
            Console.WriteLine("2️⃣  High-Quality Natural Style");
            Console.WriteLine("   Generating with HD quality and natural style...");

            var request = new ImageGenerationRequest
            {
                Prompt = "A peaceful meadow with wildflowers, realistic photography style",
                Size = "1024x1024",
                Quality = "hd",
                Style = "natural",
                ResponseFormat = "url"
            };

            request.Validate();
            Console.WriteLine("   ✅ HD quality request validated");
            Console.WriteLine($"      Style: {request.Style} (more realistic)");
            Console.WriteLine($"      Quality: {request.Quality} (higher detail)");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in HD generation: {ex.Message}");
        }
    }

    private static async Task DifferentAspectRatios(DALLE3Model model)
    {
        try
        {
            Console.WriteLine("3️⃣  Different Aspect Ratios");
            Console.WriteLine("   Demonstrating DALL-E 3's supported formats...");

            var formats = new[]
            {
                ("1024x1024", "Square - Perfect for social media"),
                ("1792x1024", "Landscape - Great for banners"),
                ("1024x1792", "Portrait - Ideal for mobile screens")
            };

            foreach (var (size, description) in formats)
            {
                var request = new ImageGenerationRequest
                {
                    Prompt = $"Beautiful architecture showcasing {description.Split('-')[1].Trim()}",
                    Size = size,
                    Quality = "standard",
                    Style = "vivid"
                };

                request.Validate();
                Console.WriteLine($"   ✅ {description} ({size}) - Validated");
            }
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in aspect ratio demo: {ex.Message}");
        }
    }

    private static async Task ResponseFormatComparison(DALLE3Model model)
    {
        try
        {
            Console.WriteLine("4️⃣  Response Format Options");
            Console.WriteLine("   DALL-E 3 supports both URL and base64 responses...");

            // URL format (default)
            var urlRequest = new ImageGenerationRequest
            {
                Prompt = "A minimalist logo design",
                Size = "1024x1024",
                Quality = "hd",
                Style = "natural",
                ResponseFormat = "url"
            };

            urlRequest.Validate();
            Console.WriteLine("   ✅ URL Response Format:");
            Console.WriteLine("      - Images accessible via download URLs");
            Console.WriteLine("      - URLs valid for 24 hours");
            Console.WriteLine("      - Best for web applications");

            // Base64 format
            var base64Request = new ImageGenerationRequest
            {
                Prompt = "A minimalist logo design",
                Size = "1024x1024",
                Quality = "hd",
                Style = "natural",
                ResponseFormat = "b64_json"
            };

            base64Request.Validate();
            Console.WriteLine("   ✅ Base64 Response Format:");
            Console.WriteLine("      - Images embedded directly in response");
            Console.WriteLine("      - No separate download required");
            Console.WriteLine("      - Best for immediate processing");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in response format demo: {ex.Message}");
        }
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
    }

    private static DALLE3Model CreateDALLE3Model(IConfiguration config)
    {
        var section = config.GetSection("DALLE3");
        
        var options = new DALLE3Options
        {
            Endpoint = section["Endpoint"] ?? throw new InvalidOperationException("DALLE3:Endpoint is required"),
            ApiKey = section["ApiKey"] ?? throw new InvalidOperationException("DALLE3:ApiKey is required"),
            DeploymentName = section["DeploymentName"] ?? throw new InvalidOperationException("DALLE3:DeploymentName is required"),
            ApiVersion = section["ApiVersion"] ?? "2024-02-01",
            ModelName = section["ModelName"] ?? "dall-e-3"
        };

        return new DALLE3Model(options);
    }
} 