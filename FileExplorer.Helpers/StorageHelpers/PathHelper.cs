using System.IO;

namespace FileExplorer.Helpers.StorageHelpers
{
    public static class PathHelper
    {
        public static string CreatePattern(string query)
        {
            string pattern = "*";

            if (Path.HasExtension(query))
            {
                var extension = Path.GetExtension(query).Remove(0, 1);
                var name = Path.GetFileNameWithoutExtension(query);
                pattern = $"*{name}*.*{extension}*";
            }

            return pattern;
        }
    }
}
