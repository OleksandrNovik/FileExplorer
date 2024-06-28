using FileExplorer.Models;

namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        /// <summary>
        /// Creates item at given location (assuming it does not exist)
        /// </summary>
        /// <param name="item"> Information about new item </param>
        /// <param name="location"> Location to create new item in </param>
        public void Create(DirectoryItemModel item, string location);

        /// <summary>
        /// Tries to move physical file or folder to a different location
        /// </summary>
        /// <param name="item"> Wrapper <see cref="DirectoryItemModel"/> for a physical item in directory </param>
        /// <param name="location"> New location for an item </param>
        /// <returns> True if item has been renamed of False if item should be created first </returns>
        public bool TryMove(DirectoryItemModel item, string location);
    }
}
