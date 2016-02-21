using System.Collections.Generic;
using System.Linq;
using DataGenerator.Nodes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataGenerator.Entities
{
    [BsonIgnoreExtraElements]
    public sealed class RepositoryInfoEntity : Node
    {
        public string Description { get; set; }
        public string HtmlUrl { get; set; }
        public ObjectId Id { get; set; }

        public override string IdNode { get; }
        public override string Label { get; }
        public override int Level { get; } = 3;
        public string Name { get; set; }
        public string StargazersCount { get; set; }
        public List<string> Tags { get; set; }

        public new static List<string> Captions()
        {
            var suffix = "GithubRepository";
            var items = new[] { "Description", "HtmlUrl", "StargazersCount", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            var items1 = new string(';', CourseEntity.Captions().Count - 1);
            var items2 = new string(';', UserEntity.Captions().Count - 1);

            var result = new List<object>
            {
                Id,
                Name,
                Level,
                Description,
                HtmlUrl,
                StargazersCount,
                string.Join(",", Tags),
                items1,
                items2
            };
            return string.Join(";", result);
        }
    }
}
