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
    /// <summary>
    /// Collection of drives that is shown in the home page
    /// </summary>
    public class ObservableDrivesCollection : ObservableCollection<DriveWrapper>, IStorage
    {
        /// <summary>
        /// Name of collection is "Home" to represent home page
        /// </summary>
        public string Name => "Home";

        /// <summary>
        /// Path to home page
        /// </summary>
        public string Path => string.Empty;

        /// <summary>
        /// No parent for a home page
        /// </summary>
        public IStorage Parent => null;

        /// <inheritdoc />
        public StorageContentType ContentType => StorageContentType.Drives;

        public ObservableDrivesCollection(IEnumerable<DriveWrapper> availableDrives)
        {
            Items.AddRange(availableDrives);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IEnumerable<IStorage> EnumerateSubDirectories(FileAttributes rejectedAttributes = 0) => this;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IEnumerable<IDirectoryItem> EnumerateFiles(FileAttributes rejectedAttributes = 0)
        {
            foreach (var drive in this)
            {
                foreach (var item in drive.EnumerateFiles(rejectedAttributes))
                {
                    yield return item;
                }
            }
        }
    }
}
