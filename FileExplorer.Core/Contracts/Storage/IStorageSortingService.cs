using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using System;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Storage
{
    /// <summary>
    /// Contract for a sort service to fulfill 
    /// </summary>
    public interface IStorageSortingService
    {
        /// <summary>
        /// Sorts directory's items 
        /// </summary>
        /// <typeparam name="TKey"> Type of sort property </typeparam>
        /// <param name="storage"> Storage for listing items </param>
        /// <param name="sortFunc"> Property for sorting </param>
        /// <returns> Collection of sorted directory items </returns>
        public ICollection<IDirectoryItem> SortByKey<TKey>(IStorage storage, Func<IDirectoryItem, TKey> sortFunc);


        /// <summary>
        /// Sorts directory's items in descending order 
        /// </summary>
        /// <typeparam name="TKey"> Type of sort property </typeparam>
        /// <param name="storage"> Storage for listing items </param>
        /// <param name="sortFunc"> Property for sorting </param>
        /// <returns> Collection of sorted directory items </returns>
        public ICollection<IDirectoryItem> SortByKeyDescending<TKey>(IStorage storage, Func<IDirectoryItem, TKey> sortFunc);

    }
}