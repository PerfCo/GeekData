using System.Collections.Generic;
using System.Linq;
using Nelibur.Sword.Extensions;

namespace DataGenerator.Nodes.Libs
{
    public sealed class LibNode : Node
    {
        private const string Suffix = "LibNode";

        public LibNode(NodeRow node)
        {
            GithubRepositories = node.GithubRepositories.Select(x => x.ToNode()).ToList();
            Level = 2;
            Id = $"{node.Tag}{Suffix}";
            Label = $"{node.Tag} Libs";
            Tag = node.Tag;
            IsEmpty = GithubRepositories.IsEmpty();
        }

        public List<GithubRepositoryNode> GithubRepositories { get; }
        public override string Id { get; }

        public bool IsEmpty { get; }

        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return ToCsvCommon();
        }
    }
}
