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

        public List<GithubRepositoryEntity> GetGithubRepositories(string tag, int topItems)
        {
            try
            {
                List<GithubRepositoryEntity> result = OpenConnection()
                    .GetCollection<GithubRepositoryEntity>(MongoCollection.GithubRepositories)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(topItems)
                    .ToListAsync()
                    .Result;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<GithubRepositoryEntity>();
            }
        }

        public List<PluralsightCourseEntity> GetPluralsightCourses(string tag)
        {
            try
            {
                List<PluralsightCourseEntity> result = OpenConnection()
                    .GetCollection<PluralsightCourseEntity>(MongoCollection.PluralsightCourses)
                    .Find(x => x.Tags.Contains(tag))
                    .ToListAsync()
                    .Result;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<PluralsightCourseEntity>();
            }
        }

        public List<StackOverflowUserEntity> GetStackOverflowUsers(string tag, int topItems)
        {
            try
            {
                List<StackOverflowUserEntity> result = OpenConnection()
                    .GetCollection<StackOverflowUserEntity>(MongoCollection.StackOverflowUsers)
                    .Find(x => x.Tags.Contains(tag))
                    .Limit(topItems)
                    .ToListAsync()
                    .Result;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<StackOverflowUserEntity>();
            }
        }
    }
}
