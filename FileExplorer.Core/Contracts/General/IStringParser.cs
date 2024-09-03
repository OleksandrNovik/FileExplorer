using System;

namespace FileExplorer.Core.Contracts.General
{
    /// <summary>
    /// Contract for service that parses strings to other values or classes
    /// </summary>
    public interface IStringParser
    {
        /// <summary>
        /// Parses string to enum value
        /// </summary>
        /// <typeparam name="TEnum"> Type of enum to parse </typeparam>
        /// <param name="str"> String value that is parsed </param>
        /// <param name="fallbackValue"> Fall back value that will be returned if parse is not successful </param>
        /// <returns> Resulting enum value </returns>
        public TEnum ParseEnum<TEnum>(string str, TEnum fallbackValue)
            where TEnum : struct, Enum;


        /// <summary>
        /// Parses string to bool value
        /// </summary>
        /// <param name="str"> String value that is parsed </param>
        /// <param name="fallbackValue"> Fall back value that will be returned if parse is not successful </param>
        /// <returns> Resulting bool value </returns>
        public bool ParseBool(string str, bool fallbackValue);
    }
}
