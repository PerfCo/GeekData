using System.Collections.Generic;
using System.Linq;
using DataGenerator.Nodes;
using MongoDB.Bson;

namespace DataGenerator.Entities
{
    public sealed class CourseEntity : Node
    {
        public ObjectId Id { get; set; }
        public override string IdNode { get; }
        public override string Label { get; }

        public override int Level { get; } = 3;
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Url { get; set; }

        public new static List<string> Captions()
        {
            var suffix = "PluralsightCourse";
            var items = new[] { "Name", "Tags", "Url" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public override string ToString()
        {
            var items1 = new string(';', RepositoryInfoEntity.Captions().Count - 1);
            var items2 = new string(';', UserEntity.Captions().Count - 1);

            return $"{Id};{Name};{Level};{items1};{Name}; {string.Join(",", Tags)}; {Url}; {items2}";
        }
    }
}
