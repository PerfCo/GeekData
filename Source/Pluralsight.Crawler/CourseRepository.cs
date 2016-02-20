using Contracts.Pluralsight;
using Core;
using MongoDB.Bson;
using Nelibur.ObjectMapper;
using Pluralsight.Crawler.Entities;

namespace Pluralsight.Crawler
{
    public sealed class CourseRepository : Repository
    {
        private const string CollectionName = "PluralsightCourses";

        public CourseRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void Save(PluralsightCourse value)
        {
            var entity = TinyMapper.Map<CourseEntity>(value);
            entity.Id = ObjectId.GenerateNewId();
            GetCollection<CourseEntity>(CollectionName).InsertOneAsync(entity).Wait();
        }
    }
}
