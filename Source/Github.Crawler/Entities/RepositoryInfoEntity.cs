using MongoDB.Bson;

namespace Github.Crawler.Entities
{
    public sealed class RepositoryInfoEntity
    {
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public string HtmlUrl { get; set; }
        public string Language { get; set; }
        public string StargazersCount { get; set; }
        public string Readme { get; set; }
    }
}