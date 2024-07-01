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


        /// <summary>
        /// Tries to move physical file or folder to a different location
        /// </summary>
        /// <param name="item"> Wrapper <see cref="DirectoryItemModel"/> for a physical item in directory </param>
        /// <param name="location"> New location for an item </param>
        /// <returns> True if item has been renamed of False if item should be created first </returns>
        public bool TryMove(DirectoryItemModel item, string location);

        public bool TryDelete(DirectoryItemModel item);

        public string GetDefaultName(bool isFile);

        public void MoveToNewDirectory(DirectoryInfo dir);
    }
}
