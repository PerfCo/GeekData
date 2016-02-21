using System.Collections.Generic;
using System.Linq;
using DataGenerator.Nodes;
using MongoDB.Bson;

namespace DataGenerator.Entities
{
    public sealed class CourseEntity : Node
    {
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
            var items1 = new string(';', RepositoryInfoEntity.Captions().Count - 1);
            var items2 = new string(';', UserEntity.Captions().Count - 1);
            var items3 = new string(';', LibNode.Captions().Count - 1);
            var items4 = new string(';', CourseNode.Captions().Count - 1);
            var items5 = new string(';', GeekNode.Captions().Count - 1);
            var items6 = new string(';', TagNode.Captions().Count - 1);

            return $"{items1}; {Id}; {Name}; {string.Join(",", Tags)}; {Url}; {items2};{items3};{items4};{items5};{items6}; {Level}";
        }

        public override int Level { get; } = 0;
    }
}
