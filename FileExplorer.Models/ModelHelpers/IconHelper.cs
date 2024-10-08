﻿#nullable enable
using FileExplorer.Helpers.StorageHelpers;
using FileExplorer.Helpers.Win32Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using DirectoryItemWrapper = FileExplorer.Models.Storage.Windows.DirectoryItemWrapper;
using FileAttributes = System.IO.FileAttributes;

namespace FileExplorer.Models.ModelHelpers
{
    public static class IconHelper
    {
        private static readonly Dictionary<string, IRandomAccessStream> CachedThumbnails = new();
        private static async Task<StorageItemThumbnail> GetIconFromItemPropsAsync(IStorageItemProperties item, uint size)
        {
            var thumbnail = await item.GetThumbnailAsync(ThumbnailMode.ListView, size);
            return thumbnail;
        }

        public static IRandomAccessStream? TryGetCachedThumbnail(string key)
        {
            IRandomAccessStream? thumbnail = null;

            if (CachedThumbnails.TryGetValue(key, out var cachedThumbnail))
            {
                thumbnail = cachedThumbnail?.CloneStream();
            }

            return thumbnail;
        }

        public static async Task<IRandomAccessStream> GetThumbnailForItem(DirectoryItemWrapper item)
        {
            var key = $"{Path.GetExtension((string?)item.Name).ToLower()}{(item.Attributes & FileAttributes.Directory) != 0}";
            var thumbnail = TryGetCachedThumbnail(key);

            if (thumbnail is null)
            {
                var itemProperties = await item.GetStorageItemPropertiesAsync();
                thumbnail = await GetIconFromItemPropsAsync(itemProperties, 95);

                if (!FileExtensionsHelper.IsImage(item.Path))
                {
                    CachedThumbnails.TryAdd(key, thumbnail);
                }
            }

            return thumbnail;
        }

        public static byte[]? GetThumbnailFromPath(string path, int size)
        {
            // Trying to get icon from cache
            var bytes = Win32Helper.GetIcon(path, size, true);

            if (bytes is null)
            {
                bytes = Win32Helper.GetIcon(path, size);
            }

            return bytes;
        }
    }
}
