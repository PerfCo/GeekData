using Contracts.Github;
using Core;
using Github.Crawler.Entities;
using MongoDB.Bson;
using Nelibur.ObjectMapper;

namespace Github.Crawler
{
    public sealed class GithubRepository : Repository
    {
        public GithubRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void Save(GithubRepositoryInfo value)
        {
            var entity = TinyMapper.Map<RepositoryInfoEntity>(value);
            entity.Id = ObjectId.GenerateNewId();
            if (!string.IsNullOrWhiteSpace(value.Language))
            {
                entity.Tags.Add(value.Language);
            }
            entity.Description = RemoveSeparator(entity.Description);
            GetCollection<RepositoryInfoEntity>(MongoCollection.GithubRepositories).InsertOneAsync(entity).Wait();
        }
    }
}
