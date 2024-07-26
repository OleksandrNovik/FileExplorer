using System;
using System.Linq;

namespace Helpers.StorageHelpers
{
    public static class FileExtensionsHelper
    {
        public static bool HasExtension(string path, params string[] extensions)
        {
            return extensions.Any(extension => path.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
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

        public static bool IsShortcut(string path)
        {
            return HasExtension(path, ".lnk");
        }

    }
}
