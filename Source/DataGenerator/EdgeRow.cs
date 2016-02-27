using System.Collections.Generic;
using DataGenerator.Nodes;
using DataGenerator.Nodes.Cources;
using DataGenerator.Nodes.Geeks;
using DataGenerator.Nodes.Libs;
using DataGenerator.Repositories.Entities;
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
                result.Add($"{_tagNode.Id};{_libNode.Id};{EdgeType};{_tagNode.Id}");
            }
            if (_node.PluralsightCourses.IsNotEmpty())
            {
                result.Add($"{_tagNode.Id};{_courseNode.Id};{EdgeType};{_tagNode.Id}");
            }
            if (_node.StackOverflowUsers.IsNotEmpty())
            {
                result.Add($"{_tagNode.Id};{_geekNode.Id};{EdgeType};{_tagNode.Id}");
            }
            result.AddRange(LibLinks());
            result.AddRange(CourcesLinks());
            result.AddRange(GeeksLinks());

            return result;
        }

        private IEnumerable<string> CourcesLinks()
        {
            foreach (PluralsightCourseEntity item in _node.PluralsightCourses)
            {
                yield return $"{_courseNode.Id};{item.Id};{EdgeType};{_tagNode.Id}";
            }
        }

        private IEnumerable<string> GeeksLinks()
        {
            foreach (StackOverflowUserEntity item in _node.StackOverflowUsers)
            {
                yield return $"{_geekNode.Id};{item.Id};{EdgeType};{_tagNode.Id}";
            }
        }

        private IEnumerable<string> LibLinks()
        {
            foreach (GithubRepositoryEntity item in _node.GithubRepositories)
            {
                yield return $"{_libNode.Id};{item.Id};{EdgeType};{_tagNode.Id}";
            }
        }
    }
}
