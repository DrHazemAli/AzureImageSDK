using System;

namespace AzureImage.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when Azure Vision API operations fail.
    /// </summary>
    public class AzureVisionException : AzureImageException
    {
        /// <summary>
        /// Gets the HTTP status code of the failed request, if available.
        /// </summary>
        public int? StatusCode { get; }

        /// <summary>
        /// Gets the raw response content from the Azure Vision API, if available.
        /// </summary>
        public string ResponseContent { get; }

        /// <summary>
        /// Initializes a new instance of the AzureVisionException class.
        /// </summary>
        /// <param name="message">The error message</param>
        public AzureVisionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AzureVisionException class.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception</param>
        public AzureVisionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AzureVisionException class.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="responseContent">The raw response content</param>
        public AzureVisionException(string message, int statusCode, string responseContent) : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        /// <summary>
        /// Initializes a new instance of the AzureVisionException class.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="responseContent">The raw response content</param>
        /// <param name="innerException">The inner exception</param>
        public AzureVisionException(string message, int statusCode, string responseContent, Exception innerException) 
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
} 