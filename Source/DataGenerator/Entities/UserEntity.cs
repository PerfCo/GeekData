using System.Collections.Generic;
using System.Linq;
using Contracts.StackOverflow;
using DataGenerator.Nodes;
using MongoDB.Bson;

namespace DataGenerator.Entities
{
    public sealed class UserEntity : Node
    {
        public int AccountId { get; set; }
        public BadgeCounts BadgeCounts { get; set; }
        public string DisplayName { get; set; }

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
            var items1 = new string(';', RepositoryInfoEntity.Captions().Count - 1);
            var items2 = new string(';', CourseEntity.Captions().Count - 1);
            var items3 = new string(';', LibNode.Captions().Count - 1);
            var items4 = new string(';', CourseNode.Captions().Count - 1);
            var items5 = new string(';', GeekNode.Captions().Count - 1);
            var items6 = new string(';', TagNode.Captions().Count - 1);

            return $"{items1}; {items2}; {AccountId}; {BadgeCounts}; {DisplayName}; {Id}; {ProfileImage}; {ProfileUrl}; {string.Join(",", Tags)};" 
                   + $"{items3};{items4};{items5};{items6}; {Level}";
        }

        public override int Level { get; } = 0;
    }
}
