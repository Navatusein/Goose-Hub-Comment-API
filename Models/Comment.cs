using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using CommentAPI.Dtos;

namespace CommentAPI.Models
{
    /// <summary>
    /// Model for store user comments
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


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
        /// Gets or Sets Thread
        /// </summary>
        public List<CommentDto> Thread { get; set; } = new List<CommentDto>();
    }
}

