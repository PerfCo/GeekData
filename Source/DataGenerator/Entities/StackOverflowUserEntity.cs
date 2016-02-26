using System.Collections.Generic;
using System.Linq;
using Contracts.StackOverflow;
using DataGenerator.Nodes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataGenerator.Entities
{
    [BsonIgnoreExtraElements]
    public sealed class StackOverflowUserEntity : Node
    {
        public int AccountId { get; set; }
        public BadgeCounts BadgeCounts { get; set; }
        public string DisplayName { get; set; }

        public ObjectId Id { get; set; }

        public override string IdNode { get; }
        public override string Label { get; }
        public override int Level { get; } = 3;
        public string ProfileImage { get; set; }
        public string ProfileUrl { get; set; }
        public List<string> Tags { get; set; }

        public static List<string> Captions()
        {
            var suffix = "StackOverflowUser";
            var items = new[] { "AccountId", "BadgeCounts", "DisplayName", "ProfileImage", "ProfileUrl", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            var items1 = new string(';', GithubRepositoryEntity.Captions().Count - 1);
            var items2 = new string(';', PluralsightCourseEntity.Captions().Count - 1);

            var result = new List<object>
            {
                Id,
                DisplayName,
                Level,
                Tags.FirstOrDefault(),
                items1,
                items2,
                AccountId,
                BadgeCounts.ToString(),
                DisplayName,
                ProfileImage,
                ProfileUrl,
                string.Join(",", Tags)
            };
            return string.Join(";", result);
        }
    }
}
