using System.Collections.Generic;
using System.Linq;
using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Libs
{
    public sealed class GithubRepositoryNode : Node
    {
        private readonly GithubRepositoryEntity _entity;

        public GithubRepositoryNode(GithubRepositoryEntity entity)
        {
            _entity = entity;
            Level = 3;
            Id = entity.Id.ToString();
            Label = entity.Name;
            Tag = entity.Tags.FirstOrDefault();
        }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public new static List<string> Captions()
        {
            var suffix = "GithubRepository";
            var items = new[] { "Description", "HtmlUrl", "StargazersCount", "Tags" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public List<object> ToCsv()
        {
            List<object> result = ToCsvCommon();
            var node = new List<object>
            {
                _entity.Description,
                _entity.HtmlUrl,
                _entity.StargazersCount,
                string.Join(",", _entity.Tags)
            };
            result.AddRange(node);
            return result;
        }

//        public override string ToString()
//        {
//            var items1 = new string(';', PluralsightCourseEntity.Captions().Count - 1);
//            var items2 = new string(';', StackOverflowUserEntity.Captions().Count - 1);
//
//            var result = new List<object>
//            {
//                Id,
//                Name,
//                Level,
//                Tags.FirstOrDefault(),
//                Description,
//                HtmlUrl,
//                StargazersCount,
//                string.Join(",", Tags),
//                items1,
//                items2
//            };
//            return string.Join(";", result);
//        }
    }
}
