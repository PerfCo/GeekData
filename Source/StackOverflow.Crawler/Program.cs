using System;
using System.Collections.Generic;
using Contracts.StackOverflow;
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
            _logger.Info("StackOverflow.Crawler is running. Press ANY key to exit.");
            var worker = new Worker();
            List<StackOverflowUser> result = worker.GetTopUsers("C#");

            SaveUsers(result);
            Console.ReadKey();
        }

        private static void SaveUsers(List<StackOverflowUser> users)
        {
            var repository = _kernel.Get<UserRepository>();
            users.ForEach(repository.Save);
        }
    }
}
