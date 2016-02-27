using System.Collections.Generic;
using DataGenerator.Nodes;
using DataGenerator.Nodes.Cources;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataGenerator.Repositories.Entities
{
    [BsonIgnoreExtraElements]
    public sealed class PluralsightCourseEntity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Url { get; set; }

        public PluralsightCourseNode ToNode()
        {
            return new PluralsightCourseNode(this);
        }
    }
}
