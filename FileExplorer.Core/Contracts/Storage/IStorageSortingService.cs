using FileExplorer.Models.Contracts.Storage.Directory;
using System;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Storage
{
    public interface IStorageSortingService
    {
        public ICollection<IDirectoryItem> SortByKey<TKey>(IDirectory directory, Func<IDirectoryItem, TKey> sortFunc);
    }
}