using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Core
{
    public sealed class Tags
    {
        private const string TagFile = "Tags.json";

        public Tags()
        {
            string file = File.ReadAllText(TagFile);
            Root = JsonConvert.DeserializeObject<List<TagItem>>(file);
        }

        public List<TagItem> Root { get; }
    }
}
