using System;

namespace FileExplorer.Helpers.General
{
    public static class StringExtensions
    {
        public static bool ContainsPattern(this string str, string pattern)
        {
            var strSpan = str.AsSpan();
            var patternSpan = pattern.AsSpan();

            return strSpan.Contains(patternSpan, StringComparison.OrdinalIgnoreCase);
        }
    }
}
