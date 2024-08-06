using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Models.Contracts;
using Models.StorageWrappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Wrapper on <see cref="ObservableCollection{T}"/> to add <see cref="DirectoryItemWrapper"/> asynchronously
    /// </summary>
    public class ConcurrentWrappersCollection : ObservableCollection<DirectoryItemWrapper>, IEnqueuingCollection<DirectoryItemWrapper>
    {
        /// <summary>
        /// Dispatcher that provides access to main thread (where collection is created)
        /// </summary>
        private readonly DispatcherQueue dispatcher = DispatcherQueue.GetForCurrentThread();

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

                await dispatcher.EnqueueAsync(async () =>
                {
                    Add(item);

                    if ((item.Attributes & FileAttributes.Hidden) == 0)
                    {
                        await item.UpdateThumbnailAsync();
                    }
                });
            }
        }
    }
}
