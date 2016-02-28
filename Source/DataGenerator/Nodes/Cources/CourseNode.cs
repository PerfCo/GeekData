using System.Collections.Generic;
using System.Linq;
using Nelibur.Sword.Extensions;

namespace DataGenerator.Nodes.Cources
{
    public sealed class CourseNode : Node
    {
        private const string Suffix = "CourseNode";

        public CourseNode(NodeRow node)
        {
            PluralsightCourses = node.PluralsightCourses.Select(x => x.ToNode()).ToList();
            Id = $"{node.Tag}{Suffix}";
            Level = 2;
            Label = $"{node.Tag} Courses";
            Tag = node.Tag;
            IsEmpty = PluralsightCourses.IsEmpty();
        }

        public override string Id { get; }
        public bool IsEmpty { get; }
        public override string Label { get; }

        public override int Level { get; }
        public List<PluralsightCourseNode> PluralsightCourses { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return ToCsvCommon();
        }
    }
}
