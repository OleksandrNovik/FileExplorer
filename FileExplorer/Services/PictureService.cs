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
        public async Task<BitmapImage> IconToImageAsync(IStorageItemProperties item)
        {
            var imgIcon = new BitmapImage();

            var thumbnail = await item.GetThumbnailAsync(ThumbnailMode.ListView, 95);
            imgIcon.SetSource(thumbnail);

            return imgIcon;
        }

        public async Task<BitmapImage> IconToImageAsync(string path, bool isFile)
        {
            IStorageItemProperties item;

            if (isFile)
            {
                item = await StorageFile.GetFileFromPathAsync(path);
            }
            else
            {
                item = await StorageFolder.GetFolderFromPathAsync(path);
            }

            return await IconToImageAsync(item);
        }
    }
}
