#nullable enable
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

namespace Models.Contracts.Additional
{
    /// <summary>
    /// Wrapper for a storage item's thumbnail
    /// </summary>
    public interface IThumbnail
    {
        /// <summary>
        /// Source image of thumbnail
        /// </summary>
        public BitmapImage? Source { get; }

        /// <summary>
        /// Current size of thumbnail
        /// </summary>
        public int Size { get; }
        public string ItemPath { get; set; }

        /// <summary>
        /// Updates thumbnail for an item
        /// </summary>
        /// <param name="size"> Required size of the thumbnail </param>
        public Task UpdateAsync(int size);

        public void Update(int size);

    }
}
