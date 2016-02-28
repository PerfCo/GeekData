using System;
using System.Collections.Generic;
using System.IO;
using Jil;

namespace Core
{
    public static class Tags
    {
        private const string TagFile = "Tags.json";

        public static Lazy<List<TagItem>> Root { get; } = new Lazy<List<TagItem>>(() =>
        {
            string fileContent = File.ReadAllText(TagFile);
            return JSON.Deserialize<List<TagItem>>(fileContent);
//            return JsonConvert.DeserializeObject<List<TagItem>>(fileContent);
        });
    }
}
