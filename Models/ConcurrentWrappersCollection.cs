using Helpers.General;
using Models.Contracts;
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
    public class ConcurrentWrappersCollection : ObservableCollection<DirectoryItemWrapper>, IEnqueuingCollection<DirectoryItemWrapper>
    {
        /// <summary>
        /// Enqueue adding enumeration of <see cref="DirectoryItemWrapper"/> in main thread
        /// Also updates icons of added items
        /// </summary>
        /// <param name="items"> Items to add to the directory content collection </param>
        /// <param name="token"> Token to cancel operation if needed </param>
        public async Task EnqueueEnumerationAsync(IEnumerable<DirectoryItemWrapper> items, CancellationToken token)
        {
            foreach (var item in items)
            {
                token.ThrowIfCancellationRequested();

                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    Add(item);
                    await item.UpdateThumbnailAsync(90);
                });
            }
        }
    }
}
