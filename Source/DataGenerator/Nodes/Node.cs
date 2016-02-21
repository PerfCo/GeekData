using System.Collections.Generic;
using System.Linq;

namespace DataGenerator.Nodes
{
    public abstract class Node
    {
        public static List<string> Captions()
        {
            var items = new[] { "Level" };
            return items.ToList();
        }

        public abstract int Level { get; }
    }
}