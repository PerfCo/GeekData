using System.Collections.Generic;
using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class EdgeRow
    {
        private readonly LibNode _libNode;
        private readonly CourseNode _courseNode;
        private readonly GeekNode _geekNode;
        private readonly NodeRow _node;

        public EdgeRow(NodeRow node)
        {
            _node = node;
            _libNode = node.LibNode;
            _courseNode = node.CourseNode;
            _geekNode = node.GeekNode;
        }

        public static string Caption
        {
            get
            {
                var items = new[] { "Source", "Target" };
                return string.Join(";", items);
            }
        }

        public List<string> Value()
        {
            var result = new List<string>();
            foreach (RepositoryInfoEntity item in _node.GithubRepositories)
            {
                result.Add($"{_libNode.Id}; {item.Id}");
            }
            foreach (CourseEntity item in _node.PluralsightCourses)
            {
                result.Add($"{_courseNode.Id}; {item.Id}");
            }
            foreach (UserEntity item in _node.StackOverflowUsers)
            {
                result.Add($"{_geekNode.Id}; {item.Id}");
            }
            return result;
        }
    }
}