using System.Collections.Generic;
using DataGenerator.Nodes;
using DataGenerator.Nodes.Geeks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataGenerator.Repositories.Entities
{
    [BsonIgnoreExtraElements]
    public sealed class StackOverflowUserEntity
    {
        public int AccountId { get; set; }
        public BadgeCounts BadgeCounts { get; set; }
        public string DisplayName { get; set; }
        public ObjectId Id { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileUrl { get; set; }
        public List<string> Tags { get; set; }

        public StackOverflowUserNode ToNode()
        {
            return new StackOverflowUserNode(this);
        }
    }
}
