using CommentAPI.Dto;
using CommentAPI.Models;
using MongoDB.Driver;

namespace CommentAPI.Service.DataService
{
    public class CommentService
    {
        private static Serilog.ILogger Logger=>Serilog.Log.ForContext<CommentService>();

        private readonly IMongoCollection<Comment> _collection;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentService(IConfiguration config, MongoDbConnectionService connectionService)
        {
            var collectionName = config.GetSection("MongoDB:CollectionContentName").Get<string>();

            _collection = connectionService.Database.GetCollection<Comment>(collectionName);
        }

        /// <summary>
        /// Delete Comment
        /// </summary>
        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", id);
            var model = await _collection.FindOneAndDeleteAsync(filter);
            return model != null;
        }

        /// <summary>
        /// Get Comments
        /// </summary>
        public async Task<List<Comment>> GetCommentsAsync(string id)
        {
            var filter = Builders<Comment>.Filter.Eq("ContentId", id);
            var result = await _collection.Find(filter).ToListAsync();
            return result;

        }

        /// <summary>
        /// Add Comment
        /// </summary>
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            
            await _collection.InsertOneAsync(comment);

            return comment;

        }


        /// <summary>
        /// Reply Comment
        /// </summary>
        public async Task<Comment> ReplyCommentAsync(string id, Comment comment)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", id);
            var update = Builders<Comment>.Update.Push("Thread", comment);

            var options = new FindOneAndUpdateOptions<Comment>
            {
                ReturnDocument = ReturnDocument.After
            };

            var model = await _collection.FindOneAndUpdateAsync(filter, update, options);

            return model;
        }

        /// <summary>
        /// Get User Comments
        /// </summary>
        public async Task<List<Comment>> GetCommentsByUserIdAsync(string id)
        {
            var filter = Builders<Comment>.Filter.Eq("UserId", id);
            var result = await _collection.Find(filter).ToListAsync();
            return result;

        }
    }
}
