#nullable enable
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Models.Contracts.Additional
{
    /// <summary>
    /// Wrapper for a storage item's thumbnail
    /// </summary>
    public interface IThumbnail : INotifyPropertyChanged
    {
        /// <summary>
        /// Source image of thumbnail
        /// </summary>
        public BitmapImage? Source { get; }

        /// <summary>
        /// Current size of thumbnail
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Updates thumbnail for an item
        /// </summary>
        /// <param name="size"> Required size of the thumbnail </param>
        public Task UpdateAsync(string path, int size);

        public void Update(string path, int size);

    }
}
