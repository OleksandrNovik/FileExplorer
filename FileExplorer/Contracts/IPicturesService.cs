using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;

namespace FileExplorer.Contracts
{
    public interface IPicturesService
    {
        public BitmapImage IconToImage(Icon icon);
    }
}
