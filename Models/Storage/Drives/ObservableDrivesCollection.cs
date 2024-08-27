using Helpers.General;
using Models.Contracts.Storage;
using Models.Enums;
using Models.Storage.Additional;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public class ObservableDrivesCollection : ObservableCollection<DriveWrapper>, IStorage<IDirectoryItem>
    {
        public string Name => "Home";
        public string Path => string.Empty;
        public IStorage<IDirectoryItem> Parent => null;
        public StorageContentType ContentType => StorageContentType.Drives;

        public ObservableDrivesCollection()
        {
            //TODO: we can handle not ready drives later
            var availableDrives = DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .Select(drive => new DriveWrapper(drive));

            Items.AddRange(availableDrives);
        }


        public IEnumerable<IDirectoryItem> EnumerateItems()
        {
            foreach (var drive in this)
            {
                foreach (var item in drive.EnumerateItems())
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<IStorage<IDirectoryItem>> EnumerateSubDirectories() => this;

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            searchOptions.MaxDirectoriesPerThread = 10;

            var drivesSearch = this.Select(async drive =>
            {
                await drive.SearchAsync(searchOptions);
            });

            await Task.WhenAll(drivesSearch);
        }
    }
}
