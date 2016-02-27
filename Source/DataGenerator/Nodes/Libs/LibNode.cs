namespace DataGenerator.Nodes.Libs
{
    public sealed class LibNode : Node
    {
        private const string Suffix = "LibNode";

        public LibNode(NodeRow node)
        {
            Level = 2;
            Id = $"{node.Tag}{Suffix}";
            Label = $"{node.Tag} Libs";
            Tag = node.Tag;
        }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }
    }
}
