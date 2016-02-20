using System.Collections.Generic;

namespace Contracts.StackOverflow
{
    public sealed class StackOverflowUser
    {
        public int AccountId { get; set; }
        public BadgeCounts BadgeCounts { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileUrl { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
