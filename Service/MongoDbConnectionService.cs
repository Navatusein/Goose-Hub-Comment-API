using MongoDB.Driver;

namespace CommentAPI.Service
{
    /// <summary>
    /// Service for MongoDb connection
    /// </summary>
    public class MongoDbConnectionService
    {
        private static Serilog.ILogger Logger => Serilog.Log.ForContext<MongoDbConnectionService>();
        private readonly IConfiguration _config;

        /// <summary>
        /// Gets or private Sets Client
        /// </summary>
        public IMongoClient Client { get; private set; }

        /// <summary>
        /// Gets or private Sets Database
        /// </summary>
        public IMongoDatabase Database { get; private set; }

        /// <summary>
        /// Сonstructor of class MongoDbConnectionService
        /// </summary>
        public MongoDbConnectionService(IConfiguration config)
        {
            _config = config;

            var connectionString = _config.GetSection("MongoDB:ConnectionURI").Value;
            var databaseName = _config.GetSection("MongoDB:DatabaseName").Value;

            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }
    }
}
