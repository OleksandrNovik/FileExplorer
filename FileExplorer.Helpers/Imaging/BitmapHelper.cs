#nullable enable
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace FileExplorer.Helpers.Imaging
{
    public static class BitmapHelper
    {
        /// <summary>
        /// Creates bitmap from <see cref="HBITMAP"/> 
        /// </summary>
        /// <param name="hBitmap"> Provided handle to bitmap </param>
        public static Bitmap? GetBitmapFromHBitmap(HBITMAP hBitmap)
        {
            try
            {
                Bitmap bmp = Image.FromHbitmap((IntPtr)hBitmap);

                if (Image.GetPixelFormatSize(bmp.PixelFormat) < 32)
                    return bmp;

                var bmBounds = new Rectangle(0, 0, bmp.Width, bmp.Height);
                var bmpData = bmp.LockBits(bmBounds, ImageLockMode.ReadOnly, bmp.PixelFormat);

                if (bmpData.HasAlphaBitmap())
                {
                    var alpha = bmpData.GetAlphaBitmap();

                    bmp.UnlockBits(bmpData);
                    bmp.Dispose();

                    return alpha;
                }

                bmp.UnlockBits(bmpData);

                return bmp;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates <see cref="Bitmap" /> image from <see cref="BitmapData"/> that has any alpha pixels
        /// </summary>
        /// <param name="bmpData"> <see cref="BitmapData"/> that is used to create alpha <see cref="Bitmap"/> </param>
        private static Bitmap GetAlphaBitmap(this BitmapData bmpData)
        {
            using var tmp = new Bitmap(bmpData.Width, bmpData.Height, bmpData.Stride, PixelFormat.Format32bppArgb, bmpData.Scan0);
            Bitmap clone = new Bitmap(tmp.Width, tmp.Height, tmp.PixelFormat);

            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(tmp, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            return clone;
        }

        /// <summary>
        /// Identifier if bitmap has alpha pixels
        /// </summary>
        /// <param name="bmpData"> <see cref="BitmapData"/> that is checked </param>
        /// <returns> true if provided <see cref="BitmapData"/> has alpha pixels </returns>
        private static bool HasAlphaBitmap(this BitmapData bmpData)
        {
            for (int y = 0; y <= bmpData.Height - 1; y++)
            {
                for (int x = 0; x <= bmpData.Width - 1; x++)
                {
                    var pixelColor = Color.FromArgb(
                        Marshal.ReadInt32(bmpData.Scan0, (bmpData.Stride * y) + (4 * x)));

                    if (pixelColor.A > 0 & pixelColor.A < 255)
                        return true;
                }
            }

            return false;
        }
    }
}
