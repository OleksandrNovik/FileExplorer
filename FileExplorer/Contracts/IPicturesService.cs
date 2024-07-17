using FileExplorer.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

namespace FileExplorer.Contracts
{
    public interface IPicturesService
    {
        public Task<BitmapImage> GetThumbnailForItem(DirectoryItemWrapper item);

    }
}
