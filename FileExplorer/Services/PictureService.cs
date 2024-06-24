using FileExplorer.Contracts;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FileExplorer.Services
{
    public class PictureService : IPicturesService
    {
        public BitmapImage IconToImage(Icon icon)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var iconBmp = icon.ToBitmap();
                iconBmp.SetResolution(95, 95);
                iconBmp.Save(ms, ImageFormat.Png);

                ms.Seek(0, SeekOrigin.Begin);

                var bmpImg = new BitmapImage();
                bmpImg.SetSource(ms.AsRandomAccessStream());
                return bmpImg;
            }
        }

    }
}
