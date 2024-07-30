using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using DirectoryItemWrapper = Models.StorageWrappers.DirectoryItemWrapper;

namespace FileExplorer.Core.Services
{
    public class FileSystemManager
    {
        public void CreatePhysical(DirectoryItemWrapper wrapper, string path)
        {
            wrapper.CreatePhysical(path);
        }

        public void Rename(DirectoryItemWrapper item)
        {
            item.Rename();
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
            //await copiedItems.CopyRangeAsync(CurrentFolder);

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
