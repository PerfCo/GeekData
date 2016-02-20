using System.Collections.Generic;
using Contracts.StackOverflow;
using MongoDB.Bson;

namespace StackOverflow.Crawler.Entities
{
    public sealed class UserEntity
    {
        public int AccountId { get; set; }
        public BadgeCounts BadgeCounts { get; set; }
        public string DisplayName { get; set; }
        public ObjectId Id { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileUrl { get; set; }
        public List<string> Tags { get; set; }
    }
}
