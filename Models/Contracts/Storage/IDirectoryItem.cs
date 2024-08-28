﻿#nullable enable
using System.ComponentModel;
using System.Threading.Tasks;

namespace Models.Contracts.Storage
{
    public interface IDirectoryItem : IRenameableObject, IThumbnailProvider, INotifyPropertyChanged
    {
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
