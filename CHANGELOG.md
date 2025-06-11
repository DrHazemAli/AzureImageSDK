# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2024-12-19

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