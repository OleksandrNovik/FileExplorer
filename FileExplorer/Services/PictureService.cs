#nullable enable
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using FileAttributes = System.IO.FileAttributes;

namespace FileExplorer.Services
{
    public class PictureService : IPicturesService
    {
        private static readonly Dictionary<string, IRandomAccessStream> CachedThumbnails = new();

        private async Task<StorageItemThumbnail> GetIconFromItemPropsAsync(IStorageItemProperties item)
        {
            var thumbnail = await item.GetThumbnailAsync(ThumbnailMode.ListView, 95);
            return thumbnail;
        }

        public IRandomAccessStream? TryGetCachedThumbnail(string key)
        {
            IRandomAccessStream? thumbnail = null;

            if (CachedThumbnails.TryGetValue(key, out var cachedThumbnail))
            {
                thumbnail = cachedThumbnail.CloneStream();
            }

            return thumbnail;
        }

        public async Task<IRandomAccessStream> GetThumbnailForItem(DirectoryItemWrapper item)
        {
            var key = $"{Path.GetExtension(item.Name).ToLower()}{(item.Attributes & FileAttributes.Directory) != 0}";
            var thumbnail = TryGetCachedThumbnail(key);

            if (thumbnail is null)
            {
                var itemProperties = await item.GetStorageItemPropertiesAsync();
                thumbnail = await GetIconFromItemPropsAsync(itemProperties);

                if (!FileExtensionsHelper.IsImage(item.Name))
                {
                    CachedThumbnails.Add(key, thumbnail);
                }
            }

            return thumbnail;
        }
    }
}
