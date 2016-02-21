using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Core.Extensions;
using DataGenerator.Entities;
using MongoDB.Driver;
using Nelibur.Sword.Extensions;
using NLog;

namespace DataGenerator
{
    public sealed class DataRepository : Repository
    {
        private const int TopUsers = 20;
        private const int TopRepositories = 100;
        private const string NodeFile = "Nodes.csv";
        private const string EdgeFile = "Edges.csv";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DataRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
            File.WriteAllLines(NodeFile, new[] { NodeRow.Caption, string.Empty });
            File.WriteAllLines(EdgeFile, new[] { EdgeRow.Caption, string.Empty });
        }

        public void Test(string tag)
        {
            List<UserEntity> users = GetStackOverflowUsers(tag);
            List<CourseEntity> courses = GetPluralsightCourses(tag);
            List<RepositoryInfoEntity> repositories = GetGithubRepositories(tag);

            var node = new NodeRow(tag)
            {
                GithubRepositories = repositories,
                PluralsightCourses = courses,
                StackOverflowUsers = users
            };

            var edge = new EdgeRow(node);

            WriteEdge(edge);
            WriteNode(node);
        }

        private List<RepositoryInfoEntity> GetGithubRepositories(string tag)
        {
            try
            {
                List<RepositoryInfoEntity> result = OpenConnection()
                    .GetCollection<RepositoryInfoEntity>(MongoCollection.GithubRepositories)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(TopRepositories)
                    .ToListAsync()
                    .Result;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<RepositoryInfoEntity>();
            }
        }

        private List<CourseEntity> GetPluralsightCourses(string tag)
        {
            try
            {
                List<CourseEntity> result = OpenConnection()
                    .GetCollection<CourseEntity>(MongoCollection.PluralsightCourses)
                    .Find(x => x.Tags.Contains(tag))
                    .ToListAsync()
                    .Result;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<CourseEntity>();
            }
        }

        private List<UserEntity> GetStackOverflowUsers(string tag)
        {
            try
            {
                List<UserEntity> result = OpenConnection()
                    .GetCollection<UserEntity>(MongoCollection.StackOverflowUsers)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(TopUsers)
                    .ToListAsync()
                    .Result;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<UserEntity>();
            }
        }

        private static void WriteEdge(EdgeRow row)
        {
            File.AppendAllLines(EdgeFile, row.Value());
        }

        private static void WriteNode(NodeRow row)
        {
            File.AppendAllLines(NodeFile, row.Value());
        }


        private sealed class NodeRow
        {
            public NodeRow(string tag)
            {
                Tag = tag;
                LibNode = new LibNode(this);
                CourseNode = new CourseNode(this);
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
                    return string.Join(";", columns);
                }
            }

            public List<RepositoryInfoEntity> GithubRepositories { get; set; } = new List<RepositoryInfoEntity>();
            public LibNode LibNode { get; }
            public CourseNode CourseNode { get; }
            public List<CourseEntity> PluralsightCourses { get; set; } = new List<CourseEntity>();
            public List<UserEntity> StackOverflowUsers { get; set; } = new List<UserEntity>();
            public string Tag { get; }

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


        private sealed class EdgeRow
        {
            private readonly LibNode _libNode;
            private readonly CourseNode _courseNode;
            private readonly NodeRow _node;

            public EdgeRow(NodeRow node)
            {
                _node = node;
                _libNode = node.LibNode;
                _courseNode = node.CourseNode;
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
                return result;
            }
        }


        private sealed class LibNode
        {
            private const string Suffix = "LibNode";
            private readonly NodeRow _node;

            public LibNode(NodeRow node)
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
                return $"{items1};{items2};{items3};{Id}; {_node.Tag} Libs";
            }
        }

        private sealed class CourseNode
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
}
