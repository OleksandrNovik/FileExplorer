using Models.Contracts.Storage;
using System.IO;

namespace Helpers.StorageHelpers
{
    public static class StorageItemExtensions
    {
        public static bool HasAttributes(this IDirectoryItem item, FileAttributes attributes)
        {
            return (item.Attributes & attributes) != 0;
        }
    }
}
