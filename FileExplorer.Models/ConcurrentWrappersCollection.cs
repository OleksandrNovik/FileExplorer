using FileExplorer.Helpers;
using FileExplorer.Helpers.General;
using FileExplorer.Models.Contracts;
using FileExplorer.Models.Contracts.Storage.Directory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DirectoryItemWrapper = FileExplorer.Models.Storage.Windows.DirectoryItemWrapper;

namespace FileExplorer.Models
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

        public void RemovePaths(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var item = this.FirstOrDefault(i => i.Path == path);

                if (item is not null)
                {
                    Remove(item);
                }
            }
        }
    }
}
