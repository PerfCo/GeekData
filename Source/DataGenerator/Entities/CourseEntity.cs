using System.Collections.Generic;
using System.Linq;
using DataGenerator.Nodes;
using MongoDB.Bson;

namespace DataGenerator.Entities
{
    public sealed class CourseEntity : Node
    {
        public static string Empty => ";;;";
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Url { get; set; }

        public new static List<string> Captions()
        {
            var suffix = "PluralsightCourse";
            var items = new[] { "Id", "Name", "Tags", "Url" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            return $"{Id}; {Name}; {string.Join(",", Tags)}; {Url}";
        }

        public override int Level { get; } = 0;
    }
}
