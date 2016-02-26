using System;
using System.Collections.Generic;
using Contracts.Github;
using Core;
using Github.Crawler.Dependencies;
using Ninject;
using NLog;

namespace Github.Crawler
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly IKernel _kernel = new StandardKernel(new DependencyModule());

        static void Main()
        {
            _logger.Info("Github.Crawler is running...");

            foreach (TagItem tag in Tags.Root.Value)
            {
                try
                {
                    List<GithubRepositoryInfo> result = new Worker().GetRepositories(tag.GithubSearchValue, tag.GithubLanguage);
                    SaveRepositories(result);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            _logger.Info("Press ANY key to exit.");
            Console.ReadKey();
        }

        private static void SaveRepositories(List<GithubRepositoryInfo> repositories)
        {
            var repository = _kernel.Get<GithubRepository>();
            repositories.ForEach(repository.Save);
        }
    }
}
