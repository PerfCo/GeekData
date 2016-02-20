using System.Collections.Generic;
using System.Linq;
using Contracts.StackOverflow;
using MongoDB.Bson;

namespace DataGenerator.Entities
{
    public sealed class UserEntity
    {
        public int AccountId { get; set; }
        public BadgeCounts BadgeCounts { get; set; }
        public string DisplayName { get; set; }

        public static string Empty => ";;;;;;";
        public ObjectId Id { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileUrl { get; set; }
        public List<string> Tags { get; set; }

        public static List<string> Captions()
        {
            var suffix = "StackOverflowUser";
            var items = new[] { "AccountId", "BadgeCounts", "DisplayName", "Id", "ProfileImage", "ProfileUrl", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            return $"{AccountId}; {BadgeCounts}; {DisplayName}; {Id}; {ProfileImage}; {ProfileUrl}; {Tags}";
        }
    }
}
