using System.Collections.Generic;
using System.Linq;
using DataGenerator.Nodes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataGenerator.Entities
{
    [BsonIgnoreExtraElements]
    public sealed class PluralsightCourseEntity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Url { get; set; }

        public static List<string> Captions()
        {
            var suffix = "PluralsightCourse";
            var items = new[] { "Name", "Tags", "Url" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            var items1 = new string(';', GithubRepositoryEntity.Captions().Count - 1);
            var items2 = new string(';', StackOverflowUserEntity.Captions().Count - 1);

            var result = new List<object>
            {
                Id,
                Name,
                Level,
                Tags.FirstOrDefault(),
                items1,
                Name,
                string.Join(",", Tags),
                Url,
                items2
            };
            return string.Join(";", result);
        }

        public PluralsightCourseNode ToNode()
        {
            return new PluralsightCourseNode(this);
        }
    }
}
