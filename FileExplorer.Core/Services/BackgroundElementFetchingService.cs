#nullable enable
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Models;
using Models.StorageWrappers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.Core.Services
{
    public class BackgroundElementFetchingService
    {
        private readonly DispatcherQueue dispatcher = DispatcherQueue.GetForCurrentThread();
        public async Task AttachElementAsync(ObservableWrappersCollection source, ParallelQuery<DirectoryItemWrapper> items, CancellationToken token)
        {
            await Task.Run(() =>
            {
                using var found = items.GetEnumerator();
                int itemsFetched = 20;

                while (true)
                {
                    Debug.Assert(found is not null);

                    var bunch = new List<DirectoryItemWrapper>(itemsFetched);

                    for (int i = 0; i < itemsFetched && found.MoveNext(); i++)
                    {
                        bunch.Add(found.Current);
                    }

                    if (token.IsCancellationRequested)
                        break;

                    dispatcher.EnqueueAsync(async () =>
                    {
                        await source.AddEnumeration(bunch);
                    });

                    if (bunch.Count < itemsFetched)
                        break;
                }
                Debug.WriteLine("------------------ Task DONE -------------------");

            }, token);
        }

    }
}
