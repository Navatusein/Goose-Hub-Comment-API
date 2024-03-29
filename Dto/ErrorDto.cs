using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CommentAPI.Dto
{
    /// <summary>
    /// Model for errors
    /// </summary>
    public class ErrorDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorDto(string message, string code)
        {
            Id = Guid.NewGuid().ToString();
            Message = message;
            Code = code;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorDto()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [Required]
        public string Message { get; set; } = null!;

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [Required]
        public string Code { get; set; } = null!;
    }
}