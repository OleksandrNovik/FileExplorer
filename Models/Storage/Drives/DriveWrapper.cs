#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage;
using Models.ModelHelpers;
using Models.Storage.Abstractions;
using Models.Storage.Additional;
using Models.Storage.Windows;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public sealed partial class DriveWrapper : InteractiveStorageItem, IStorage<DirectoryItemWrapper>
    {
        /// <summary>
        /// Rive info that wrapper is containing
        /// </summary>
        private readonly DriveInfo driveInfo;

        /// <summary>
        /// Root directory of drive
        /// </summary>
        private readonly DirectoryWrapper rootDirectory;

        /// <summary>
        /// Information about space on drive
        /// </summary>
        public DriveSpaceInfo DriveSpace { get; }

        [ObservableProperty]
        private string friendlyName;

        public DriveWrapper(DriveInfo drive)
        {
            driveInfo = drive;
            rootDirectory = new DirectoryWrapper(driveInfo.RootDirectory);
            DriveSpace = new DriveSpaceInfo(drive);

            name = driveInfo.VolumeLabel;
            Path = rootDirectory.Path;
            friendlyName = driveInfo.GetFriendlyName();
            Thumbnail.ItemPath = rootDirectory.Path;
        }

        public void Rename()
        {
            driveInfo.VolumeLabel = Name;
            FriendlyName = driveInfo.GetFriendlyName();
        }

        public IStorage<DirectoryItemWrapper>? Parent => rootDirectory.Parent;
        public StorageContentType ContentType => StorageContentType.Files;

        /// <summary>
        /// Returns items inside <see cref="rootDirectory"/>
        /// </summary>
        public IEnumerable<DirectoryItemWrapper> EnumerateItems()
        {
            return rootDirectory.EnumerateItems();
        }

        /// <summary>
        /// Returns <see cref="rootDirectory"/> of drive
        /// </summary>
        public IEnumerable<IStorage<DirectoryItemWrapper>> EnumerateSubDirectories()
        {
            return [rootDirectory];
        }

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            await rootDirectory.SearchAsync(searchOptions);
        }
    }
}
