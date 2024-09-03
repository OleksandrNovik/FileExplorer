using System;
using System.Linq;

namespace Helpers.General
{
    public static class StringExtensions
    {
        public static bool ContainsPattern(this string str, string pattern)
        {
            var strSpan = str.AsSpan();
            var patternSpan = pattern.AsSpan();

            return strSpan.Contains(patternSpan, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Parses string to enum value
        /// </summary>
        /// <typeparam name="TEnum"> Type of enum to parse </typeparam>
        /// <param name="str"> String value that is parsed </param>
        /// <param name="fallbackValue"> Fall back value that will be returned if parse is not successful </param>
        /// <returns> Resulting enum value </returns>
        public static TEnum ParseEnum<TEnum>(this string str, TEnum fallbackValue)
            where TEnum : struct, Enum
        {
            if (!Enum.TryParse<TEnum>(str, out var value))
            {
                value = fallbackValue;
            }

            return value;
        }

        /// <summary>
        /// Parses string to bool value
        /// </summary>
        /// <param name="str"> String value that is parsed </param>
        /// <param name="fallbackValue"> Fall back value that will be returned if parse is not successful </param>
        /// <returns> Resulting bool value </returns>
        public static bool ParseBool(this string str, bool fallbackValue)
        {
            if (!bool.TryParse(str, out var value))
            {
                value = fallbackValue;
            }

            return value;
        }
    }
}
