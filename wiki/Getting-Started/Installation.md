# Installation Guide

This guide will help you install and set up the AzureImage SDK in your project.

## Prerequisites

- .NET 6.0 or later
- Visual Studio 2022 or later (recommended)
- Azure AI Foundry account and API key

## Installation Methods

### NuGet Package

The recommended way to install the SDK is through NuGet Package Manager.

#### Using Visual Studio

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages"
3. Search for "AzureImage"
4. Click "Install"

#### Using .NET CLI

```bash
dotnet add package AzureImage
```

### GitHub Package

If you prefer to use the GitHub package:

1. Create a Personal Access Token (PAT) with `read:packages` scope
2. Configure NuGet to use GitHub packages:

```bash
dotnet nuget add source --username USERNAME --password YOUR_PAT --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DrHazemAli/index.json"
```

3. Install the package:

```bash
dotnet add package AzureImage --source https://nuget.pkg.github.com/DrHazemAli/index.json
```

## Package References

Add the following to your project file:

```xml
<ItemGroup>
    <PackageReference Include="AzureImage" Version="1.0.0" />
</ItemGroup>
```

## Configuration

After installation, you need to configure the SDK in your application. Here's a basic example:

```csharp
// Program.cs or Startup.cs
using AzureImage;
using AzureImage.Extensions;

// For ASP.NET Core applications
services.AddAzureImage(options =>
{
    options.Endpoint = "your-endpoint";
    options.ApiKey = "your-api-key";
});

// For console applications
var client = new AzureImageClient(new AzureImageOptions
{
    Endpoint = "your-endpoint",
    ApiKey = "your-api-key"
});
```

## Environment Variables

You can also configure the SDK using environment variables:

```bash
AZURE_IMAGE_ENDPOINT=your-endpoint
AZURE_IMAGE_API_KEY=your-api-key
```

## Next Steps

- [Quick Start Guide](Quick-Start.md)
- [Configuration Guide](Configuration.md)
- [Basic Usage Examples](../Examples/Basic-Usage.md)

## Troubleshooting

If you encounter any issues during installation:

1. Ensure you have the correct .NET version installed
2. Verify your NuGet package source configuration
3. Check your API key and endpoint configuration
4. Review the [Common Issues](../Troubleshooting/Common-Issues.md) guide

## Support

If you need help with installation:

- [GitHub Issues](https://github.com/DrHazemAli/AzureImageSDK/issues)
- [Documentation](https://github.com/DrHazemAli/AzureImageSDK/wiki)
- [Samples](https://github.com/DrHazemAli/AzureImageSDK/tree/main/samples) 