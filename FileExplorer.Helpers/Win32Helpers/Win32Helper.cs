#nullable enable
using FileExplorer.Helpers.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace FileExplorer.Helpers.Win32Helpers
{
    public static class Win32Helper
    {
        public sealed class LinkTargetData
        {
            public bool IsFolder { get; internal set; }
            public FileSystemInfo? FileInfo { get; internal set; }
        }
        public static LinkTargetData? GetLinkItem(string path)
        {
            // TODO: Link can have run as admin and other properties too
            try
            {
                using var link = new ShellLink(path);

                var linkItemData = new LinkTargetData();

                var targetPath = link.TargetPath;

                if (Path.Exists(targetPath))
                {
                    linkItemData.FileInfo = link.IsFolder ? new DirectoryInfo(targetPath) : new FileInfo(targetPath);
                    linkItemData.IsFolder = link.IsFolder;
                }

                return linkItemData;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets file's type by its path and attributes
        /// </summary>
        public static string GetItemType(string path, FileAttributes attributes)
        {
            var shInfo = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(
                path,
                attributes,
                ref shInfo,
                Marshal.SizeOf(shInfo),
                Shell32.SHGFI.SHGFI_USEFILEATTRIBUTES | Shell32.SHGFI.SHGFI_TYPENAME);

            return shInfo.szTypeName;
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
