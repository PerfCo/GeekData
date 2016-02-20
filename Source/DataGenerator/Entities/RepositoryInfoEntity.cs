using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace DataGenerator.Entities
{
    public sealed class RepositoryInfoEntity
    {
        public string Description { get; set; }

        public static string Empty => ",,,,";
        public string HtmlUrl { get; set; }
        public ObjectId Id { get; set; }
        public string StargazersCount { get; set; }
        public List<string> Tags { get; set; }

        public static List<string> Captions()
        {
            var suffix = "GithubRepository";
            var items = new[] { "Description", "HtmlUrl", "Id", "StargazersCount", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            return $"{Description}, {HtmlUrl}, {Id}, {StargazersCount}, {Tags}";
        }
    }
}
