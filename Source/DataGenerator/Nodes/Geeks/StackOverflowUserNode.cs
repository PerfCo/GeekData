using System.Collections.Generic;
using System.Linq;
using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Geeks
{
    public sealed class StackOverflowUserNode : Node
    {
        private readonly StackOverflowUserEntity _entity;

        static StackOverflowUserNode()
        {
            var suffix = "StackOverflowUser";
            var items = new[] { "AccountId", "BadgeCounts", "DisplayName", "ProfileImage", "ProfileUrl", "Tags" };
            Headers = items.Select(x => $"{x}{suffix}").ToList();
        }

        public StackOverflowUserNode(StackOverflowUserEntity entity)
        {
            _entity = entity;
            Level = 3;
            Id = entity.Id.ToString();
            Label = entity.DisplayName;
            Tag = entity.Tags.FirstOrDefault();
        }

        public static List<string> Headers { get; }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            var result = new List<object>
            {
                _entity.AccountId,
                _entity.BadgeCounts.ToNode().ToCsv(),
                _entity.DisplayName,
                _entity.ProfileImage,
                _entity.ProfileUrl,
                string.Join(",", _entity.Tags)
            };
            return result;
        }
    }
}
