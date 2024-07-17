using FileExplorer.Contracts;
using FileExplorer.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace FileExplorer.Services
{
    public class PictureService : IPicturesService
    {
        private async Task<BitmapImage> IconToImageAsync(IStorageItemProperties item)
        {
            var imgIcon = new BitmapImage();

            var thumbnail = await item.GetThumbnailAsync(ThumbnailMode.ListView, 95);
            imgIcon.SetSource(thumbnail);

            return imgIcon;
        }

        public async Task<BitmapImage> GetThumbnailForItem(DirectoryItemWrapper item)
        {
            var itemProperties = await item.GetStorageItemPropertiesAsync();
            return await IconToImageAsync(itemProperties);
        }
    }
}
