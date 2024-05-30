using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CommentAPI.Dtos
{
    /// <summary>
    /// Dto for Comment
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or Sets ParentId
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [Required]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or Sets ContentId
        /// </summary>
        [Required]
        public string ContentId { get; set; } = null!;

        /// <summary>
        /// Gets or Sets Dispatch
        /// </summary>
        [Required]
        public DateOnly Dispatch { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [Required]
        public string Message { get; set; } = null!;

        /// <summary>
        /// Gets or Sets Replies
        /// </summary>
        public List<string>? Replies { get; set; }
    }
}
