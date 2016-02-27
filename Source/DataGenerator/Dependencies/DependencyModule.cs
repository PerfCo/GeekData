using Core;
using DataGenerator.Properties;
using DataGenerator.Repositories;
using Ninject.Modules;

namespace DataGenerator.Dependencies
{
    public sealed class DependencyModule : NinjectModule
    {
        private readonly Settings _settings = Settings.Default;

        public override void Load()
        {
            RegisterConnectionFactory();
            RegisterRepositories();
        }

        private void RegisterConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory(_settings.MongoDbConnectionString, _settings.DatabaseName);
            Bind<ConnectionFactory>().ToConstant(connectionFactory).InSingletonScope();
        }

        private void RegisterRepositories()
        {
            Bind<DataRepository>().ToSelf().InSingletonScope();
        }
    }
}
