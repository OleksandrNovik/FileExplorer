using FileExplorer.Helpers.Win32Helpers;
using FileExplorer.Models.Contracts.Storage.Directory;
using System.IO;

namespace FileExplorer.Models.ModelHelpers.Storage
{
    public static class StorageItemExtensions
    {
        public static bool HasAttributes(this IDirectoryItem item, FileAttributes attributes)
        {
            return (item.Attributes & attributes) != 0;
        }

        public static string GetItemType(this IDirectoryItem item)
        {
            return Win32Helper.GetItemType(item.Path, item.Attributes);
        }
    }
}
