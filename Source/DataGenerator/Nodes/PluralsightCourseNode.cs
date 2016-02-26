using System.Collections.Generic;
using System.Linq;
using DataGenerator.Entities;

namespace DataGenerator.Nodes
{
    public sealed class PluralsightCourseNode : Node
    {
        public PluralsightCourseNode(PluralsightCourseEntity entity)
        {
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

            };
        }

    }
}
