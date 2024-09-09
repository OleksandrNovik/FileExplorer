using Helpers;
using Helpers.General;
using Models.Contracts;
using Models.Contracts.Storage.Directory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using DirectoryItemWrapper = Models.Storage.Windows.DirectoryItemWrapper;

namespace Models
{
    /// <summary>
    /// Wrapper on <see cref="ObservableCollection{T}"/> to add <see cref="DirectoryItemWrapper"/> asynchronously
    /// </summary>
    public class ConcurrentWrappersCollection : ObservableCollection<IDirectoryItem>, IEnqueuingCollection<IDirectoryItem>
    {
        public ConcurrentWrappersCollection() { }

        public ConcurrentWrappersCollection(IEnumerable<IDirectoryItem> enumerable) : base(enumerable) { }

        /// <summary>
        /// Enqueue adding enumeration of <see cref="IDirectoryItem"/> in main thread
        /// Also updates icons of added items
        /// </summary>
        /// <param name="items"> Items to add to the directory content collection </param>
        /// <param name="token"> Token to cancel operation if needed </param>
        public async Task EnqueueEnumerationAsync(IEnumerable<IDirectoryItem> items, CancellationToken token)
        {
            foreach (var item in items)
            {
                token.ThrowIfCancellationRequested();

                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    Add(item);
                    await item.UpdateThumbnailAsync(Constants.ThumbnailSizes.Big);
                });
            }
        }

        public async Task UpdateIconsAsync(int size, CancellationToken token)
        {
            foreach (var item in Items)
            {
                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    await item.UpdateThumbnailAsync(size);
                });
            }
        }
    }
}
