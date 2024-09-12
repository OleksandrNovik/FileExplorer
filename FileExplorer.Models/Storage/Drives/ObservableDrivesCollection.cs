using FileExplorer.Helpers.General;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Enums;
using FileExplorer.Models.Storage.Additional;
using FileExplorer.Models.Storage.Enumerators;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.Models.Storage.Drives
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

        /// <summary>
        /// Tries to get drive from collection accessing it by its root path
        /// </summary>
        /// <param name="path"> Path to the root directory of drive </param>
        /// <param name="drive"> Resulting drive parameter </param>
        /// <returns> True if drive exists, False if there is no such drive in collection </returns>
        public bool TryGetDrive(string path, out DriveWrapper drive)
        {
            drive = Items.FirstOrDefault(d => d.Path == path);

            return drive is not null;
        }
    }
}
