using System.Collections.Generic;
using MongoDB.Bson;

namespace Github.Crawler.Entities
{
    public sealed class RepositoryInfoEntity
    {
        public string Description { get; set; }
        public string FullName { get; set; }
        public string HtmlUrl { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string StargazersCount { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
