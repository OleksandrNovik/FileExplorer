using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using DirectoryItemWrapper = FileExplorer.Models.Storage.Windows.DirectoryItemWrapper;

namespace FileExplorer.Core.Contracts
{
    //TODO: this interface should be changed to a clipboard manager
    public interface IDirectoryManager
    {
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
