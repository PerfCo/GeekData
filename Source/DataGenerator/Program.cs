using System;
using System.Collections.Generic;
using Core;
using DataGenerator.Dependencies;
using Ninject;
using NLog;

namespace DataGenerator
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly IKernel _kernel = new StandardKernel(new DependencyModule());

        static void Main()
        {
            _logger.Info("DataGenerator is running...");
            var worker = _kernel.Get<Worker>();
            worker.Generate(Tags.Root.Value);

            _logger.Info("Press ANY key to exit.");
            Console.ReadKey();
        }
    }
}
