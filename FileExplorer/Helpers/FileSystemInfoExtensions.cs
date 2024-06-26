using System.IO;
using System.Linq;

namespace FileExplorer.Helpers
{
    public static class FileSystemInfoExtensions
    {
        private static readonly string[] ImageExtensions = ["", ""];

        public static bool IsImage(this FileSystemInfo info)
        {
            var fileExtension = Path.GetExtension(info.Name);

            return ImageExtensions.Contains(fileExtension);
        }
    }
}
