using System;

namespace AzureImage.Core.Exceptions;

/// <summary>
/// Base exception class for Azure Image SDK
/// </summary>
public class AzureImageException : Exception
{
    /// <summary>
    /// Gets the error code associated with this exception
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Gets the HTTP status code if applicable
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureImageException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    public AzureImageException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureImageException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public AzureImageException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureImageException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="errorCode">The error code</param>
    /// <param name="statusCode">The HTTP status code</param>
    public AzureImageException(string message, string? errorCode = null, int? statusCode = null) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureImageException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    /// <param name="errorCode">The error code</param>
    /// <param name="statusCode">The HTTP status code</param>
    public AzureImageException(string message, Exception innerException, string? errorCode = null, int? statusCode = null) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
} 