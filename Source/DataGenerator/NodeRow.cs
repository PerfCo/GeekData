using System.Collections.Generic;
using System.Linq;
using DataGenerator.Entities;
using DataGenerator.Nodes;
using Nelibur.Sword.Extensions;

namespace DataGenerator
{
    public sealed class NodeRow
    {
        public NodeRow(string tag)
        {
            Tag = tag;
            LibNode = new LibNode(this);
            CourseNode = new CourseNode(this);
            GeekNode = new GeekNode(this);
            TagNode = new TagNode(this);
        }

        public static string Caption
        {
            get
            {
                var columns = new List<string>();
                columns.AddRange(Node.Captions());
                columns.AddRange(GithubRepositoryEntity.Captions());
                columns.AddRange(PluralsightCourseEntity.Captions());
                columns.AddRange(StackOverflowUserEntity.Captions());
                return string.Join(";", columns);
            }
        }

        public CourseNode CourseNode { get; }
        public GeekNode GeekNode { get; }

        public List<GithubRepositoryEntity> GithubRepositories { get; set; } = new List<GithubRepositoryEntity>();
        public LibNode LibNode { get; }
        public List<PluralsightCourseEntity> PluralsightCourses { get; set; } = new List<PluralsightCourseEntity>();
        public List<StackOverflowUserEntity> StackOverflowUsers { get; set; } = new List<StackOverflowUserEntity>();
        public string Tag { get; }
        public TagNode TagNode { get; }

        public List<string> Value()
        {
            var result = new List<string>();

            if (GithubRepositories.IsNotEmpty())
            {
                result.Add(LibNode.ToString());
            }
            if (PluralsightCourses.IsNotEmpty())
            {
                result.Add(CourseNode.ToString());
            }
            if (StackOverflowUsers.IsNotEmpty())
            {
                result.Add(GeekNode.ToString());
            }
            result.Add(TagNode.ToString());

            result.AddRange(GithubRepositories.Select(x => x.ToString()));
            result.AddRange(PluralsightCourses.Select(x => x.ToString()));
            result.AddRange(StackOverflowUsers.Select(x => x.ToString()));

            return result;
        }

    }
}
