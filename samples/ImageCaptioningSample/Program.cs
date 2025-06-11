using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AzureImage.Core;
using AzureImage.Inference.Models;
using AzureImage.Inference.Models.AzureVisionCaptioning;

namespace ImageCaptioningSample;

class Program
{
    private static async Task Main(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        // Setup logging
        using var loggerFactory = LoggerFactory.Create(builder =>
            builder
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information));

        var logger = loggerFactory.CreateLogger<Program>();

        try
        {
            logger.LogInformation("Starting Azure Image Captioning Sample");

            // Create Azure Image client
            using var azureImageClient = AzureImageClient.Create(loggerFactory);

            // Configure Azure Vision Captioning model
            var visionOptions = new AzureVisionCaptioningOptions
            {
                Endpoint = configuration["AzureVision:Endpoint"] ?? 
                          throw new InvalidOperationException("AzureVision:Endpoint not configured"),
                ApiKey = configuration["AzureVision:ApiKey"] ?? 
                        throw new InvalidOperationException("AzureVision:ApiKey not configured"),
                ApiVersion = "2024-02-01"
            };

            // Validate configuration
            visionOptions.Validate();

            // Create HTTP client and model
            using var httpClient = new HttpClient();
            var captioningModel = new AzureVisionCaptioningModel(httpClient, visionOptions);

            // Test image URLs
            var testImageUrls = new[]
            {
                "https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/280px-PNG_transparency_demonstration_1.png"
            };

            // Example 1: Generate single caption from URL
            await GenerateSingleCaptionFromUrl(azureImageClient, captioningModel, testImageUrls[0], logger);

            // Example 2: Generate single caption with options
            await GenerateCaptionWithOptions(azureImageClient, captioningModel, testImageUrls[0], logger);

            // Example 3: Generate dense captions
            await GenerateDenseCaptions(azureImageClient, captioningModel, testImageUrls[0], logger);

            // Example 4: Process local image file (if exists)
            await ProcessLocalImageFile(azureImageClient, captioningModel, logger);

            logger.LogInformation("Azure Image Captioning Sample completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during execution");
            Environment.Exit(1);
        }
    }

    private static async Task GenerateSingleCaptionFromUrl(
        IAzureImageClient client, 
        IImageCaptioningModel model, 
        string imageUrl, 
        ILogger logger)
    {
        logger.LogInformation("=== Example 1: Generate Single Caption from URL ===");
        logger.LogInformation("Image URL: {ImageUrl}", imageUrl);

        try
        {
            var result = await client.GenerateCaptionAsync(model, imageUrl);
            
            logger.LogInformation("Caption: {Caption}", result.Caption.Text);
            logger.LogInformation("Confidence: {Confidence:F4}", result.Caption.Confidence);
            logger.LogInformation("Model Version: {ModelVersion}", result.ModelVersion);
            logger.LogInformation("Image Dimensions: {Width}x{Height}", 
                result.Metadata.Width, result.Metadata.Height);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate caption for URL");
        }

        logger.LogInformation("");
    }

    private static async Task GenerateCaptionWithOptions(
        IAzureImageClient client, 
        IImageCaptioningModel model, 
        string imageUrl, 
        ILogger logger)
    {
        logger.LogInformation("=== Example 2: Generate Caption with Options ===");
        logger.LogInformation("Image URL: {ImageUrl}", imageUrl);

        try
        {
            var options = new ImageCaptionOptions
            {
                Language = "en",
                GenderNeutralCaption = true
            };

            var result = await client.GenerateCaptionAsync(model, imageUrl, options);
            
            logger.LogInformation("Gender-Neutral Caption: {Caption}", result.Caption.Text);
            logger.LogInformation("Confidence: {Confidence:F4}", result.Caption.Confidence);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate caption with options");
        }

        logger.LogInformation("");
    }

    private static async Task GenerateDenseCaptions(
        IAzureImageClient client, 
        IImageCaptioningModel model, 
        string imageUrl, 
        ILogger logger)
    {
        logger.LogInformation("=== Example 3: Generate Dense Captions ===");
        logger.LogInformation("Image URL: {ImageUrl}", imageUrl);

        try
        {
            var result = await client.GenerateDenseCaptionsAsync(model, imageUrl);
            
            logger.LogInformation("Found {CaptionCount} dense captions:", result.Captions.Count);
            
            for (int i = 0; i < result.Captions.Count; i++)
            {
                var caption = result.Captions[i];
                logger.LogInformation("  {Index}. {Text} (Confidence: {Confidence:F4})", 
                    i + 1, caption.Text, caption.Confidence);
                logger.LogInformation("     Region: {BoundingBox}", caption.BoundingBox);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate dense captions");
        }

        logger.LogInformation("");
    }

    private static async Task ProcessLocalImageFile(
        IAzureImageClient client, 
        IImageCaptioningModel model, 
        ILogger logger)
    {
        logger.LogInformation("=== Example 4: Process Local Image File ===");

        var sampleImagePath = "sample-image.jpg";
        
        if (!File.Exists(sampleImagePath))
        {
            logger.LogInformation("Sample image file '{SampleImagePath}' not found. Skipping local file example.", sampleImagePath);
            logger.LogInformation("To test with a local file, place an image named 'sample-image.jpg' in the application directory.");
            return;
        }

        try
        {
            using var imageStream = File.OpenRead(sampleImagePath);
            
            var result = await client.GenerateCaptionAsync(model, imageStream);
            
            logger.LogInformation("Local Image Caption: {Caption}", result.Caption.Text);
            logger.LogInformation("Confidence: {Confidence:F4}", result.Caption.Confidence);
            logger.LogInformation("File Size: {FileSize} bytes", new FileInfo(sampleImagePath).Length);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process local image file");
        }

        logger.LogInformation("");
    }
} 