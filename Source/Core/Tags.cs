using System.Collections.Generic;

namespace Core
{
    public sealed class Tags
    {
        //C,  C++, C#, F#, Go, CSS, JavaScript, Java, Ruby, Scala, Objective-C, ASP.NET, PHP, R, Python, Perl, Lua, iOS, WCF, WPF
        public Tags()
        {
            Root = new List<TagItem>
            {
                new TagItem
                {
                    StackOverflow = "C",
                    Github = "C"
                },
                new TagItem
                {
                    StackOverflow = "C++",
                    Github = "cpp"
                },
                new TagItem
                {
                    StackOverflow = "C#",
                    Github = "csharp"
                },
                new TagItem
                {
                    StackOverflow = "F#",
                    Github = "fsharp"
                },
                new TagItem
                {
                    StackOverflow = "Go",
                    Github = "Go"
                }
            };
        }

        public List<TagItem> Root { get; }
    }
}
