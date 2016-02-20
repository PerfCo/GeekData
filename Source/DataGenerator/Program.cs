using Core;
using DataGenerator.Properties;

namespace DataGenerator
{
    class Program
    {
        private static readonly ConnectionFactory _connectionFactory = new ConnectionFactory(Settings.Default.MongoDbConnectionString, Settings.Default.DatabaseName);

        static void Main(string[] args)
        {
            var repository = new DataRepository(_connectionFactory);
            repository.Test("C#");
        }
    }
}
