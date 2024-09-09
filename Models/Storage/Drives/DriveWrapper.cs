#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage;
using Models.Contracts.Storage.Directory;
using Models.Enums;
using Models.ModelHelpers;
using Models.Storage.Abstractions;
using Models.Storage.Additional;
using Models.Storage.Additional.Properties;
using Models.Storage.Windows;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public sealed partial class DriveWrapper : InteractiveStorageItem, IDirectory
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

        /// <summary>
        /// Friendly name for disk (it is generated based on volume label or uses default name)
        /// </summary>
        [ObservableProperty]
        private string friendlyName;

        public bool IsReady => driveInfo.IsReady;

        public DriveWrapper(DriveInfo drive)
        {
            driveInfo = drive;
            rootDirectory = new DirectoryWrapper(driveInfo.RootDirectory);

            if (driveInfo.IsReady)
            {
                DriveSpace = new DriveSpaceInfo(drive);
                Name = drive.VolumeLabel;
            }
            else
            {
                Name = driveInfo.Name;
            }

            Path = rootDirectory.Path;
            friendlyName = driveInfo.GetFriendlyName();
        }

        public override void Rename()
        {
            driveInfo.VolumeLabel = Name;
            FriendlyName = driveInfo.GetFriendlyName();
        }

        public StorageItemProperties GetBasicProperties()
        {
            return new DriveBasicProperties(FriendlyName, Path)
            {
                DriveFormat = driveInfo.DriveFormat,
                DriveType = DriveHelper.GetStringType(driveInfo.DriveType),
                SpaceInfo = DriveSpace
            };
        }

        /// <summary>
        /// Special implementation for IStorage to return friendly name of this drive
        /// </summary>
        string IStorage.Name => FriendlyName;

        /// <inheritdoc />
        public IStorage? Parent => rootDirectory.Parent;

        /// <inheritdoc />
        public StorageContentType ContentType => StorageContentType.Files;

        /// <summary>
        /// Returns items inside <see cref="rootDirectory"/>
        /// </summary>
        public IEnumerable<IDirectoryItem> EnumerateItems(FileAttributes rejectedAttributes = 0)
        {
            // TODO: if drive is not ready
            return rootDirectory.EnumerateItems(rejectedAttributes);
        }


        /// <summary>
        /// Returns <see cref="rootDirectory"/> of drive
        /// </summary>
        public IEnumerable<IStorage> EnumerateSubDirectories()
        {
            return [rootDirectory];
        }

        public async Task<IDirectoryItem> CreateAsync(bool isDirectory)
        {
            return await rootDirectory.CreateAsync(isDirectory);
        }

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            await rootDirectory.SearchAsync(searchOptions);
        }
    }
}
