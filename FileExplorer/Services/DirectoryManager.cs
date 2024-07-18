using FileExplorer.Contracts;
using FileExplorer.Helpers.StorageHelpers;
using FileExplorer.Models.StorageWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using DirectoryItemWrapper = FileExplorer.Models.StorageWrappers.DirectoryItemWrapper;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        public StorageFolder CurrentFolder { get; set; }

        public DirectoryWrapper CurrentDirectory { get; set; }

        public void CreatePhysical(DirectoryItemWrapper wrapper)
        {
            wrapper.CreatePhysical(CurrentDirectory.Path);
        }

        public void Rename(DirectoryItemWrapper item)
        {
            item.Move(CurrentDirectory.Path);
        }

        //public void CopyToClipboard(IEnumerable<DirectoryItemModel> items, DataPackageOperation operation)
        //{
        //    var dirItems = items.AsParallel().Select(model => model.FullInfo);

        //    var dataPackage = new DataPackage();
        //    dataPackage.SetStorageItems(dirItems);
        //    dataPackage.RequestedOperation = operation;

        //    Clipboard.SetContent(dataPackage);
        //}

        public async Task<IEnumerable<IStorageItem>> PasteFromClipboard()
        {
            var clipboardContent = Clipboard.GetContent();

            if (!clipboardContent.Contains(StandardDataFormats.StorageItems))
            {
                throw new ArgumentException("Clipboard does not contain storage items", nameof(clipboardContent));
            }

            var copiedItems = (await clipboardContent.GetStorageItemsAsync()).ToArray();
            await copiedItems.CopyRangeAsync(CurrentFolder);

            if ((clipboardContent.RequestedOperation & DataPackageOperation.Move) != 0)
            {
                foreach (var item in copiedItems)
                {
                    await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }

            return copiedItems;
        }

        public async Task MoveToRecycleBinAsync(DirectoryItemWrapper item)
        {
            await item.RecycleAsync();
        }


        public void Delete(DirectoryItemWrapper item)
        {
            item.Delete();
        }
    }
}
