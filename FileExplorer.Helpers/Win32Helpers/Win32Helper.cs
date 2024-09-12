#nullable enable
using FileExplorer.Helpers.Imaging;
using System;
using System.Drawing;
using System.IO;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace FileExplorer.Helpers.Win32Helpers
{
    public static class Win32Helper
    {
        public sealed class LinkTargetData
        {
            public FileSystemInfo? FileInfo { get; internal set; }
        }
        public static LinkTargetData GetLinkItem(string path)
        {
            // TODO: Link can have run as admin and other properties too
            using var link = new ShellLink(path, LinkResolution.NoUIWithMsgPump, default, TimeSpan.FromMilliseconds(100));
            var linkItemData = new LinkTargetData();

            try
            {
                var targetPath = Environment.ExpandEnvironmentVariables(link.TargetPath) ?? "";

                if (Path.Exists(targetPath))
                {
                    linkItemData.FileInfo = link.Target.IsFolder ?
                        new DirectoryInfo(targetPath) :
                        new FileInfo(targetPath);
                }
            }
            catch
            {
            }

            return linkItemData;
        }

        /// <summary>
        /// Gets bytes for a thumbnail image using windows api if it is possible
        /// </summary>
        /// <param name="path"> Path that identifies storage element location </param>
        /// <param name="size"> Required icon size </param>
        /// <param name="useCache"> if true only uses cache to get icon (if icon is not in cache returns null) </param>
        /// <returns> Byte array of icon image for a storage item </returns>
        public static byte[]? GetIcon(string path, int size, bool useCache = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            byte[]? iconBytes = null;

            var shellItem = ShellItem.Open(path);
            var flags = Shell32.SIIGBF.SIIGBF_BIGGERSIZEOK;

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
