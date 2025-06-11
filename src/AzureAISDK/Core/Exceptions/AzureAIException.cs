using System;

namespace AzureAISDK.Core.Exceptions;

/// <summary>
/// Base exception class for Azure AI SDK
/// </summary>
public class AzureAIException : Exception
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
    /// Initializes a new instance of the <see cref="AzureAIException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    public AzureAIException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureAIException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public AzureAIException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureAIException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="errorCode">The error code</param>
    /// <param name="statusCode">The HTTP status code</param>
    public AzureAIException(string message, string? errorCode = null, int? statusCode = null) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureAIException"/> class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    /// <param name="errorCode">The error code</param>
    /// <param name="statusCode">The HTTP status code</param>
    public AzureAIException(string message, Exception innerException, string? errorCode = null, int? statusCode = null) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
} 