using AzureImage.Inference.Models.GPTImage1;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace GPTImage1.Sample;

class Program
{
    private static readonly HttpClient httpClient = new();
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("🎨 GPT-Image-1 Azure Image SDK Sample");
        Console.WriteLine("=====================================");
        Console.WriteLine();

        try
        {
            // Load configuration
            var config = LoadConfiguration();
            
            // Create the GPT-Image-1 model
            var model = CreateGPTImage1Model(config);
            
            Console.WriteLine($"✅ GPT-Image-1 Model configured:");
            Console.WriteLine($"   Endpoint: {model.Endpoint}");
            Console.WriteLine($"   Deployment: {model.DeploymentName}");
            Console.WriteLine($"   API Version: {model.ApiVersion}");
            Console.WriteLine();

            // Demonstrate different capabilities
            await RunImageGenerationSamples(model);
            await RunImageEditingSamples(model);
            
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

    private static async Task RunImageGenerationSamples(GPTImage1Model model)
    {
        Console.WriteLine("🖼️  Image Generation Samples");
        Console.WriteLine("============================");
        Console.WriteLine();

        // Sample 1: Basic image generation
        await BasicImageGeneration(model);
        
        // Sample 2: High-quality image generation
        await HighQualityImageGeneration(model);
        
        // Sample 3: Batch image generation
        await BatchImageGeneration(model);
        
        // Sample 4: Different aspect ratios
        await DifferentAspectRatios(model);
        
        Console.WriteLine();
    }

    private static async Task RunImageEditingSamples(GPTImage1Model model)
    {
        Console.WriteLine("✏️  Image Editing Samples");
        Console.WriteLine("=========================");
        Console.WriteLine();

        Console.WriteLine("ℹ️  Image editing samples require existing images.");
        Console.WriteLine("   For demonstration, we'll show request creation only.");
        Console.WriteLine("   Replace with actual API calls when you have images to edit.");
        Console.WriteLine();

        // Sample 1: Basic image editing setup
        await BasicImageEditingSetup(model);
        
        // Sample 2: Image editing with mask
        await ImageEditingWithMaskSetup(model);
        
        Console.WriteLine();
    }

    private static async Task BasicImageGeneration(GPTImage1Model model)
    {
        try
        {
            Console.WriteLine("1️⃣  Basic Image Generation");
            Console.WriteLine("   Generating a simple landscape image...");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = "A serene mountain landscape with a crystal clear lake reflecting the peaks",
                Size = "1024x1024",
                Quality = "high",
                N = 1
            };

            // Validate the request
            request.Validate();
            Console.WriteLine("   ✅ Request validated successfully");
            Console.WriteLine($"      Prompt: {request.Prompt}");
            Console.WriteLine($"      Size: {request.Size}");
            Console.WriteLine($"      Quality: {request.Quality}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in basic image generation: {ex.Message}");
        }
    }

    private static async Task HighQualityImageGeneration(GPTImage1Model model)
    {
        try
        {
            Console.WriteLine("2️⃣  High-Quality Image Generation");
            Console.WriteLine("   Generating with high quality settings...");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = "Professional portrait in modern office environment",
                Size = "1024x1024",
                Quality = "high",
                OutputFormat = "PNG",
                OutputCompression = 100,
                N = 1
            };

            request.Validate();
            Console.WriteLine("   ✅ High-quality request validated");
            Console.WriteLine($"      Quality: {request.Quality}");
            Console.WriteLine($"      Format: {request.OutputFormat}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in high-quality generation: {ex.Message}");
        }
    }

    private static async Task BatchImageGeneration(GPTImage1Model model)
    {
        try
        {
            Console.WriteLine("3️⃣  Batch Image Generation");
            Console.WriteLine("   Generating multiple variations...");

            var request = new ImageGenerationRequest
            {
                Model = model.ModelName,
                Prompt = "Abstract digital art with vibrant colors",
                Size = "1024x1024",
                Quality = "medium",
                N = 3
            };

            request.Validate();
            Console.WriteLine("   ✅ Batch request validated");
            Console.WriteLine($"      Generating {request.N} variations");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in batch generation: {ex.Message}");
        }
    }

    private static async Task DifferentAspectRatios(GPTImage1Model model)
    {
        try
        {
            Console.WriteLine("4️⃣  Different Aspect Ratios");
            Console.WriteLine("   Demonstrating various supported image sizes...");

            var sizes = new[] { "1024x1024", "1024x1536", "1536x1024" };
            var descriptions = new[] { "Square (1:1)", "Portrait (2:3)", "Landscape (3:2)" };

            for (int i = 0; i < sizes.Length; i++)
            {
                var request = new ImageGenerationRequest
                {
                    Model = model.ModelName,
                    Prompt = $"Beautiful nature scene optimized for {descriptions[i]} format",
                    Size = sizes[i],
                    Quality = "high"
                };

                request.Validate();
                Console.WriteLine($"   ✅ {descriptions[i]} ({sizes[i]}) - Request validated");
            }
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in aspect ratio demo: {ex.Message}");
        }
    }

    private static async Task BasicImageEditingSetup(GPTImage1Model model)
    {
        try
        {
            Console.WriteLine("1️⃣  Basic Image Editing Setup");
            Console.WriteLine("   Creating an image editing request...");

            var sampleImageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header

            var request = ImageEditingRequest.FromBytes(
                imageBytes: sampleImageBytes,
                prompt: "Add a bright blue sky with fluffy white clouds",
                imageFileName: "original_image.png"
            );

            request.Size = "1024x1024";
            request.Quality = "high";
            request.Model = model.ModelName;

            request.Validate();
            Console.WriteLine("   ✅ Image editing request validated");
            Console.WriteLine($"      Edit Prompt: {request.Prompt}");
            Console.WriteLine($"      Size: {request.Size}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in editing setup: {ex.Message}");
        }
    }

    private static async Task ImageEditingWithMaskSetup(GPTImage1Model model)
    {
        try
        {
            Console.WriteLine("2️⃣  Image Editing with Mask Setup");
            Console.WriteLine("   Creating an image editing request with mask for precise editing...");

            // Sample image and mask bytes
            var sampleImageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            var sampleMaskBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

            var request = ImageEditingRequest.FromBytes(
                imageBytes: sampleImageBytes,
                prompt: "Replace the masked area with a beautiful garden with colorful flowers",
                imageFileName: "original_image.png",
                maskBytes: sampleMaskBytes,
                maskFileName: "edit_mask.png"
            );

            request.Size = "1024x1536"; // Portrait orientation
            request.Quality = "high";
            request.Model = model.ModelName;
            request.OutputFormat = "PNG";
            request.OutputCompression = 100;

            request.Validate();
            Console.WriteLine("   ✅ Masked image editing request validated");
            Console.WriteLine($"      Edit Prompt: {request.Prompt}");
            Console.WriteLine($"      Image File: {request.ImageFileName}");
            Console.WriteLine($"      Mask File: {request.MaskFileName}");
            Console.WriteLine($"      Size: {request.Size}");
            Console.WriteLine($"      Output: {request.OutputFormat} at {request.OutputCompression}% compression");
            Console.WriteLine();

            // Demonstrate error handling for common issues
            Console.WriteLine("3️⃣  Error Handling Demonstration");
            Console.WriteLine("   Testing validation with invalid parameters...");

            try
            {
                var invalidRequest = new ImageEditingRequest
                {
                    Prompt = "", // Invalid: empty prompt
                    Image = Array.Empty<byte>(), // Invalid: empty image
                    Size = "invalid-size" // Invalid: unsupported size
                };
                invalidRequest.Validate();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"   ✅ Validation correctly caught error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ Error in masked editing setup: {ex.Message}");
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

    private static GPTImage1Model CreateGPTImage1Model(IConfiguration config)
    {
        var section = config.GetSection("GPTImage1");
        
        var options = new GPTImage1Options
        {
            Endpoint = section["Endpoint"] ?? throw new InvalidOperationException("GPTImage1:Endpoint is required"),
            ApiKey = section["ApiKey"] ?? throw new InvalidOperationException("GPTImage1:ApiKey is required"),
            DeploymentName = section["DeploymentName"] ?? throw new InvalidOperationException("GPTImage1:DeploymentName is required"),
            ApiVersion = section["ApiVersion"] ?? "2025-04-01-preview",
            ModelName = section["ModelName"] ?? "gpt-image-1"
        };

        return new GPTImage1Model(options);
    }
} 