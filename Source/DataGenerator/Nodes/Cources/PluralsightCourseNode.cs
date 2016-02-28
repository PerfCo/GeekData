using System.Collections.Generic;
using System.Linq;
using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Cources
{
    public sealed class PluralsightCourseNode : Node
    {
        private readonly PluralsightCourseEntity _entity;

        static PluralsightCourseNode()
        {
            var suffix = "PluralsightCourse";
            var items = new[] { "Name", "Tags", "Url" };
            Headers = items.Select(x => $"{x}{suffix}").ToList();
        }

        public PluralsightCourseNode(PluralsightCourseEntity entity)
        {
            _entity = entity;
            Level = 3;
            Id = entity.Id.ToString();
            Label = entity.Name;
            Tag = entity.Tags.FirstOrDefault();
        }

        public static List<string> Headers { get; }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            var result = new List<object>
            {
                _entity.Name,
                string.Join(",", _entity.Tags),
                _entity.Url
            };
            return result;
        }
    }
}
