using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface IPicturesService
    {
        public Task<BitmapImage> IconToImageAsync(IStorageItem item);

        public Task<BitmapImage> IconToImageAsync(string path, bool isFile);

    }
}
