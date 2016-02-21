using System;
using System.Collections.Generic;
using Contracts.StackOverflow;
using Core;
using Ninject;
using NLog;
using StackOverflow.Crawler.Dependencies;

namespace StackOverflow.Crawler
{
    internal class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly IKernel _kernel = new StandardKernel(new DependencyModule());

        private static void Main()
        {
            _logger.Info("StackOverflow.Crawler is running...");
            var worker = new Worker();
            List<TagItem> rootTags = new Tags().Root;
            foreach (TagItem tag in rootTags)
            {
                try
                {
                    List<StackOverflowUser> result = worker.GetTopUsers(tag.StackOverflow);
                    SaveUsers(result);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            _logger.Info("Press ANY key to exit.");
            Console.ReadKey();
        }

        private static void SaveUsers(List<StackOverflowUser> users)
        {
            var repository = _kernel.Get<UserRepository>();
            users.ForEach(repository.Save);
        }
    }
}
