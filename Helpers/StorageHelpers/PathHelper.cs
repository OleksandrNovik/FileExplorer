using System.IO;

namespace Helpers.StorageHelpers
{
    public static class PathHelper
    {
        public static string CreatePattern(string query)
        {
            string pattern = "*";

            if (Path.HasExtension(query))
            {
                var extension = Path.GetExtension(query);
                var name = Path.GetFileNameWithoutExtension(query);
                pattern = $"*{name}*.*{extension}*";
            }

            return pattern;
        }
    }
}
