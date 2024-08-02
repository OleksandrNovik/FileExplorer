using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;
using Models.StorageWrappers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Models.Services
{
    public class ConcurrentAttachingService
    {
        private readonly ObservableWrappersCollection collection;
        private readonly DispatcherQueue dispatcher;

        public ConcurrentAttachingService(ObservableWrappersCollection collection) :
            this(collection, DispatcherQueue.GetForCurrentThread())
        { }

        public ConcurrentAttachingService(ObservableWrappersCollection collection, DispatcherQueue mainThreadDispatcher)
        {
            this.collection = collection;
            dispatcher = mainThreadDispatcher;
        }

        public async Task AddEnumerationAsync(IEnumerable<DirectoryItemWrapper> items)
        {
            await Parallel.ForEachAsync(items, async (item, token) =>
            {
                await dispatcher.EnqueueAsync(async () =>
                {
                    collection.Add(item);

                    if ((item.Attributes & FileAttributes.Hidden) == 0)
                    {
                        item.Thumbnail = new BitmapImage();
                        await item.UpdateThumbnailAsync();
                    }
                });
            });
        }
    }
}
