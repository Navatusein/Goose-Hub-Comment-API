using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CommentAPI.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CommentDto
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string Id { get; set; } = null!;

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
        public DateTime Dispatch { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [Required]
        public string Message { get; set; } = null!;

        /// <summary>
        /// Gets or Sets Thread
        /// </summary>
        public List<CommentDto> Thread { get; set; } = new List<CommentDto>();

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CommentDto {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  Dispatch: ").Append(Dispatch).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  Thread: ").Append(Thread.Select(x => x.ToString())).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
