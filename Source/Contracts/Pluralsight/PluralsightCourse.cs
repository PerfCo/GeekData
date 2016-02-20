using System.Collections.Generic;

namespace Contracts.Pluralsight
{
    public sealed class PluralsightCourse
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string Url { get; set; }
    }
}
