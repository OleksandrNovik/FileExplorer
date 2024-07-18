using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        public DirectoryWrapper CurrentDirectory { get; set; }

        public void CreatePhysical(DirectoryItemWrapper wrapper);

        public void Rename(DirectoryItemWrapper item);

        //public void CopyToClipboard(IEnumerable<DirectoryItemModel> items, DataPackageOperation operation);

        public Task<IEnumerable<IStorageItem>> PasteFromClipboard();

        /// <summary>
        /// Deletes item with ability to restore it from recycle bin
        /// </summary>
        /// <param name="item"> Item that being moved to a recycle bin </param>
        public Task MoveToRecycleBinAsync(DirectoryItemWrapper item);

        public void Delete(DirectoryItemWrapper item);

    }
}
