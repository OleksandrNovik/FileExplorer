using FileExplorer.Core.Contracts.General;
using System;

namespace FileExplorer.Core.Services.General
{
    public sealed class StringParser : IStringParser
    {
        /// <inheritdoc />
        public TEnum ParseEnum<TEnum>(string str, TEnum fallbackValue)
            where TEnum : struct, Enum
        {
            if (!Enum.TryParse<TEnum>(str, out var value))
            {
                value = fallbackValue;
            }

            return value;
        }

        /// <inheritdoc />
        public bool ParseBool(string str, bool fallbackValue)
        {
            if (!bool.TryParse(str, out var value))
            {
                value = fallbackValue;
            }

            return value;
        }
    }
}
