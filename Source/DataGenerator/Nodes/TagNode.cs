using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class TagNode : Node
    {
        private readonly NodeRow _node;
        private const string Suffix = "TagNode";

        public TagNode(NodeRow node)
        {
            _node = node;
            IdNode = $"{node.Tag}{Suffix}";
            Label = $"{node.Tag}";
        }

        public override string IdNode { get; }
        public override string Label { get; }
        public override int Level { get; } = 1;

        public override string ToString()
        {
            var items1 = new string(';', GithubRepositoryEntity.Captions().Count - 1);
            var items2 = new string(';', PluralsightCourseEntity.Captions().Count - 1);
            var items3 = new string(';', StackOverflowUserEntity.Captions().Count - 1);

            return $"{IdNode};{Label};{Level};{_node.Tag};{items1};{items2};{items3}";
        }
    }
}
