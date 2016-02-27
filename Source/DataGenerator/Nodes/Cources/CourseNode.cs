using System.Collections.Generic;

namespace DataGenerator.Nodes.Cources
{
    public sealed class CourseNode : Node
    {
        private const string Suffix = "CourseNode";
        private readonly NodeRow _node;

        public CourseNode(NodeRow node)
        {
            _node = node;
            Id = $"{node.Tag}{Suffix}";
            Level = 2;
            Label = $"{node.Tag} Courses";
            Tag = node.Tag;
        }

        public override string Id { get; }
        public override string Label { get; }

        public override int Level { get; }
        public List<PluralsightCourseNode> PluralsightCourses { get; } = new List<PluralsightCourseNode>();
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return ToCsvCommon();
        }
    }
}
