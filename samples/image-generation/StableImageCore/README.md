# StableImageCore Model Documentation

## Overview
StableImageCore is an image generation model implementation that follows the same architecture pattern as StableImageUltra but is configured for the Stable-Image-Core model.

## Architecture
- Configuration class: `StableImageCoreOptions`
- Model implementation: `StableImageCoreModel`
- Request/Response models for API interaction
- Comprehensive test coverage

## Configuration
- Model Name: "Stable-Image-Core"
- API Version: "2024-05-01-preview"
- Default Size: "1024x1024"
- Default Output Format: "png"

## Key Features
- Factory method for easy instantiation
- Validation for configuration and requests
- Async image save functionality
- Comprehensive error handling
