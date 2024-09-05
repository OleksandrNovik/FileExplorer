using Helpers.General;
using Models.Contracts.Storage;
using Models.Enums;
using Models.General;
using Models.Storage.Additional;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public class ObservableDrivesCollection : ObservableCollection<DriveWrapper>, IStorage
    {
        public string Name => "Home";
        public string Path => string.Empty;
        public IStorage Parent => null;
        public StorageContentType ContentType => StorageContentType.Drives;

        public ObservableDrivesCollection()
        {
            //TODO: we can handle not ready drives later
            var availableDrives = DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .Select(drive => new DriveWrapper(drive));

            Items.AddRange(availableDrives);
        }


        public IEnumerable<IDirectoryItem> EnumerateItems(FileAttributes rejectedAttributes = 0)
        {
            foreach (var drive in this)
            {
                foreach (var item in drive.EnumerateItems(rejectedAttributes))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<IStorage> EnumerateSubDirectories() => this;

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            //var drivesSearch = this.Select(async drive =>
            //{
            //    await drive.SearchAsync(searchOptions);
            //});

            //await Task.WhenAll(drivesSearch);

            using var enumerator = new DrivesCollectionEnumerator(this);

            while (enumerator.MoveNext())
            {
                Debug.Assert(enumerator.Current is not null);

                await Task.Run(async () =>
                {
                    await enumerator.Current.SearchAsync(searchOptions);
                });

            }
        }
    }
}
