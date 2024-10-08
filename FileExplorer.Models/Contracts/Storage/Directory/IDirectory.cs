﻿using FileExplorer.Models.Contracts.Storage.Properties;

namespace FileExplorer.Models.Contracts.Storage.Directory
{
    /// <summary>
    /// Interface that contains methods for each directory to implement 
    /// </summary>
    public interface IDirectory : IStorage, IBasicPropertiesProvider
    {

        /// <summary>
        /// Creates item inside this directory
        /// </summary>
        /// <param name="isDirectory"> True if directory needs to be created and False if file is being created </param>
        /// <returns> New item that was created inside of directory </returns>
        public IDirectoryItem Create(bool isDirectory);
    }
}
