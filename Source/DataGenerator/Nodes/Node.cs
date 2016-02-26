using System.Collections.Generic;
using System.Linq;

namespace DataGenerator.Nodes
{
    public abstract class Node
    {
        protected const string Separator = ";";
        public abstract string IdNode { get; }
        public abstract string Label { get; }
        public abstract int Level { get; }
        public abstract string Tag { get; }

        public static List<string> Captions()
        {
            var items = new[] { "id", "Label", "Level", "Tag" };
            return items.ToList();
        }
    }
}
