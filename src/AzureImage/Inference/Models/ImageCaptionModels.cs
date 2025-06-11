using System.Collections.Generic;

namespace AzureImage.Inference.Models
{
    /// <summary>
    /// Options for configuring image captioning requests.
    /// </summary>
    public class ImageCaptionOptions
    {
        /// <summary>
        /// Gets or sets the language for the generated captions. Default is "en" (English).
        /// </summary>
        public string Language { get; set; } = "en";

        /// <summary>
        /// Gets or sets whether to use gender-neutral captions. 
        /// When true, gender-specific terms like "man/woman" are replaced with "person".
        /// </summary>
        public bool GenderNeutralCaption { get; set; } = false;

        /// <summary>
        /// Gets or sets the maximum number of dense captions to generate (1-10).
        /// Only applicable for dense captioning. Default is 10.
        /// </summary>
        public int MaxDenseCaptions { get; set; } = 10;
    }

    /// <summary>
    /// Represents the result of an image captioning operation.
    /// </summary>
    public class ImageCaptionResult
    {
        /// <summary>
        /// Gets or sets the generated caption.
        /// </summary>
        public Caption Caption { get; set; }

        /// <summary>
        /// Gets or sets the model version used for captioning.
        /// </summary>
        public string ModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the metadata about the analyzed image.
        /// </summary>
        public ImageMetadata Metadata { get; set; }
    }

    /// <summary>
    /// Represents the result of a dense captioning operation.
    /// </summary>
    public class DenseCaptionResult
    {
        /// <summary>
        /// Gets or sets the list of dense captions for different regions.
        /// </summary>
        public List<DenseCaption> Captions { get; set; } = new List<DenseCaption>();

        /// <summary>
        /// Gets or sets the model version used for captioning.
        /// </summary>
        public string ModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the metadata about the analyzed image.
        /// </summary>
        public ImageMetadata Metadata { get; set; }
    }

    /// <summary>
    /// Represents a single caption with confidence score.
    /// </summary>
    public class Caption
    {
        /// <summary>
        /// Gets or sets the caption text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the confidence score (0.0 to 1.0).
        /// </summary>
        public double Confidence { get; set; }
    }

    /// <summary>
    /// Represents a dense caption with bounding box coordinates.
    /// </summary>
    public class DenseCaption : Caption
    {
        /// <summary>
        /// Gets or sets the bounding box coordinates for this caption's region.
        /// </summary>
        public BoundingBox BoundingBox { get; set; }
    }

    /// <summary>
    /// Represents a rectangular bounding box.
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the top-left corner.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the top-left corner.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the bounding box.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the bounding box.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Returns a string representation of the bounding box.
        /// </summary>
        public override string ToString()
        {
            return $"X={X}, Y={Y}, Width={Width}, Height={Height}";
        }
    }

    /// <summary>
    /// Represents metadata about an image.
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// Gets or sets the width of the image in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the image in pixels.
        /// </summary>
        public int Height { get; set; }
    }
} 