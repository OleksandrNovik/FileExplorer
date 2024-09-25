using System;
using System.IO;
using System.Linq;

namespace FileExplorer.Helpers.StorageHelpers
{
    public static class FileExtensionsHelper
    {
        public static bool HasExtension(string path, params string[] extensions)
        {
            var hashSet = extensions.ToHashSet(StringComparer.OrdinalIgnoreCase);
            return hashSet.Contains(Path.GetExtension(path));
        }

        public static bool IsImage(string path)
        {
            return HasExtension(path, ".png", ".bmp", ".jpg", ".jpeg", ".jfif", ".gif",
                ".tiff", ".tif", ".webp");
        }

        public static bool IsVideo(string path)
        {
            return HasExtension(path, ".mp4", ".webm", ".ogg", ".mov", ".qt", ".m4v",
                ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".mkv");
        }

        public static bool IsMedia(string path)
        {
            return HasExtension(path, ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp",
                ".mpg", ".mp2", ".mpeg", ".mpe", ".mpv", ".mkv", ".ogg", ".avi", ".wmv", ".mov", ".qt");
        }

        public static bool IsExecutable(string path)
        {
            return HasExtension(path, ".exe", ".bat", ".sh", ".msi", ".app", ".apk", ".bin");
        }

        public static bool IsDocument(string path)
        {
            return HasExtension(path, ".pdf", ".doc", ".docx", ".xls", ".xlsx",
                ".ppt", ".pptx", ".txt", ".rtf", ".odt");
        }

        public static bool IsArchive(string path)
        {
            return HasExtension(path, ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz", ".iso");
        }

        public static bool IsShortcut(string path)
        {
            return HasExtension(path, ".lnk");
        }

    }
}
