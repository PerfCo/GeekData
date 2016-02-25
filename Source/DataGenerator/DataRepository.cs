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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public DataRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public List<RepositoryInfoEntity> GetGithubRepositories(string tag, int topItems)
        {
            try
            {
                List<RepositoryInfoEntity> result = OpenConnection()
                    .GetCollection<RepositoryInfoEntity>(MongoCollection.GithubRepositories)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(topItems)
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

        public List<CourseEntity> GetPluralsightCourses(string tag)
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

        public List<UserEntity> GetStackOverflowUsers(string tag, int topItems)
        {
            try
            {
                List<UserEntity> result = OpenConnection()
                    .GetCollection<UserEntity>(MongoCollection.StackOverflowUsers)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(topItems)
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
    }
}
