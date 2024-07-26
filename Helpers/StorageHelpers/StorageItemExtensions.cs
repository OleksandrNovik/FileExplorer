using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Helpers.StorageHelpers
{
    public static class StorageItemExtensions
    {
        public static async Task CopyRangeAsync(this IEnumerable<IStorageItem> items, StorageFolder destination)
        {
            foreach (var item in items)
            {
                await item.CopyAsync(destination);
            }
        }

        public static async Task CopyAsync(this IStorageItem item, StorageFolder destination)
        {
            if (item is StorageFile file)
            {
                await file.CopyAsync(destination, file.Name, NameCollisionOption.GenerateUniqueName);
            }
            else if (item is StorageFolder folder)
            {
                await folder.CopyAsync(destination);
            }
            else
            {
                throw new ArgumentException("Item is neither a file nor folder", nameof(item));
            }
        }

        public static async Task CopyAsync(this StorageFolder folder, StorageFolder destination)
        {
            var folderItems = await folder.GetItemsAsync();

            var copy = await destination.CreateFolderAsync(folder.Name, CreationCollisionOption.GenerateUniqueName);

            foreach (var item in folderItems)
            {
                await item.CopyAsync(copy);
            }
        }

        //public static async Task MoveAsync(this IStorageItem item, StorageFolder destination)
        //{
        //    if (item is StorageFile file)
        //    {
        //        await file.MoveAsync(destination, file.Name, NameCollisionOption.GenerateUniqueName);
        //    }
        //    else if (item is StorageFolder folder)
        //    {
        //        await folder.MoveAsync(destination);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Item is neither a file nor folder", nameof(item));
        //    }
        //}

        //public static async Task MoveAsync(this StorageFolder folder, StorageFolder destination)
        //{
        //    await folder.CopyAsync(destination);
        //    await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
        //}
    }
}
