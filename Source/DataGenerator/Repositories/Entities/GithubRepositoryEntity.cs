using System.Collections.Generic;
using DataGenerator.Nodes;
using DataGenerator.Nodes.Libs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataGenerator.Repositories.Entities
{
    [BsonIgnoreExtraElements]
    public sealed class GithubRepositoryEntity
    {
        public string Description { get; set; }
        public string HtmlUrl { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string StargazersCount { get; set; }
        public List<string> Tags { get; set; }

        public GithubRepositoryNode ToNode()
        {
            return new GithubRepositoryNode(this);
        }
    }
}
