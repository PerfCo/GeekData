using System;
using System.Collections.Generic;
using System.IO;
using Core;
using DataGenerator.Entities;
using MongoDB.Driver;
using NLog;

namespace DataGenerator
{
    public sealed class Worker : Repository
    {
        private const int TopUsers = 20;
        private const int TopRepositories = 200;
//        private const int TopCources = 3;
        private const string NodeFile = "Nodes.csv";
        private const string EdgeFile = "Edges.csv";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Worker(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
            File.WriteAllLines(NodeFile, new[] { NodeRow.Caption, string.Empty });
            File.WriteAllLines(EdgeFile, new[] { EdgeRow.Caption, string.Empty });
        }

        public void Generate(string tag)
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
//                    .Limit(TopCources)
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
    }
}
