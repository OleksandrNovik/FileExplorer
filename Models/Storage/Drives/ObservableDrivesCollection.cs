using Helpers.General;
using Models.Contracts;
using Models.Contracts.Storage;
using Models.General;
using Models.Storage.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public class ObservableDrivesCollection : ObservableCollection<DriveWrapper>, IStorage<DirectoryItemWrapper>
    {
        public string Name => "Home";
        public string Path => string.Empty;
        public IStorage<DirectoryItemWrapper> Parent => null;

        public ObservableDrivesCollection()
        {
            //TODO: we can handle not ready drives later
            var availableDrives = DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .Select(drive => new DriveWrapper(drive));

            Items.AddRange(availableDrives);
        }

        public IEnumerable<DirectoryItemWrapper> EnumerateItems()
        {
            foreach (var drive in this)
            {
                foreach (var item in drive.EnumerateItems())
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<IStorage<DirectoryItemWrapper>> EnumerateSubDirectories() => this;

        public async Task SearchAsync(IEnqueuingCollection<DirectoryItemWrapper> destination, SearchOptionsModel options, CancellationToken token)
        {
            await Parallel.ForEachAsync(this, new ParallelOptions
            {
                CancellationToken = token,
                MaxDegreeOfParallelism = 1

            }, async (drive, cancellationToken) =>
            {
                await drive.SearchAsync(destination, options, cancellationToken);
            });
        }
    }
}
