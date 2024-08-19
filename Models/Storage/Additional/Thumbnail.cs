#nullable enable
using Microsoft.UI.Xaml.Media.Imaging;
using Models.Contracts.Additional;
using Models.ModelHelpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage.Additional
{
    /// <inheritdoc />
    public sealed class Thumbnail : IThumbnail
    {
        public string ItemPath { get; set; }

        /// <inheritdoc />
        public BitmapImage? Source { get; private set; }

        /// <inheritdoc />
        public int Size { get; private set; }

        /// <inheritdoc />
        public async Task UpdateAsync(int size)
        {
            Size = size;
            Source ??= new BitmapImage();

            var iconBytes = IconHelper.GetThumbnailFromPath(ItemPath, size);

            if (iconBytes is not null)
            {
                using (var ms = new MemoryStream(iconBytes))
                {
                    await Source.SetSourceAsync(ms.AsRandomAccessStream());
                }
            }
        }

        public void Update(int size)
        {
            Size = size;
            Source ??= new BitmapImage();

            var iconBytes = IconHelper.GetThumbnailFromPath(ItemPath, size);

            if (iconBytes is not null)
            {
                using (var ms = new MemoryStream(iconBytes))
                {
                    Source.SetSource(ms.AsRandomAccessStream());
                }
            }
        }
    }
}
