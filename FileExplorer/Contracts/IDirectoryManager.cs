using FileExplorer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        public StorageFolder CurrentFolder { get; set; }
        public DirectoryWrapper CurrentDirectory { get; set; }

        public void CreatePhysical(DirectoryItemWrapper wrapper);

        public void Rename(DirectoryItemWrapper item);

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items, DataPackageOperation operation);

        public Task<IEnumerable<IStorageItem>> PasteFromClipboard();

        /// <summary>
        /// Deletes item with ability to restore it from recycle bin
        /// </summary>
        /// <param name="item"> Item that being moved to a recycle bin </param>
        public Task MoveToRecycleBinAsync(DirectoryItemModel item);

        /// <summary>
        /// Permanently deletes item
        /// </summary>
        /// <param name="item"> Item that being deleted permanently </param>
        public Task DeleteAsync(DirectoryItemModel item);
    }
}
