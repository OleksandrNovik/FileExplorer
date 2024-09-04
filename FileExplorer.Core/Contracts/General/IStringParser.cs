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
        /// <returns> Null if provided string cannot be parsed to bool otherwise resulting enum value </returns>
        public TEnum? ParseEnum<TEnum>(string str)
            where TEnum : struct, Enum;


        /// <summary>
        /// Parses bool value from provided string
        /// </summary>
        /// <param name="str"> String to parse </param>
        /// <returns> Null if provided string cannot be parsed to bool otherwise resulting bool value </returns>
        public bool? ParseBool(string str);
    }
}
