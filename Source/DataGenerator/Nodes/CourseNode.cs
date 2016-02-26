using System.Collections.Generic;
using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class CourseNode : Node
    {
        private const string Suffix = "CourseNode";
        private readonly NodeRow _node;

        public CourseNode(NodeRow node)
        {
            _node = node;
            IdNode = $"{node.Tag}{Suffix}";
            Label = $"{node.Tag} Courses";
        }

        public override string IdNode { get; }
        public override string Label { get; }

        public override int Level { get; } = 2;
        public List<PluralsightCourseEntity> PluralsightCourses { get; } = new List<PluralsightCourseEntity>();

        public override string ToString()
        {
            var items1 = new string(';', GithubRepositoryEntity.Captions().Count - 1);
            var items2 = new string(';', PluralsightCourseEntity.Captions().Count - 1);
            var items3 = new string(';', StackOverflowUserEntity.Captions().Count - 1);

            return $"{IdNode};{Label};{Level};{_node.Tag};{items1};{items2};{items3}";
        }
    }
}
