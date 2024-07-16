using FileExplorer.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface IPicturesService
    {
        public Task<BitmapImage> IconToImageAsync(IStorageItemProperties item);

        public Task<BitmapImage> GetThumbnailForItem(DirectoryItemWrapper item);

    }
}
