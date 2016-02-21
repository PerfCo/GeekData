using System.Collections.Generic;
using Core.Extensions;
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
                columns.AddRange(RepositoryInfoEntity.Captions());
                columns.AddRange(CourseEntity.Captions());
                columns.AddRange(UserEntity.Captions());
                columns.AddRange(LibNode.Captions());
                columns.AddRange(CourseNode.Captions());
                columns.AddRange(GeekNode.Captions());
                columns.AddRange(TagNode.Captions());
                return string.Join(";", columns);
            }
        }

        public CourseNode CourseNode { get; }
        public GeekNode GeekNode { get; }

        public List<RepositoryInfoEntity> GithubRepositories { get; set; } = new List<RepositoryInfoEntity>();
        public LibNode LibNode { get; }
        public List<CourseEntity> PluralsightCourses { get; set; } = new List<CourseEntity>();
        public List<UserEntity> StackOverflowUsers { get; set; } = new List<UserEntity>();
        public string Tag { get; }
        public TagNode TagNode { get; }

        public List<string> Value()
        {
            var result = new List<string> { TagNode.ToString() };

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

            while (GithubRepositories.IsNotEmpty() || PluralsightCourses.IsNotEmpty() || StackOverflowUsers.IsNotEmpty())
            {
                RepositoryInfoEntity repository = GithubRepositories.TakeFirst();
                string repositoryValue = repository.IsNull() ? RepositoryInfoEntity.Empty : repository.ToString();

                CourseEntity course = PluralsightCourses.TakeFirst();
                string courseValue = course.IsNull() ? CourseEntity.Empty : course.ToString();

                UserEntity user = StackOverflowUsers.TakeFirst();
                string userValue = user.IsNull() ? UserEntity.Empty : user.ToString();

                result.Add($"{repositoryValue}; {courseValue}; {userValue}");
            }

            return result;
        }
    }
}
