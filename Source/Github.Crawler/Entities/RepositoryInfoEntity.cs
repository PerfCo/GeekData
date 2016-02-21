using System.Collections.Generic;
using MongoDB.Bson;

namespace Github.Crawler.Entities
{
    public sealed class RepositoryInfoEntity
    {
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public string HtmlUrl { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string StargazersCount { get; set; }
    }
}