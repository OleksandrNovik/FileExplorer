#nullable enable
using FileExplorer.Models.Contracts.Storage.Properties;
using FileExplorer.Models.Storage.Additional;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.Models.Contracts.Storage.Directory
{
    public interface IDirectoryItem : IRenameableObject, IBasicPropertiesProvider, IThumbnailProvider
    {
        public IDirectory? Directory { get; }
        public FileAttributes Attributes { get; }
        public DateTime LastAccess { get; }
        public ByteSize? Size { get; }

        /// <summary>
        /// Copy item from current directory to a destination directory
        /// </summary>
        /// <param name="destination"> Path to the destination directory </param>
        /// <returns> Copy of directory item </returns>
        public IDirectoryItem Copy(string destination);

        /// <summary>
        /// Moves item from current directory to destination
        /// </summary>
        /// <param name="destination"> Path of a destination directory </param>
        public void Move(string destination);

        /// <summary>
        /// Moves item to a recycle bin
        /// </summary>
        public Task RecycleAsync();

        /// <summary>
        /// Deletes item permanently
        /// </summary>
        public void Delete();
    }
}
