using System.Collections.Generic;
using System.Linq;
using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class CourseNode
    {
        private const string Suffix = "CourseNode";
        private readonly NodeRow _node;

        public CourseNode(NodeRow node)
        {
            _node = node;
            Id = $"{_node.Tag}{Suffix}";
        }

        public string Id { get; }

        public static List<string> Captions()
        {
            var items = new[] { "Id", "Label" };
            List<string> result = items.Select(x => $"{x}{Suffix}").ToList();
            return result;
        }

        public override string ToString()
        {
            var items1 = new string(';', RepositoryInfoEntity.Captions().Count - 1);

            int totalSeparators = RepositoryInfoEntity.Captions().Count
                + CourseEntity.Captions().Count
                + UserEntity.Captions().Count
                + GeekNode.Captions().Count
                + LibNode.Captions().Count
                + TagNode.Captions().Count
                - 6;

            var items = new string(';', totalSeparators);

            return $"{items}; {Id}; {_node.Tag} Courses";
        }
    }
}