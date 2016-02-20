using Contracts.Github;
using Core;
using Github.Crawler.Entities;
using Github.Crawler.Properties;
using Nelibur.ObjectMapper;
using Ninject.Modules;

namespace Github.Crawler.Dependencies
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
            TinyMapper.Bind<GithubRepositoryInfo, RepositoryInfoEntity>();
        }
    }
}
