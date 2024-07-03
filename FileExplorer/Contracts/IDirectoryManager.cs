using FileExplorer.Models;
using System.IO;

namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        /// <summary>
        /// Creates item at given location (assuming it does not exist)
        /// </summary>
        /// <param name="item"> Information about new item </param>
        public void Create(DirectoryItemModel item);

        public void Move(DirectoryItemModel item, string location);

        public void Delete(DirectoryItemModel item);

        public string GetDefaultName(bool isFile);

        public void MoveToNewDirectory(DirectoryInfo dir);
    }
}
