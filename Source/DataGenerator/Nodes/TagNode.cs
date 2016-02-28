using System.Collections.Generic;

namespace DataGenerator.Nodes
{
    public sealed class TagNode : Node
    {
        private const string Suffix = "TagNode";

        public TagNode(NodeRow node)
        {
            Level = 1;
            Id = $"{node.Tag}{Suffix}";
            Label = $"{node.Tag}";
            Tag = node.Tag;
        }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return ToCsvCommon();
        }
    }
}
