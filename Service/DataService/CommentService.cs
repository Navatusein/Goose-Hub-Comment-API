﻿using CommentAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml.Linq;

namespace CommentAPI.Service.DataService
{
    /// <summary>
    /// Comment MongoDB service
    /// </summary>
    public class CommentService
    {
        private static Serilog.ILogger Logger => Serilog.Log.ForContext<CommentService>();

        private readonly IMongoCollection<Comment> _collection;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentService(IConfiguration config, MongoDbConnectionService connectionService)
        {
            var collectionName = config.GetSection("MongoDB:CollectionCommentName").Get<string>();

            _collection = connectionService.Database.GetCollection<Comment>(collectionName);
        }

        /// <summary>
        /// Get Comment
        /// </summary>
        public async Task<Comment> GetAsync(string id)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", id);
            var model = await _collection.Find(filter).FirstOrDefaultAsync();
            return model;
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
        public async Task<List<Comment>> GetCommentsByContentIdAsync(string id)
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
            comment.Id = ObjectId.GenerateNewId().ToString();

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
