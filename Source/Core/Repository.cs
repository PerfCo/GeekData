using MongoDB.Driver;

namespace Core
{
    public abstract class Repository
    {
        private readonly ConnectionFactory _connectionFactory;

        protected Repository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return OpenConnection().GetCollection<T>(collectionName);
        }

        protected IMongoDatabase OpenConnection()
        {
            return _connectionFactory.OpenConnection();
        }
    }
}
