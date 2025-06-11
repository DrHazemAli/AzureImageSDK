# GPT-Image-1 Sample

This sample demonstrates how to use the GPT-Image-1 model with the AzureImage SDK.

## Features

- Image Generation with customizable parameters
- Image Editing with text prompts
- Batch processing support
- Multiple aspect ratios
- Quality and format options

## Setup

1. Update `appsettings.json` with your Azure OpenAI configuration
2. Run with `dotnet run`

## Configuration

Replace these values in appsettings.json:
- Endpoint: Your Azure OpenAI resource URL
- ApiKey: Your API key
- DeploymentName: Your GPT-Image-1 deployment name

## Supported Sizes

- 1024x1024 (Square)
- 1024x1536 (Portrait)
- 1536x1024 (Landscape)

## Quality Levels

- low: Fast generation
- medium: Balanced
- high: Best quality 