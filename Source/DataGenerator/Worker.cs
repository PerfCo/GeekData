using System;
using System.Collections.Generic;
using System.IO;
using Core;
using DataGenerator.Entities;
using DataGenerator.Properties;
using NLog;

namespace DataGenerator
{
    public sealed class Worker
    {
        private const string NodeFile = "Nodes.csv";
        private const string EdgeFile = "Edges.csv";
        private readonly DataRepository _dataRepository;
        private readonly Settings _settings = Settings.Default;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


        public Worker(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            File.WriteAllLines(NodeFile, new[] { NodeRow.Caption, string.Empty });
            File.WriteAllLines(EdgeFile, new[] { EdgeRow.Caption, string.Empty });
        }

        public void Generate(List<TagItem> rootTags)
        {
            foreach (TagItem tag in rootTags)
            {
                try
                {
                    Generate(tag.StackOverflow);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        private void Generate(string tag)
        {
            List<UserEntity> users = _dataRepository.GetStackOverflowUsers(tag, _settings.TopStackOverflowUsers);
            List<CourseEntity> courses = _dataRepository.GetPluralsightCourses(tag);
            List<RepositoryInfoEntity> repositories = _dataRepository.GetGithubRepositories(tag, _settings.TopGithubRepositories);

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
