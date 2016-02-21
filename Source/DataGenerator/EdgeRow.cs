using System.Collections.Generic;
using DataGenerator.Entities;
using Nelibur.Sword.Extensions;

namespace DataGenerator.Nodes
{
    public sealed class EdgeRow
    {
        private readonly CourseNode _courseNode;
        private readonly GeekNode _geekNode;
        private readonly LibNode _libNode;
        private readonly NodeRow _node;
        private readonly TagNode _tagNode;

        public EdgeRow(NodeRow node)
        {
            _node = node;
            _libNode = node.LibNode;
            _courseNode = node.CourseNode;
            _geekNode = node.GeekNode;
            _tagNode = node.TagNode;
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
            if (_node.GithubRepositories.IsNotEmpty())
            {
                result.Add($"{_tagNode.Id}; {_libNode.Id}");
            }
            if (_node.GithubRepositories.IsNotEmpty())
            {
                result.Add($"{_tagNode.Id}; {_courseNode.Id}");
            }
            if (_node.GithubRepositories.IsNotEmpty())
            {
                result.Add($"{_tagNode.Id}; {_libNode.Id}");
            }
            result.AddRange(LibLinks());
            result.AddRange(CourcesLinks());
            result.AddRange(GeeksLinks());

            return result;
        }

        private IEnumerable<string> CourcesLinks()
        {
            foreach (CourseEntity item in _node.PluralsightCourses)
            {
                yield return $"{_courseNode.Id}; {item.Id}";
            }
        }

        private IEnumerable<string> GeeksLinks()
        {
            foreach (UserEntity item in _node.StackOverflowUsers)
            {
                yield return $"{_geekNode.Id}; {item.Id}";
            }
        }

        private IEnumerable<string> LibLinks()
        {
            foreach (RepositoryInfoEntity item in _node.GithubRepositories)
            {
                yield return $"{_libNode.Id}; {item.Id}";
            }
        }
    }
}
