using System;
using System.Collections.Generic;
using Core;
using DataGenerator.Entities;
using MongoDB.Driver;
using NLog;

namespace DataGenerator
{
    public sealed class DataRepository : Repository
    {
        private const int TopUsers = 20;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DataRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void Test(string tag)
        {
            List<UserEntity> users = GetStackOverflowUsers(tag);
            List<CourseEntity> courses = GetPluralsightCourses(tag);
            List<RepositoryInfoEntity> repositories = GetGithubRepositories(tag);
        }

        private List<RepositoryInfoEntity> GetGithubRepositories(string tag)
        {
            try
            {
                List<RepositoryInfoEntity> result = OpenConnection()
                    .GetCollection<RepositoryInfoEntity>(MongoCollection.GithubRepositories)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(TopUsers)
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


        private sealed class ResultRow
        {
            public List<CourseEntity> PluralsightCourses { get; set; } = new List<CourseEntity>();
            public List<UserEntity> StackOverflowUsers { get; set; } = new List<UserEntity>();
        }
    }
}
