using System;
using System.Collections.Generic;
using Contracts.Pluralsight;
using Ninject;
using NLog;
using Pluralsight.Crawler.Dependencies;
using Pluralsight.Crawler.Properties;

namespace Pluralsight.Crawler
{
    internal class Program
    {
        private static readonly Settings _settings = Settings.Default;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly IKernel _kernel = new StandardKernel(new DependencyModule());

        private static void Main()
        {
            _logger.Info("Pluralsight.Crawler is running. Press ANY key to exit.");
            List<PluralsightCourse> couses = new Worker().GetCourses(_settings.PluralsightCoursesFilePath);

            SaveCourses(couses);

            Console.ReadKey();
        }

        private static void SaveCourses(List<PluralsightCourse> courses)
        {
            var repository = _kernel.Get<CourseRepository>();
            courses.ForEach(repository.Save);
        }
    }
}
