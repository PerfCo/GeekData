using System.Collections.Generic;
using System.Linq;

namespace DataGenerator.Nodes
{
    public abstract class Node
    {
        protected const string Separator = ";";
        public abstract string Id { get; }
        public abstract string Label { get; }
        public abstract int Level { get; }
        public abstract string Tag { get; }

        public static List<string> Captions()
        {
            var items = new[] { "Id", "Label", "Level", "Tag" };
            return items.ToList();
        }

        protected List<object> ToCsvCommon()
        {
            return new List<object>
            {
                Id,
                Label,
                Level,
                Tag
            };
        }
    }
}
