using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureImage.Utilities
{
    /// <summary>
    /// Utility class for handling image formats and MIME types.
    /// </summary>
    public static class ImageFormatConverter
    {
        private static readonly Dictionary<string, string> MimeTypeMap = new()
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".webp", "image/webp" },
            { ".tiff", "image/tiff" },
            { ".svg", "image/svg+xml" },
            { ".ico", "image/x-icon" }
        };

        private static readonly Dictionary<string, string> ExtensionMap;

        static ImageFormatConverter()
        {
            // Create reverse mapping for extensions
            ExtensionMap = MimeTypeMap.ToDictionary(
                kvp => kvp.Value,
                kvp => kvp.Key,
                StringComparer.OrdinalIgnoreCase
            );
        }

        /// <summary>
        /// Gets the MIME type for a given file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension (e.g., ".jpg", ".png")</param>
        /// <returns>The corresponding MIME type</returns>
        /// <exception cref="ArgumentException">Thrown when the file extension is not supported</exception>
        public static string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentException("File extension cannot be null or empty", nameof(fileExtension));

            fileExtension = fileExtension.ToLowerInvariant();
            if (!fileExtension.StartsWith("."))
                fileExtension = "." + fileExtension;

            if (!MimeTypeMap.TryGetValue(fileExtension, out string? mimeType) || mimeType == null)
                throw new ArgumentException($"Unsupported file extension: {fileExtension}", nameof(fileExtension));

            return mimeType;
        }

        /// <summary>
        /// Gets the file extension for a given MIME type.
        /// </summary>
        /// <param name="mimeType">The MIME type (e.g., "image/jpeg", "image/png")</param>
        /// <returns>The corresponding file extension</returns>
        /// <exception cref="ArgumentException">Thrown when the MIME type is not supported</exception>
        public static string GetFileExtension(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("MIME type cannot be null or empty", nameof(mimeType));

            if (!ExtensionMap.TryGetValue(mimeType, out string? extension) || extension == null)
                throw new ArgumentException($"Unsupported MIME type: {mimeType}", nameof(mimeType));

            return extension;
        }

        /// <summary>
        /// Checks if a given format is a valid image format.
        /// </summary>
        /// <param name="format">The format to check (can be file extension or MIME type)</param>
        /// <returns>True if the format is supported, false otherwise</returns>
        public static bool IsValidImageFormat(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return false;

            format = format.ToLowerInvariant();
            if (format.StartsWith("."))
                return MimeTypeMap.ContainsKey(format);
            
            return ExtensionMap.ContainsKey(format);
        }

        /// <summary>
        /// Gets an array of all supported image formats.
        /// </summary>
        /// <returns>Array of supported file extensions</returns>
        public static string[] GetSupportedFormats()
        {
            return MimeTypeMap.Keys.ToArray();
        }

        /// <summary>
        /// Gets an array of all supported MIME types.
        /// </summary>
        /// <returns>Array of supported MIME types</returns>
        public static string[] GetSupportedMimeTypes()
        {
            return MimeTypeMap.Values.Distinct().ToArray();
        }
    }
} 