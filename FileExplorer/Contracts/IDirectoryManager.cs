using FileExplorer.Models;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        public DirectoryInfo CurrentDirectory { get; set; }

        /// <summary>
        /// Creates item at given location (assuming it does not exist)
        /// </summary>
        /// <param name="item"> Information about new item </param>
        public void Create(DirectoryItemModel item);

        /// <summary>
        /// Moves file to new location (if possible)
        /// </summary>
        /// <param name="item"> Item that is moved </param>
        /// <param name="location"> New location for an item </param>
        public void Move(DirectoryItemModel item, string location);

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items);

        public IEnumerable<DirectoryItemModel> PasteFromClipboard();

        /// <summary>
        /// Deletes item from directory (if possible)
        /// </summary>
        /// <param name="item"> Item that should be deleted </param>
        public void Delete(DirectoryItemModel item);

        /// <summary>
        /// Gets default name for a file or folder (example: New File 0)
        /// </summary>
        /// <param name="nameTemplate"></param>
        /// <param name="isFile"> Is name for a file or folder </param>
        /// <returns> New unique name for item </returns>
        public string GetDefaultName(string nameTemplate, bool isFile);

    }
}
