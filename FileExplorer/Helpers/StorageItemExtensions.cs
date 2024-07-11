using Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Helpers
{
    public static class StorageItemExtensions
    {
        public static async Task PasteAsync(this StorageFile file, StorageFolder destination, CopyOperation operation)
        {
            switch (operation)
            {
                case CopyOperation.Copy:
                    await file.CopyAsync(destination, file.Name, NameCollisionOption.GenerateUniqueName);
                    break;
                case CopyOperation.Cut:
                    await file.MoveAsync(destination, file.Name, NameCollisionOption.GenerateUniqueName);
                    break;
                default:
                    throw new ArgumentException("None copy value is not allowed.");
            }
        }
        public static async Task PasteAsync(this IReadOnlyCollection<IStorageItem> items, StorageFolder destination, CopyOperation operation)
        {
            await Parallel.ForEachAsync(items, async (item, token) =>
            {
                if (item is StorageFile file)
                {
                    await file.PasteAsync(destination, operation);
                }
                else if (item is StorageFolder folder)
                {
                    var copied = await destination.CreateFolderAsync(folder.DisplayName,
                        CreationCollisionOption.GenerateUniqueName);
                    var folderContent = await folder.GetItemsAsync();
                    await folderContent.PasteAsync(copied, operation);
                }
            });
        }
    }
}
