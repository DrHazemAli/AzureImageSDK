# DALL-E 3 Sample

This sample demonstrates how to use the DALL-E 3 model with the AzureImage SDK for premium image generation.

## Features

- High-quality image generation with DALL-E 3
- Two distinct styles: natural and vivid
- HD and standard quality options
- Multiple aspect ratios (square, landscape, portrait)
- Flexible response formats (URL and base64)
- Single image focus for maximum quality

## Setup

1. Update `appsettings.json` with your Azure OpenAI configuration
2. Run with `dotnet run`

## Configuration

Replace these values in appsettings.json:
- Endpoint: Your Azure OpenAI resource URL
- ApiKey: Your API key
- DeploymentName: Your DALL-E 3 deployment name

## Supported Sizes

- 1024x1024 (Square) - Perfect for social media
- 1792x1024 (Landscape) - Great for banners
- 1024x1792 (Portrait) - Ideal for mobile screens

## Quality Levels

- standard: Balanced quality and speed
- hd: Maximum quality with finer details

## Styles

- natural: More realistic and subdued
- vivid: Hyper-real and cinematic

## Response Formats

- url: Download URLs (24-hour availability)
- b64_json: Direct base64-encoded data

## Key Differences from GPT-Image-1

- Single image generation only (n=1)
- Style parameter for artistic control
- Response format flexibility
- Different supported aspect ratios
- No image editing capabilities 