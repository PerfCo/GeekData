using System;
using System.Collections.Generic;
using System.IO;
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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DataRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
            File.WriteAllLines(NodeFile,  new[] { ResultRow.Caption, string.Empty });
        }

        public void Test(string tag)
        {
            List<UserEntity> users = GetStackOverflowUsers(tag);
            List<CourseEntity> courses = GetPluralsightCourses(tag);
            List<RepositoryInfoEntity> repositories = GetGithubRepositories(tag);

            var row = new ResultRow
            {
                GithubRepositories = repositories,
                PluralsightCourses = courses,
                StackOverflowUsers = users
            };

            WriteNodes(row);
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

        private static void WriteNodes(ResultRow row)
        {
            File.AppendAllLines(NodeFile, row.Nodes());
        }


        private sealed class ResultRow
        {
            public static string Caption
            {
                get
                {
                    var columns = new List<string>();
                    columns.AddRange(RepositoryInfoEntity.Captions());
                    columns.AddRange(CourseEntity.Captions());
                    columns.AddRange(UserEntity.Captions());
                    return string.Join(";", columns);
                }
            }

            public List<RepositoryInfoEntity> GithubRepositories { get; set; } = new List<RepositoryInfoEntity>();
            public List<CourseEntity> PluralsightCourses { get; set; } = new List<CourseEntity>();
            public List<UserEntity> StackOverflowUsers { get; set; } = new List<UserEntity>();

            public List<string> Nodes()
            {
                var result = new List<string>();
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
}
