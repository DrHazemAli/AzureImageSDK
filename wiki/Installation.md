# Installation Guide

This guide walks you through installing the Azure Image SDK for .NET.

## Prerequisites

- .NET 6.0 or later
- Azure AI service deployment with API access
- Valid API key for your Azure AI service

## Supported Frameworks

- .NET 6.0+
- .NET 7.0+
- .NET 8.0+

## Installation Methods

### Package Manager Console (Visual Studio)

```powershell
Install-Package AzureImage
```

### .NET CLI

```bash
dotnet add package AzureImage
```

### PackageReference (Manual)

Add this to your `.csproj` file:

```xml
<PackageReference Include="AzureImage" Version="1.0.0" />
```

### GitHub Packages

If you want to use pre-release versions:

```bash
# Add GitHub package source (one-time setup)
dotnet nuget add source https://nuget.pkg.github.com/your-repo/index.json \
    --name github \
    --username YOUR_USERNAME \
    --password YOUR_PAT

# Install pre-release version
dotnet add package AzureImage --version 1.0.0-alpha.* --source github
```

## Verify Installation

Create a simple console application to verify the installation:

```csharp
using AzureImage.Core;
using AzureImage.Inference.Image.StableImageUltra;

namespace VerifyInstallation;

class Program
{
    static void Main(string[] args)
    {
        // Create a model (this doesn't make any API calls)
        var model = StableImageUltraModel.Create(
            endpoint: "https://test.endpoint.com",
            apiKey: "test-key");

        // Create client
        using var client = AzureAIClient.Create();

        Console.WriteLine($"✅ Azure Image SDK installed successfully!");
        Console.WriteLine($"📦 SDK Version: 1.0.0");
        Console.WriteLine($"🎯 Model: {model.ModelName}");
        Console.WriteLine($"🔗 Endpoint: {model.Endpoint}");
    }
}
```

## Next Steps

1. **[Configure your Azure AI service](Configuration.md)** - Set up your endpoints and API keys
2. **[Follow the Quick Start guide](Quick-Start.md)** - Build your first application
3. **[Explore model documentation](models/)** - Learn about specific AI models
4. **[Check out samples](examples/Sample-Projects.md)** - See working examples

## Troubleshooting

### Common Issues

#### Package Not Found
```
error NU1101: Unable to find package 'AzureImage'
```

**Solution**: Ensure you're using a package source that has the package:
- For releases: Use NuGet.org (default)
- For pre-releases: Add GitHub packages source

#### Framework Compatibility
```
error NU1202: Package AzureImage is not compatible with netcoreapp3.1
```

**Solution**: Upgrade to .NET 6.0 or later:
```xml
<TargetFramework>net6.0</TargetFramework>
```

#### Missing Dependencies
If you encounter missing dependencies, restore packages:
```bash
dotnet restore
```

### Getting Help

- 📖 [Documentation](https://github.com/your-repo/AzureImage/wiki)
- 🐛 [Report Issues](https://github.com/your-repo/AzureImage/issues)
- 💬 [Ask Questions](https://github.com/your-repo/AzureImage/discussions) 