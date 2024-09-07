#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Models.Contracts.Additional;
using Models.ModelHelpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage.Additional
{
    public sealed class Thumbnail : ObservableObject, IThumbnail
    {
        /// <inheritdoc cref="Source" />
        public BitmapImage? Source { get; private set; }

        /// <inheritdoc />
        public int Size { get; private set; }

        /// <inheritdoc />
        public async Task UpdateAsync(string path, int size)
        {
            Size = size;
            Source ??= new BitmapImage();

            var iconBytes = IconHelper.GetThumbnailFromPath(path, size);

            if (iconBytes is not null)
            {
                using (var ms = new MemoryStream(iconBytes))
                {
                    await Source.SetSourceAsync(ms.AsRandomAccessStream());
                }
            }
        }

        public void Update(string path, int size)
        {
            Size = size;
            Source ??= new BitmapImage();

            var iconBytes = IconHelper.GetThumbnailFromPath(path, size);

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
