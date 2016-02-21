using System.Collections.Generic;

namespace Core
{
    public sealed class TagItem
    {
        public string StackOverflow { get; set; }
        public string Github { get; set; }
        public List<SubTagItem> SubTags { get; set; } = new List<SubTagItem>();
    }
}