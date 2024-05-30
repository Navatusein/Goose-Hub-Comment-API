using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace CommentAPI.MassTransit.Responses
{
    /// <summary>
    /// Model for response on AnimeAddContentEvent
    /// </summary>
    [MessageUrn("ContentExistResponse")]
    public class ContentExistResponse
    {
        /// <summary>
        /// Gets or Sets ContentId
        /// </summary>
        [Required]
        public string ContentId { get; set; } = null!;

        /// <summary>
        /// Gets or Sets ContentId
        /// </summary>
        [Required]
        public bool IsExists { get; set; }
    }
}
