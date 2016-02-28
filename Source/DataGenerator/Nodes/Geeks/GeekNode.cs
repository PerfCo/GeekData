using System.Collections.Generic;
using System.Linq;
using Nelibur.Sword.Extensions;

namespace DataGenerator.Nodes.Geeks
{
    public sealed class GeekNode : Node
    {
        private const string Suffix = "GeekNode";

        public GeekNode(NodeRow node)
        {
            StackOverflowUsers = node.StackOverflowUsers.Select(x => x.ToNode()).ToList();
            Id = $"{node.Tag}{Suffix}";
            Level = 2;
            Label = $"{node.Tag} Geeks";
            Tag = node.Tag;
            IsEmpty = StackOverflowUsers.IsEmpty();
        }

        public override string Id { get; }

        public bool IsEmpty { get; }

        public override string Label { get; }
        public override int Level { get; }

        public List<StackOverflowUserNode> StackOverflowUsers { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return ToCsvCommon();
        }
    }
}
