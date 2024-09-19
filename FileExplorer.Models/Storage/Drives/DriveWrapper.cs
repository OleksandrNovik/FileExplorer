#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Enums;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.Models.Storage.Additional;
using FileExplorer.Models.Storage.Additional.Properties;
using FileExplorer.Models.Storage.Windows;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.Models.Storage.Drives
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


        public IEnumerable<IStorage> EnumerateSubDirectories(FileAttributes rejectedAttributes = 0)
        {
            return rootDirectory.EnumerateSubDirectories(rejectedAttributes);
        }

        public IDirectoryItem Create(bool isDirectory)
        {
            return rootDirectory.Create(isDirectory);
        }

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            await rootDirectory.SearchAsync(searchOptions);
        }

        public IEnumerable<IDirectoryItem> EnumerateFiles(FileAttributes rejectedAttributes = FileAttributes.None)
        {
            return rootDirectory.EnumerateFiles(rejectedAttributes);
        }
    }
}
