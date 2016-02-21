using Contracts.StackOverflow;
using Core;
using MongoDB.Bson;
using Nelibur.ObjectMapper;
using StackOverflow.Crawler.Entities;

namespace StackOverflow.Crawler
{
    public sealed class UserRepository : Repository
    {

        public UserRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void Save(StackOverflowUser value)
        {
            var entity = TinyMapper.Map<UserEntity>(value);
            entity.Id = ObjectId.GenerateNewId();
            entity.DisplayName = RemoveSeparator(entity.DisplayName);
            GetCollection<UserEntity>(MongoCollection.StackOverflowUsers).InsertOneAsync(entity).Wait();
        }
    }
}
