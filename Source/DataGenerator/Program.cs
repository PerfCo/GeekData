using System;
using System.Collections.Generic;
using Core;
using DataGenerator.Properties;
using NLog;

namespace DataGenerator
{
    class Program
    {
        private static readonly ConnectionFactory _connectionFactory = new ConnectionFactory(Settings.Default.MongoDbConnectionString, Settings.Default.DatabaseName);
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            _logger.Info("DataGenerator is running...");
            List<TagItem> rootTags = new Tags().Root;
            var worker = new Worker(_connectionFactory);
            foreach (TagItem tag in rootTags)
            {
                try
                {
                    worker.Generate(tag.StackOverflow);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            _logger.Info("Press ANY key to exit.");
            Console.ReadKey();
        }
    }
}
