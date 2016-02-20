using Contracts.Pluralsight;
using Core;
using Nelibur.ObjectMapper;
using Ninject.Modules;
using Pluralsight.Crawler.Entities;
using Pluralsight.Crawler.Properties;

namespace Pluralsight.Crawler.Dependencies
{
    public sealed class DependencyModule : NinjectModule
    {
        private readonly Settings _settings = Settings.Default;

        public override void Load()
        {
            RegisterMappings();
            RegisterConnectionFactory();
        }

        private void RegisterConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory(_settings.MongoDbConnectionString, _settings.DatabaseName);
            Bind<ConnectionFactory>().ToConstant(connectionFactory).InSingletonScope();
        }

        private static void RegisterMappings()
        {
            TinyMapper.Bind<PluralsightCourse, CourseEntity>();
        }
    }
}
