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
            var items2 = new string(';', CourseEntity.Captions().Count - 1);
            var items3 = new string(';', UserEntity.Captions().Count - 1);
            return $"{items1};{items2};{items3};{Id}; {_node.Tag} Courses";
        }
    }
}