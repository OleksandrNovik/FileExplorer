using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        public StorageFolder CurrentDirectory { get; set; }

        public async Task<DirectoryItemModel> CreateAsync(bool isFile)
        {
            IStorageItem item;

            if (isFile)
            {
                item = await CurrentDirectory.CreateFileAsync("New File",
                    CreationCollisionOption.GenerateUniqueName);
            }
            else
            {
                item = await CurrentDirectory.CreateFolderAsync("New Folder",
                    CreationCollisionOption.GenerateUniqueName);
            }
            return new DirectoryItemModel(item);
        }

        public async Task RenameAsync(DirectoryItemModel item)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await item.FullInfo.RenameAsync(item.Name, NameCollisionOption.GenerateUniqueName);

            item.Name = item.FullInfo.Name;
        }

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items, DataPackageOperation operation)
        {
            var dirItems = items.AsParallel().Select(model => model.FullInfo);

            var dataPackage = new DataPackage();
            dataPackage.SetStorageItems(dirItems);
            dataPackage.RequestedOperation = operation;

            Clipboard.SetContent(dataPackage);
        }

        public async Task<IEnumerable<IStorageItem>> PasteFromClipboard()
        {
            var clipboardContent = Clipboard.GetContent();

            if (!clipboardContent.Contains(StandardDataFormats.StorageItems))
            {
                throw new ArgumentException("Clipboard does not contain storage items", nameof(clipboardContent));
            }

            var copiedItems = (await clipboardContent.GetStorageItemsAsync()).ToArray();
            await copiedItems.CopyRangeAsync(CurrentDirectory);

            if ((clipboardContent.RequestedOperation & DataPackageOperation.Move) != 0)
            {
                foreach (var item in copiedItems)
                {
                    await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }

            return copiedItems;
        }

        private async Task DeleteItemAsync(DirectoryItemModel item, StorageDeleteOption deleteOption = StorageDeleteOption.Default)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await item.FullInfo.DeleteAsync(deleteOption);
        }

        public async Task MoveToRecycleBinAsync(DirectoryItemModel item) => await DeleteItemAsync(item);

        public async Task DeleteAsync(DirectoryItemModel item) =>
            await DeleteItemAsync(item, StorageDeleteOption.PermanentDelete);
    }
}
