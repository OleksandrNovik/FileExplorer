using Helpers.General;
using Models.Contracts.Storage;
using Models.Contracts.Storage.Directory;
using Models.Enums;
using Models.Storage.Additional;
using Models.Storage.Enumerators;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public class ObservableDrivesCollection : ObservableCollection<DriveWrapper>, IStorage
    {
        public string Name => "Home";
        public string Path => string.Empty;
        public IStorage Parent => null;
        public StorageContentType ContentType => StorageContentType.Drives;

        public ObservableDrivesCollection(IEnumerable<DriveWrapper> availableDrives)
        {
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
            using var enumerator = new StorageCollectionEnumerator(this);

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
