using System.Collections.Generic;
using System.Linq;
using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Libs
{
    public sealed class GithubRepositoryNode : Node
    {
        private readonly GithubRepositoryEntity _entity;

        static GithubRepositoryNode()
        {
            var suffix = "GithubRepository";
            var items = new[] { "Description", "HtmlUrl", "StargazersCount", "Tags" };
            Headers = items.Select(x => $"{x}{suffix}").ToList();
        }

        public GithubRepositoryNode(GithubRepositoryEntity entity)
        {
            _entity = entity;
            Level = 3;
            Id = entity.Id.ToString();
            Label = entity.Name;
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
                _entity.Description,
                _entity.HtmlUrl,
                _entity.StargazersCount,
                string.Join(",", _entity.Tags)
            };
            return result;
        }
    }
}
