namespace Contracts.StackOverflow
{
    public sealed class BadgeCounts
    {
        public int Bronze { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }

        public override string ToString()
        {
            return $"Bronze: {Bronze}, Gold: {Gold}, Silver: {Silver}";
        }
    }
}
