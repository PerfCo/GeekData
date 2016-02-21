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
        public string StargazersCount { get; set; }
        public List<string> Tags { get; set; }

        public new static List<string> Captions()
        {
            var suffix = "GithubRepository";
            var items = new[] { "Description", "HtmlUrl", "Id", "StargazersCount", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            var items1 = new string(';', CourseEntity.Captions().Count - 1);
            var items2 = new string(';', UserEntity.Captions().Count - 1);
            var items3 = new string(';', LibNode.Captions().Count - 1);
            var items4 = new string(';', CourseNode.Captions().Count - 1);
            var items5 = new string(';', GeekNode.Captions().Count - 1);
            var items6 = new string(';', TagNode.Captions().Count - 1);

            return $"{Description}; {HtmlUrl}; {Id}; {StargazersCount}; {string.Join(",", Tags)}; {items1}; {items2};{items3};{items4};{items5};{items6}; {Level}";
        }

        public override int Level { get; } = 0;
    }
}
