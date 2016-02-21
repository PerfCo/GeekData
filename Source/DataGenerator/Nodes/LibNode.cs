using System.Collections.Generic;
using System.Linq;
using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class LibNode : Node
    {
        private const string Suffix = "LibNode";
        private readonly NodeRow _node;

        public LibNode(NodeRow node)
        {
            _node = node;
            Id = $"{_node.Tag}{Suffix}";
        }

        public string Id { get; }

        public override int Level { get; } = 0;

        public new static List<string> Captions()
        {
            var items = new[] { "Id", "Label" };
            List<string> result = items.Select(x => $"{x}{Suffix}").ToList();
            return result;
        }

        //                columns.AddRange(RepositoryInfoEntity.Captions());
        //                columns.AddRange(CourseEntity.Captions());
        //                columns.AddRange(UserEntity.Captions());
        //                columns.AddRange(LibNode.Captions());
        //                columns.AddRange(CourseNode.Captions());
        //                columns.AddRange(GeekNode.Captions());
        //                columns.AddRange(TagNode.Captions());
        public override string ToString()
        {
            var items1 = new string(';', RepositoryInfoEntity.Captions().Count - 1);
            var items2 = new string(';', CourseEntity.Captions().Count - 1);
            var items3 = new string(';', UserEntity.Captions().Count - 1);

            var items4 = new string(';', CourseNode.Captions().Count - 1);
            var items5 = new string(';', GeekNode.Captions().Count - 1);
            var items6 = new string(';', TagNode.Captions().Count - 1);

            return $"{items1}; {items2}; {items3}; {Id}; {_node.Tag} Libs; {items4}; {items5};  {items6}";
        }
    }
}
