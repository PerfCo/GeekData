using DataGenerator.Nodes.Geeks;

namespace DataGenerator.Repositories.Entities
{
    public sealed class BadgeCounts
    {
        public int Bronze { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }

        public BadgeCountsNode ToNode()
        {
            return new BadgeCountsNode(this);
        }
    }
}
