using System.Collections.Generic;

namespace DataGenerator.Nodes.Geeks
{
    public sealed class GeekNode : Node
    {
        private const string Suffix = "GeekNode";

        public GeekNode(NodeRow node)
        {
            Id = $"{node.Tag}{Suffix}";
            Level = 2;
            Label = $"{node.Tag} Geeks";
            Tag = node.Tag;
        }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }

        public List<StackOverflowUserNode> StackOverflowUsers { get; } = new List<StackOverflowUserNode>();
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return ToCsvCommon();
        }
    }
}
