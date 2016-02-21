using System.Collections.Generic;
using DataGenerator.Entities;
using DataGenerator.Nodes;
using Nelibur.Sword.Extensions;

namespace DataGenerator
{
    public sealed class EdgeRow
    {
        private const string EdgeType = "Directed";

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
                var items = new[] { "Source", "Target", "Type", "Tag" };
                return string.Join(";", items);
            }
        }

        public List<string> Value()
        {
            var result = new List<string>();
            if (_node.GithubRepositories.IsNotEmpty())
            {
                result.Add($"{_tagNode.IdNode};{_libNode.IdNode};{EdgeType};{_tagNode.IdNode}");
            }
            if (_node.PluralsightCourses.IsNotEmpty())
            {
                result.Add($"{_tagNode.IdNode};{_courseNode.IdNode};{EdgeType};{_tagNode.IdNode}");
            }
            if (_node.StackOverflowUsers.IsNotEmpty())
            {
                result.Add($"{_tagNode.IdNode};{_geekNode.IdNode};{EdgeType};{_tagNode.IdNode}");
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
                yield return $"{_courseNode.IdNode};{item.Id};{EdgeType};{_tagNode.IdNode}";
            }
        }

        private IEnumerable<string> GeeksLinks()
        {
            foreach (UserEntity item in _node.StackOverflowUsers)
            {
                yield return $"{_geekNode.IdNode};{item.Id};{EdgeType};{_tagNode.IdNode}";
            }
        }

        private IEnumerable<string> LibLinks()
        {
            foreach (RepositoryInfoEntity item in _node.GithubRepositories)
            {
                yield return $"{_libNode.IdNode};{item.Id};{EdgeType};{_tagNode.IdNode}";
            }
        }
    }
}
