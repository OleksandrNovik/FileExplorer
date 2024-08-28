﻿#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage;
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
    public sealed partial class DriveWrapper : InteractiveStorageItem, IStorage<IDirectoryItem>
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

        public DriveWrapper(DriveInfo drive)
        {
            driveInfo = drive;
            rootDirectory = new DirectoryWrapper(driveInfo.RootDirectory);
            DriveSpace = new DriveSpaceInfo(drive);

            name = drive.VolumeLabel;
            Path = rootDirectory.Path;
            friendlyName = driveInfo.GetFriendlyName();
            Thumbnail.ItemPath = rootDirectory.Path;
        }

        public override void Rename()
        {
            driveInfo.VolumeLabel = Name;
            FriendlyName = driveInfo.GetFriendlyName();
        }

        public override IBasicStorageItemProperties GetBasicProperties()
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
        string IStorage<IDirectoryItem>.Name => FriendlyName;

        /// <inheritdoc />
        public IStorage<IDirectoryItem>? Parent => rootDirectory.Parent;

        /// <inheritdoc />
        public StorageContentType ContentType => StorageContentType.Files;

        /// <summary>
        /// Returns items inside <see cref="rootDirectory"/>
        /// </summary>
        public IEnumerable<IDirectoryItem> EnumerateItems()
        {
            return rootDirectory.EnumerateItems();
        }

        /// <summary>
        /// Returns <see cref="rootDirectory"/> of drive
        /// </summary>
        public IEnumerable<IStorage<IDirectoryItem>> EnumerateSubDirectories()
        {
            return [rootDirectory];
        }

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            await rootDirectory.SearchAsync(searchOptions);
        }
    }
}
