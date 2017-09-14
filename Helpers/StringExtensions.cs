namespace Helpers
{
    public static class StringExtensions
    {
        public static string[] SplitLastOf(this string toSplit, char splitChar)
        {
            var index = toSplit.LastIndexOf(splitChar);
            return new[] { toSplit.Substring(0, index), toSplit.Substring(index + 1) };
        }
    }
}