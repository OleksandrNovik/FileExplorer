#nullable enable
using Helpers.Imaging;
using System.Drawing;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace Helpers.Win32Helpers
{
    public static class Win32Helper
    {

        public static byte[]? GetIcon(string path, int size, bool useCache = false)
        {
            var shellItem = ShellItem.Open(path);
            var flags = Shell32.SIIGBF.SIIGBF_BIGGERSIZEOK;
            byte[]? iconBytes = null;

            if (shellItem is not null && shellItem.IShellItem is Shell32.IShellItemImageFactory shellFactory)
            {
                if (useCache)
                {
                    flags |= Shell32.SIIGBF.SIIGBF_INCACHEONLY;
                }

                var hResult = shellFactory.GetImage(new SIZE(size, size), flags, out var hBitmap);

                if (hResult == HRESULT.S_OK)
                {
                    using var image = BitmapHelper.GetBitmapFromHBitmap(hBitmap);

                    if (image is not null)
                        iconBytes = new ImageConverter().ConvertTo(image, typeof(byte[])) as byte[];
                }
            }

            return iconBytes;
        }
    }
}
