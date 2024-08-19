#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts;
using Models.Contracts.Additional;
using Models.Contracts.Storage;
using Models.General;
using Models.ModelHelpers;
using Models.Storage.Abstractions;
using Models.Storage.Additional;
using Models.Storage.Windows;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public sealed partial class DriveWrapper : RenamableObject, ISearchCatalog<DirectoryItemWrapper>, IThumbnailProvider
    {
        public IThumbnail Thumbnail { get; } = new Thumbnail();
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

        /// <inheritdoc />
        public IEnumerable<DirectoryItemWrapper> EnumerateItems(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return rootDirectory.EnumerateItems(pattern, option);
        }

        /// <inheritdoc />
        public IEnumerable<DirectoryItemWrapper> EnumerateItems(EnumerationOptions enumeration, string pattern = "*")
        {
            return rootDirectory.EnumerateItems(enumeration, pattern);
        }

        /// <inheritdoc />
        public async Task SearchAsync(IEnqueuingCollection<DirectoryItemWrapper> destination, SearchOptionsModel options, CancellationToken token)
        {
            await rootDirectory.SearchAsync(destination, options, token);
        }

        public void Rename()
        {
            driveInfo.VolumeLabel = Name;
            FriendlyName = driveInfo.GetFriendlyName();
        }

        public async Task UpdateThumbnailAsync(int size)
        {
            await Thumbnail.UpdateAsync(size);
        }
    }
}
