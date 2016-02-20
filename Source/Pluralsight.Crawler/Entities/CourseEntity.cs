using System.Collections.Generic;
using MongoDB.Bson;

namespace Pluralsight.Crawler.Entities
{
    public sealed class CourseEntity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Url { get; set; }
    }
}