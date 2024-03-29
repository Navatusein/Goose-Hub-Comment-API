using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CommentAPI.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]
        public string UserId { get; set; }

        [BsonRequired]
        public string ContentId { get; set; }

        [BsonRequired]
        public DateTime Dispatch { get; set; }

        [BsonRequired]
        public string Message { get; set; }

        public List<Comment> Thread { get; set; } = new List<Comment>();
    }
}

