using System.Collections.Generic;

namespace DataGenerator.Nodes
{
    public abstract class Node
    {
        public static List<string> HeadersCommon => new List<string> { "Id", "Label", "Level", "Tag" };
        public abstract string Id { get; }
        public abstract string Label { get; }
        public abstract int Level { get; }
        public abstract string Tag { get; }

        public List<object> ToCsvCommon()
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
