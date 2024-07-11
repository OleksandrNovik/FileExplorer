using FileExplorer.Contracts;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace FileExplorer.Services
{
    public class PictureService : IPicturesService
    {
        public async Task<BitmapImage> IconToImageAsync(IStorageItem item)
        {
            var imgIcon = new BitmapImage();

            var storageItemProperties = item as IStorageItemProperties;

            ArgumentNullException.ThrowIfNull(storageItemProperties);

            var thumbnail = await storageItemProperties.GetThumbnailAsync(ThumbnailMode.ListView, 95);
            imgIcon.SetSource(thumbnail);

            return imgIcon;
        }

    }
}
