# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2025-06-12

### Added

#### 🎨 Advanced AI Image Models
- **GPT-Image-1 Model** - Complete support for Azure OpenAI's GPT-Image-1 model
- **DALL-E 3 Model** - Full integration with Azure OpenAI's DALL-E 3 model
  - **Image Generation** - Generate high-quality images from text prompts
    - Support for customizable image sizes (1024x1024, 1024x1536, 1536x1024)
    - Three quality levels: low, medium, high (optimized for speed vs quality)
    - Multiple output formats: PNG, JPEG with configurable compression
    - Batch generation support (1-10 images per request)
    - User tracking for usage monitoring
    - Revised prompt access to see how prompts were interpreted
  - **Image Editing** - Advanced image editing capabilities
    - Edit existing images using text prompts
    - Optional mask support for precise editing regions
    - Same quality and format options as generation
    - Multipart/form-data API support for image uploads
    - Helper methods for file-based and byte-based image handling
     - **Unified Model Architecture** - Single model supporting both generation and editing
     - Implements both `IImageGenerationModel` and `IImageEditingModel` interfaces
     - Per-deployment configuration with individual endpoints and API keys
     - Configurable timeouts, retry policies, and error handling
     - Built-in validation for all request parameters
- **DALL-E 3 Model** - Azure OpenAI's flagship image generation model
  - **Premium Image Generation** - Create stunning, high-quality images from text
    - Three optimized aspect ratios: 1024x1024 (square), 1792x1024 (landscape), 1024x1792 (portrait)
    - Dual quality modes: "standard" for speed, "hd" for maximum quality
    - Two distinct styles: "natural" for realistic images, "vivid" for hyper-real cinematic output
    - Flexible response formats: URL downloads or direct base64-encoded data
    - Single image focus (n=1) for highest quality generation
    - 24-hour URL availability for convenient image access
  - **Enhanced Model Features** - Advanced capabilities beyond basic generation
    - Revised prompt access showing how DALL-E 3 interpreted your request
    - Built-in content safety with comprehensive filtering
    - Automatic prompt enhancement and optimization
    - Superior prompt understanding and visual coherence
  - **Generation-Only Architecture** - Specialized for creation (editing not supported)
    - Implements `IImageGenerationModel` interface for generation capabilities
    - Optimized configuration for DALL-E 3's specific parameters and constraints
    - Comprehensive validation for size, quality, and style combinations

#### 🏗️ Enhanced Core Architecture
- **Image Editing Interface** - New `IImageEditingModel` interface for editing capabilities
  - Standardized editing endpoint and configuration management
  - Consistent timeout and retry behavior across all editing models
  - Extensible design for future editing model implementations

#### 📁 Request/Response Models
- **ImageGenerationRequest** - Comprehensive request model for image generation
  - Full parameter validation with descriptive error messages
  - JSON serialization attributes for proper API communication
  - Support for all GPT-Image-1 generation parameters
- **ImageGenerationResponse** - Rich response model with helper methods
  - URL-based image access with 24-hour availability
  - Automatic HTTP download and file saving capabilities
  - Error handling for content filtering and API failures
  - Timestamp conversion and metadata access
- **ImageEditingRequest** - Specialized request model for image editing
  - Multipart/form-data structure for file uploads
  - Static factory methods for file and byte-based creation
  - Mask image support with validation
  - Helper methods for common editing scenarios
- **ImageEditingResponse** - Inherits from generation response for consistency
  - Same download and error handling capabilities
  - Semantic clarity for editing vs generation operations
- **DALL-E 3 Request/Response Models** - Specialized models for DALL-E 3 API
  - **ImageGenerationRequest** - DALL-E 3 specific request with style and response format options
  - **ImageGenerationResponse** - Dual-format response supporting both URL and base64 data
  - **Intelligent Data Handling** - Automatic detection and handling of response format
  - **Enhanced Error Handling** - DALL-E 3 specific error codes and content filtering responses

#### 🔧 Configuration & Options
- **GPTImage1Options** - Comprehensive configuration class
  - Required deployment name for Azure OpenAI resource targeting
  - Flexible API version support (default: 2025-04-01-preview)
  - Configurable default values for size, quality, format, and compression
  - Extensive validation with descriptive error messages
  - Support for custom timeout and retry policies
- **DALLE3Options** - Specialized configuration for DALL-E 3
  - Azure OpenAI deployment targeting with required deployment name
  - API version support (default: 2024-02-01)
  - Style and quality defaults with validation
  - Response format configuration (URL vs base64)
  - DALL-E 3 specific size validation and error handling

#### ✅ Testing & Quality Assurance
- **Comprehensive Unit Tests** - Full test coverage for all components
  - `GPTImage1OptionsTests` - Configuration validation and default value testing
  - `ImageGenerationRequestTests` - Request model validation and parameter testing  
  - `ImageEditingRequestTests` - Editing request validation and file handling testing
  - `DALLE3OptionsTests` - DALL-E 3 configuration validation and style/quality testing
  - `DALLE3 ImageGenerationRequestTests` - DALL-E 3 specific request validation and constraints
  - Edge case coverage including null values, invalid ranges, and malformed data
  - Mock-based testing for external dependencies
  - Async operation testing for file I/O operations
  - Response format testing for both URL and base64 data handling

### Enhanced
- **Package Metadata** - Updated package tags to include GPT-Image-1 and image editing
- **Documentation** - Comprehensive inline documentation for all public APIs
- **Error Handling** - Enhanced validation with specific error messages for troubleshooting

### Technical Details
- **API Compatibility** - Dual Azure OpenAI model support
  - GPT-Image-1 API version 2025-04-01-preview with generation and editing
  - DALL-E 3 API version 2024-02-01 with premium generation capabilities
- **Content Safety** - Built-in support for Azure's content filtering responses
- **Image Formats** - Multiple format support across models
  - GPT-Image-1: PNG and JPEG output with customizable compression (0-100)
  - DALL-E 3: URL-based downloads or base64-encoded data with automatic handling
- **Size Support** - Model-optimized aspect ratios
  - GPT-Image-1: 1024x1024, 1024x1536, 1536x1024 with batch processing (1-10 images)
  - DALL-E 3: 1024x1024, 1792x1024, 1024x1792 with single high-quality generation
- **File Operations** - Seamless file I/O with automatic directory creation
- **Memory Efficiency** - Byte array handling for in-memory image processing
- **Response Handling** - Intelligent format detection and processing for optimal performance

## [1.0.0] - 2025-06-10

### Added

#### 🎨 AI-Powered Image Generation
- **Stable Image Ultra Model** - Advanced image generation with customizable parameters
  - High-quality image output with fine-tuned controls
  - Support for custom prompts, negative prompts, and seeds
  - Multiple output formats (PNG, JPG, JPEG, WEBP)
  - Configurable image dimensions and aspect ratios
- **Stable Image Core Model** - Essential image generation capabilities
  - Core functionality for standard image generation tasks
  - Optimized for performance and reliability
  - Separate endpoint and configuration support

#### 👁️ Computer Vision & AI Analysis
- **Azure Vision Captioning** - Generate descriptive captions for images
  - Support for both image streams and URLs
  - Multiple language support for international applications
  - Confidence scoring for caption quality assessment
  - Gender-neutral caption options
- **Dense Captioning** - Advanced region-specific image analysis
  - Multiple captions for different regions within a single image
  - Bounding box coordinates for precise region identification
  - Confidence scores for each region caption
  - Ideal for detailed image understanding and analysis

#### 🛠️ Image Processing Utilities
- **ImageFormatConverter** - Comprehensive format handling
  - Convert between image formats and MIME types
  - Support for PNG, JPG, GIF, BMP, WEBP, TIFF, SVG, ICO
  - Validation of supported formats
  - Bidirectional conversion (extension ↔ MIME type)
- **ImageQualityAnalyzer** - Advanced quality assessment
  - Sharpness analysis for image clarity measurement
  - Brightness analysis for exposure assessment
  - Contrast analysis for dynamic range evaluation
  - Overall quality scoring with composite metrics
- **ImageMetadataExtractor** - Comprehensive metadata extraction
  - Image dimensions (width, height)
  - File format and MIME type detection
  - File size analysis
  - Extended metadata properties
- **ImageSizeValidator** - Flexible validation system
  - Dimension validation with customizable limits
  - File size validation for upload restrictions
  - Comprehensive validation reporting
- **AspectRatioConverter** - Aspect ratio management
  - Calculate aspect ratios from dimensions
  - Generate dimensions for target aspect ratios
  - Aspect ratio validation with tolerance settings

#### 🏗️ Core Architecture & Infrastructure
- **Modular Client Architecture**
  - `IAzureImageClient` interface for client abstraction
  - `AzureImageClient` implementation with dependency injection support
  - `ModelHttpClientService` for HTTP communication with Azure AI services
  - Extensible design for adding new AI models and services
- **Dependency Injection Support** - Full integration with .NET DI container
  - `ServiceCollectionExtensions` for easy service registration
  - Configuration support via `appsettings.json` and environment variables
  - ASP.NET Core integration with built-in DI container
- **Error Handling** - Comprehensive exception management
  - `AzureImageException` for Azure-specific errors
  - `AzureVisionException` for vision-specific errors
  - Proper HTTP status code handling and error propagation
  - Detailed error messages with context information
- **Retry Logic** - Robust retry mechanisms
  - Built-in exponential backoff retry mechanism
  - Configurable max retry attempts per operation
  - Configurable retry delays with jitter
  - Timeout handling with proper cancellation
- **Logging Integration** - Structured logging throughout
  - Microsoft.Extensions.Logging integration
  - Configurable log levels per component
  - Structured logging with contextual information
- **Type Safety** - Strongly typed throughout
  - `ImageGenerationRequest` and `ImageGenerationResponse` models
  - `IImageModel` and `IImageGenerationModel` interfaces
  - `IImageCaptioningModel` for vision capabilities
  - Compile-time type checking for all operations
- **Async/Await Support** - Fully asynchronous API
  - All operations support cancellation tokens
  - Proper async/await patterns throughout
  - Non-blocking I/O for optimal performance
- **Configuration Flexibility**
  - Multiple configuration sources (appsettings.json, environment variables, code)
  - Per-model configuration with individual endpoints and API keys
  - Runtime configuration updates support
- **File Operations** - Comprehensive file handling
  - Easy image saving with automatic format detection
  - Byte array access for in-memory processing
  - Stream-based operations for large files
- **Testing & Quality Assurance**
  - Comprehensive unit test coverage for all components
  - Integration tests for end-to-end scenarios
  - Mock services for testing without live APIs

### Framework Support
- .NET 6.0+
- .NET 7.0+
- .NET 8.0+

### Dependencies
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.Options
- Microsoft.Extensions.Http
- System.Text.Json

### Configuration
- Support for multiple deployment endpoints
- Per-model API key configuration
- Configurable timeouts and retry policies
- Environment variable support
- appsettings.json integration

### API Features
- Simple one-line image generation
- Advanced parameter customization
- Metadata access (dimensions, seed, format)
- Batch operations support
- Streaming response handling

### Security
- Secure API key handling
- HTTPS enforcement
- Input validation and sanitization 