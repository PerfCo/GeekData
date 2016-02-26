using System.Collections.Generic;
using System.Linq;
using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class PluralsightCourseNode : Node
    {
        private readonly PluralsightCourseEntity _entity;

        public PluralsightCourseNode(PluralsightCourseEntity entity)
        {
            _entity = entity;
            Level = 3;
            IdNode = entity.Id.ToString();
            Label = entity.Name;
            Tag = entity.Tags.FirstOrDefault();
        }

        public override string IdNode { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public List<object> ToCsv()
        {
            return new List<object>
            {
                _entity.Name,
                string.Join(",", _entity.Tags),
                _entity.Url
            };
        }
    }
}
