using FileExplorer.Core.Contracts.General;
using System;

namespace FileExplorer.Core.Services.General
{
    public sealed class StringParser : IStringParser
    {
        /// <inheritdoc />
        public TEnum? ParseEnum<TEnum>(string str)
            where TEnum : struct, Enum
        {
            TEnum? value = null;

            if (Enum.TryParse<TEnum>(str, out var result))
            {
                value = result;
            }

            return value;
        }

        /// <inheritdoc />
        public bool? ParseBool(string str)
        {
            bool? value = null;

            if (bool.TryParse(str, out var result))
            {
                value = result;
            }

            return value;
        }
    }
}
