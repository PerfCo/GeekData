using System.Collections.Generic;
using DataGenerator.Nodes;
using DataGenerator.Nodes.Cources;
using DataGenerator.Nodes.Geeks;
using DataGenerator.Nodes.Libs;

namespace DataGenerator
{
    public sealed class EdgeRow
    {
        private const string EdgeType = "Directed";

        private readonly CourseNode _courseNode;
        private readonly GeekNode _geekNode;
        private readonly LibNode _libNode;
        private readonly TagNode _tagNode;

        public EdgeRow(NodeRow node)
        {
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
            if (_libNode.IsEmpty == false)
            {
                result.Add($"{_tagNode.Id};{_libNode.Id};{EdgeType};{_tagNode.Id}");
            }
            if (_courseNode.IsEmpty == false)
            {
                result.Add($"{_tagNode.Id};{_courseNode.Id};{EdgeType};{_tagNode.Id}");
            }
            if (_geekNode.IsEmpty == false)
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
            foreach (PluralsightCourseNode item in _courseNode.PluralsightCourses)
            {
                yield return $"{_courseNode.Id};{item.Id};{EdgeType};{_tagNode.Id}";
            }
        }

        private IEnumerable<string> GeeksLinks()
        {
            foreach (StackOverflowUserNode item in _geekNode.StackOverflowUsers)
            {
                yield return $"{_geekNode.Id};{item.Id};{EdgeType};{_tagNode.Id}";
            }
        }

        private IEnumerable<string> LibLinks()
        {
            foreach (GithubRepositoryNode item in _libNode.GithubRepositories)
            {
                yield return $"{_libNode.Id};{item.Id};{EdgeType};{_tagNode.Id}";
            }
        }
    }
}
