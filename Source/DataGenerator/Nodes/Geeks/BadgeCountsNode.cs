using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Geeks
{
    public sealed class BadgeCountsNode
    {
        private readonly BadgeCounts _entity;

        public BadgeCountsNode(BadgeCounts entity)
        {
            _entity = entity;
        }

        public string ToCsv()
        {
            return $"{{Bronze: {_entity.Bronze}, Gold: {_entity.Gold}, Silver: {_entity.Silver}}}";
        }
    }
}
