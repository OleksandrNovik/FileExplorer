using System;
using System.Threading.Tasks;

namespace Models.Contracts.Storage
{
    public interface IDirectoryItem : IRenameableObject, IThumbnailProvider
    {
        public DateTime LastAccess { get; }

        /// <summary>
        /// Copy item from current directory to a destination directory
        /// </summary>
        /// <param name="destination"> Path to the destination directory </param>
        public void Copy(string destination);

        /// <summary>
        /// Moves item from current directory to destination
        /// </summary>
        /// <param name="destination"> Path of a destination directory </param>
        public void Move(string destination);

        public Task RecycleAsync();
        public void Delete();
    }
}
