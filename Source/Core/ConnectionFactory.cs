using MongoDB.Driver;

namespace Core
{
    public sealed class ConnectionFactory
    {
        private readonly IMongoDatabase _database;

        public ConnectionFactory(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public IMongoDatabase OpenConnection()
        {
            return _database;
        }
    }
}
