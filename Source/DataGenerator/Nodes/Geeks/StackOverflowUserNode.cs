using System.Collections.Generic;
using System.Linq;
using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Geeks
{
    public sealed class StackOverflowUserNode : Node
    {
        private readonly StackOverflowUserEntity _entity;

        public StackOverflowUserNode(StackOverflowUserEntity entity)
        {
            _entity = entity;
            Level = 3;
            Id = entity.Id.ToString();
            Label = entity.DisplayName;
            Tag = entity.Tags.FirstOrDefault();
        }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public static List<string> Captions()
        {
            var suffix = "StackOverflowUser";
            var items = new[] { "AccountId", "BadgeCounts", "DisplayName", "ProfileImage", "ProfileUrl", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public List<object> ToCsv()
        {
            List<object> result = ToCsvCommon();
            var node = new List<object>
            {
                _entity.AccountId,
                _entity.BadgeCounts.ToNode().ToCsv(),
                _entity.DisplayName,
                _entity.ProfileImage,
                _entity.ProfileUrl,
                string.Join(",", _entity.Tags)
            };
            result.AddRange(node);
            return result;
        }

//        public override string ToString()
//        {
//            var items1 = new string(';', GithubRepositoryEntity.Captions().Count - 1);
//            var items2 = new string(';', PluralsightCourseEntity.Captions().Count - 1);
//
//            var result = new List<object>
//            {
//                Id,
//                DisplayName,
//                Level,
//                Tags.FirstOrDefault(),
//                items1,
//                items2,
//                AccountId,
//                BadgeCounts.ToString(),
//                DisplayName,
//                ProfileImage,
//                ProfileUrl,
//                string.Join(",", Tags)
//            };
//            return string.Join(";", result);
//        }
    }
}
