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

        /// <summary>
        /// Creates new item in <see cref="CurrentFolder"/> asynchronely
        /// </summary>
        /// <param name="isFile"> Is created item file or a folder </param>
        /// <returns> Corresponding wrapper element for a physical file/folder that was created </returns>
        public Task<DirectoryItemModel> CreateAsync(bool isFile);

        /// <summary>
        /// Renames item that is provided as argument (sets unique name if name is colliding with existing item) 
        /// </summary>
        /// <param name="item"> Item that is renamed </param>
        public Task RenameAsync(DirectoryItemModel item);

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
