using Models.Contracts;
using Models.Contracts.Storage;
using Models.General;
using Models.Storage.Windows;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Storage.Drives
{
    public sealed class DrivesInformationWrapper : RenamableObject, ISearchCatalog<DirectoryItemWrapper>
    {
        /// <summary>
        /// Rive info that wrapper is containing
        /// </summary>
        private DriveInfo driveInfo;

        /// <summary>
        /// Root directory of drive
        /// </summary>
        private DirectoryWrapper rootDirectory;

        public DrivesInformationWrapper(DriveInfo drive)
        {
            driveInfo = drive;
            rootDirectory = new DirectoryWrapper(driveInfo.RootDirectory);
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
    }
}
