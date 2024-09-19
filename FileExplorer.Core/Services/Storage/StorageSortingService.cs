using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Contracts.Storage;
using FileExplorer.Helpers.Application;
using FileExplorer.Helpers.General;
using FileExplorer.Models.Contracts.Storage.Directory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Core.Services.Storage
{
    public sealed class StorageSortingService : IStorageSortingService
    {
        private readonly ILocalSettingsService localSettings;

        public StorageSortingService(ILocalSettingsService localSettings)
        {
            this.localSettings = localSettings;
        }

        public ICollection<IDirectoryItem> SortByKey<TKey>(IDirectory directory, Func<IDirectoryItem, TKey> sortFunc)
        {
            return GetDirectoryItemsParallel(directory, sortFunc, false).ToArray();
        }

        public ICollection<IDirectoryItem> SortByKeyDescending<TKey>(IDirectory directory, Func<IDirectoryItem, TKey> sortFunc)
        {
            return GetDirectoryItemsParallel(directory, sortFunc, true).ToArray();
        }

        private ParallelQuery<IDirectoryItem> GetDirectoryItemsParallel<TKey>(IDirectory directory, Func<IDirectoryItem, TKey> sortFunc, bool isDescending)
        {
            var skippedAttributes = localSettings.GetSkippedAttributes();
            var foldersFirst = localSettings.ReadBool(LocalSettings.Keys.FoldersFirst);

            ParallelQuery<IDirectoryItem> result;

            if (foldersFirst is true)
            {
                var folders = directory.EnumerateSubDirectories(skippedAttributes)
                                       .AsParallel()
                                       .OfType<IDirectoryItem>()
                                       .Order(sortFunc, isDescending);

                var files = directory.EnumerateFiles(skippedAttributes)
                                     .AsParallel()
                                     .Order(sortFunc, isDescending);

                result = folders.Concat(files);

            }
            else
            {
                result = directory.EnumerateItems().AsParallel().Order(sortFunc, isDescending);
            }

            return result;
        }
    }
}
