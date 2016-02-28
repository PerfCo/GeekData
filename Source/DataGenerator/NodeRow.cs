using System.Collections.Generic;
using System.Linq;
using DataGenerator.Nodes;
using DataGenerator.Nodes.Cources;
using DataGenerator.Nodes.Geeks;
using DataGenerator.Nodes.Libs;
using DataGenerator.Repositories.Entities;

namespace DataGenerator
{
    public sealed class NodeRow
    {
        private const string Separator = ";";
        private static string _githubRepositoryEmpty;
        private static string _pluralsightCourseEmpty;
        private static string _stackOverflowUserEmpty;

        public NodeRow(
            string tag,
            List<GithubRepositoryEntity> githubRepositories,
            List<PluralsightCourseEntity> pluralsightCourses,
            List<StackOverflowUserEntity> stackOverflowUsers)
        {
            Tag = tag;
            GithubRepositories = githubRepositories;
            PluralsightCourses = pluralsightCourses;
            StackOverflowUsers = stackOverflowUsers;

            LibNode = new LibNode(this);
            CourseNode = new CourseNode(this);
            GeekNode = new GeekNode(this);
            TagNode = new TagNode(this);

            _githubRepositoryEmpty = new string(Separator[0], GithubRepositoryNode.Headers.Count - 1);
            _pluralsightCourseEmpty = new string(Separator[0], PluralsightCourseNode.Headers.Count - 1);
            _stackOverflowUserEmpty = new string(Separator[0], StackOverflowUserNode.Headers.Count - 1);
        }

        public static string Caption
        {
            get
            {
                var columns = new List<string>();
                columns.AddRange(Node.HeadersCommon);
                columns.AddRange(GithubRepositoryNode.Headers);
                columns.AddRange(PluralsightCourseNode.Headers);
                columns.AddRange(StackOverflowUserNode.Headers);
                return string.Join(Separator, columns);
            }
        }

        public CourseNode CourseNode { get; }
        public GeekNode GeekNode { get; }

        public List<GithubRepositoryEntity> GithubRepositories { get; }
        public LibNode LibNode { get; }
        public List<PluralsightCourseEntity> PluralsightCourses { get; }
        public List<StackOverflowUserEntity> StackOverflowUsers { get; }
        public string Tag { get; }
        public TagNode TagNode { get; }

        public List<string> Value()
        {
            var result = new List<string>();
            result.AddRange(CreateLibRows());
            result.AddRange(CreateCourseRows());
            result.AddRange(CreateGeeksRows());
            result.Add(CreateTagRow());

            return result;
        }

        private List<string> CreateCourseRows()
        {
            var result = new List<string>();

            if (CourseNode.IsEmpty)
            {
                return result;
            }
            result.Add(Join(new[] { Join(CourseNode.ToCsv()), _githubRepositoryEmpty, _pluralsightCourseEmpty, _stackOverflowUserEmpty }));
            result.AddRange(CourseNode.PluralsightCourses.Select(CreatePluralsightCourseRow));
            return result;
        }

        private List<string> CreateGeeksRows()
        {
            var result = new List<string>();

            if (GeekNode.IsEmpty)
            {
                return result;
            }
            result.Add(Join(new[] { Join(GeekNode.ToCsv()), _githubRepositoryEmpty, _pluralsightCourseEmpty, _stackOverflowUserEmpty }));
            result.AddRange(GeekNode.StackOverflowUsers.Select(CreateStackOverflowUserRow));
            return result;
        }

        private static string CreateGithubRepositoryRow(GithubRepositoryNode value)
        {
            return Join(new[]
            {
                Join(value.ToCsvCommon()),
                Join(value.ToCsv()),
                _pluralsightCourseEmpty,
                _stackOverflowUserEmpty
            });
        }

        private List<string> CreateLibRows()
        {
            var result = new List<string>();

            if (LibNode.IsEmpty)
            {
                return result;
            }
            result.Add(Join(new[] { Join(LibNode.ToCsv()), _githubRepositoryEmpty, _pluralsightCourseEmpty, _stackOverflowUserEmpty }));
            result.AddRange(LibNode.GithubRepositories.Select(CreateGithubRepositoryRow));
            return result;
        }

        private static string CreatePluralsightCourseRow(PluralsightCourseNode value)
        {
            return Join(new[]
            {
                Join(value.ToCsvCommon()),
                _githubRepositoryEmpty,
                Join(value.ToCsv()),
                _stackOverflowUserEmpty
            });
        }

        private static string CreateStackOverflowUserRow(StackOverflowUserNode value)
        {
            return Join(new[]
            {
                Join(value.ToCsvCommon()),
                _pluralsightCourseEmpty,
                _stackOverflowUserEmpty,
                Join(value.ToCsv())
            });
        }

        private string CreateTagRow()
        {
            if (LibNode.IsEmpty && CourseNode.IsEmpty && GeekNode.IsEmpty)
            {
                return string.Empty;
            }
            return Join(TagNode.ToCsv());
        }

        private static string Join(IEnumerable<object> value)
        {
            return string.Join(Separator, value);
        }
    }
}
