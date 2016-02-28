using System.IO;
using DataGenerator.Repositories.Entities;
using Jil;

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
            using (var output = new StringWriter())
            {
                JSON.Serialize(_entity, output);
                return output.ToString();
            }
        }
    }
}
