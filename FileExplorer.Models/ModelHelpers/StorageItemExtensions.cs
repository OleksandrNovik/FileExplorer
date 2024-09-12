using FileExplorer.Models.Contracts.Storage.Directory;
using System.IO;

namespace FileExplorer.Models.ModelHelpers
{
    public static class StorageItemExtensions
    {
        public static bool HasAttributes(this IDirectoryItem item, FileAttributes attributes)
        {
            return (item.Attributes & attributes) != 0;
        }
    }
}
