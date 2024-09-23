using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Contracts.Storage;
using FileExplorer.Helpers.Application;
using FileExplorer.Helpers.General;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Core.Services.Storage
{
    /// <summary>
    /// Sorting service that provides sorting for folder depending on "folders first" setting
    /// </summary>
    public sealed class StorageSortingService : IStorageSortingService
    {
        /// <summary>
        /// Local settings service to set "folders first" setting value 
        /// </summary>
        private readonly ILocalSettingsService localSettings;

        public StorageSortingService(ILocalSettingsService localSettings)
        {
            this.localSettings = localSettings;
        }

        /// <inheritdoc />
        public ICollection<IDirectoryItem> SortByKey<TKey>(IStorage directory, Func<IDirectoryItem, TKey> sortFunc)
        {
            return GetDirectoryItemsParallel(directory, sortFunc, false).ToArray();
        }

        /// <inheritdoc />
        public ICollection<IDirectoryItem> SortByKeyDescending<TKey>(IStorage directory, Func<IDirectoryItem, TKey> sortFunc)
        {
            return GetDirectoryItemsParallel(directory, sortFunc, true).ToArray();
        }

        /// <summary>
        /// Sorts items inside of directory and lists folders first if needed
        /// </summary>
        /// <typeparam name="TKey"> Type of sort key </typeparam>
        /// <param name="directory"> Directory that provides its items </param>
        /// <param name="sortFunc"> Property for sorting </param>
        /// <param name="isDescending"> Is sort Descending </param>
        /// <returns></returns>
        private ParallelQuery<IDirectoryItem> GetDirectoryItemsParallel<TKey>(IStorage directory, Func<IDirectoryItem, TKey> sortFunc, bool isDescending)
        {
            var skippedAttributes = localSettings.GetSkippedAttributes();
            var foldersFirst = localSettings.ReadBool(LocalSettings.Keys.FoldersFirst);

            ParallelQuery<IDirectoryItem> result;

            if (foldersFirst is true)
            {
                var folders = directory.EnumerateSubDirectories(skippedAttributes)
                                       .AsParallel()
                                       .OfType<IDirectoryItem>()
                                       .Sort(sortFunc, isDescending);

                var files = directory.EnumerateFiles(skippedAttributes)
                                     .AsParallel()
                                     .Sort(sortFunc, isDescending);

                result = folders.Concat(files);

            }
            else
            {
                result = directory.EnumerateItems().AsParallel()
                                  .Sort(sortFunc, isDescending);
            }

            return result;
        }
    }
}
