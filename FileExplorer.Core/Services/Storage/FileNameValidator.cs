using FileExplorer.Core.Contracts.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FileExplorer.Core.Services.Storage
{
    public sealed class FileNameValidator : INameValidator
    {
        private static readonly string InvalidMatch = $"[{new string(Path.GetInvalidFileNameChars())}]";

        private static readonly HashSet<string> InvalidWindowsNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

        /// <summary>
        /// Returns string of illegal characters for a file name
        /// </summary>
        public string IlleagalCharacters => "\\ / : * ? \" < > |";

        public bool IsInvalid(string name)
        {
            bool invalidChars = Regex.IsMatch(name, InvalidMatch);

            return invalidChars || InvalidWindowsNames.Contains(name);
        }
    }
}
