#nullable enable
using Helpers.General;
using Models.Contracts.Storage;
using Models.Enums;
using Models.General;
using Models.ModelHelpers;
using Models.Storage.Additional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using IOPath = System.IO.Path;

namespace Models.Storage.Windows
{
    public sealed class DirectoryWrapper : DirectoryItemWrapper, IDirectory
    {
        private StorageFolder? asStorageFolder;
        public DirectoryWrapper() { }
        public DirectoryWrapper(DirectoryInfo info) : base(info) { }
        public DirectoryWrapper(string path) : base(new DirectoryInfo(path)) { }

        #region ISearchCatalog logic

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            await ShallowSearchAsync(searchOptions);

            if (searchOptions.Filter.IsNestedSearch)
            {
                var subDirectories = EnumerateSubDirectories();

                if (searchOptions.OptimizationsEnabled)
                {
                    await subDirectories.ToArray().OptimizedSearchAsync(searchOptions);
                }
                else
                {
                    await subDirectories.SearchCatalogsAsync(searchOptions);
                }
            }
        }

        /// <summary>
        /// Initiates shallow search (only-top level of current directory)
        /// </summary>
        /// <param name="searchOptions"> Provided options for this search </param>
        public async Task ShallowSearchAsync(SearchOptions searchOptions)
        {
            await searchOptions.Destination.EnqueueEnumerationAsync(SearchDirectory(searchOptions.Filter), searchOptions.Token);
        }

        private IEnumerable<IDirectoryItem> SearchDirectory(SearchFilter options)
        {
            var enumeration = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false,
                MatchCasing = MatchCasing.CaseInsensitive,
            };

            var found = EnumerateItems(enumeration, options.SearchPattern)
                // Any item that has size in range
                .Where(item => options.SizeChecker.Satisfies(item.Size))
                // Any item that is used at provided date range
                .Where(item => options.AccessDateChecker.Satisfies(item.LastAccess))
                // Any file types that satisfy filter
                .Where(item => options.ExtensionFilter.Invoke(item.Name));

            if (options.SearchName is not null)
            {
                found = found.Where(item =>
                    item.Name.ContainsPattern(options.SearchName));
            }

            return found;
        }

        #endregion

        #region IStorage logic

        public IStorage<IDirectoryItem>? Parent => GetParentDirectory();
        public StorageContentType ContentType => StorageContentType.Files;

        public IEnumerable<IDirectoryItem> EnumerateItems()
        {
            return EnumerateWrappers(Directory.EnumerateFileSystemEntries(Path));
        }
        public IEnumerable<IStorage<IDirectoryItem>> EnumerateSubDirectories()
        {
            try
            {
                return Directory.EnumerateDirectories(Path).Select(path => new DirectoryWrapper(path));
            }
            catch
            {
                return [];
            }
        }

        #endregion

        #region IStorageElementCreator logic

        /// <inheritdoc />
        public async Task<IDirectoryItem> CreateAsync(bool isDirectory)
        {
            DirectoryItemWrapper element = isDirectory ? new DirectoryWrapper() : new FileWrapper();

            element.CreatePhysical(Path);

            await element.UpdateThumbnailAsync(Thumbnail.Size);

            return element;
        }

        #endregion

        private IEnumerable<DirectoryItemWrapper> EnumerateItems(EnumerationOptions enumeration, string pattern = "*")
        {
            return EnumerateWrappers(Directory.EnumerateFileSystemEntries(Path, pattern, enumeration));
        }

        private IEnumerable<DirectoryItemWrapper> EnumerateWrappers(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                if (File.Exists(path))
                    yield return new FileWrapper(path);
                else
                    yield return new DirectoryWrapper(path);
            }
        }

        /// <inheritdoc />
        public override void Copy(string destination)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Move(string destination)
        {
            Name = GenerateUniqueName(destination, Name);
            var newPath = IOPath.Combine(destination, Name);

            // Folder is being moved to the same directory
            if (newPath == Path) return;

            Directory.Move(Path, newPath);
            info = new DirectoryInfo(newPath);
            InitializeData();
            asStorageFolder = null;
        }

        /// <inheritdoc />
        public override async Task RecycleAsync()
        {
            var storageFolder = await AsStorageFolderAsync();
            await storageFolder.DeleteAsync(StorageDeleteOption.Default);
        }

        /// <inheritdoc />
        public override void Delete()
        {
            Directory.Delete(Path, true);
        }

        /// <inheritdoc />
        public override void CreatePhysical(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, "New Folder");
            var newPath = IOPath.Combine(destination, uniqueName);
            info = Directory.CreateDirectory(newPath);
            InitializeData();
        }

        /// <inheritdoc />
        public override async Task<IStorageItemProperties> GetStorageItemPropertiesAsync()
        {
            return await AsStorageFolderAsync();
        }

        /// <inheritdoc />
        public override DirectoryWrapper GetCurrentDirectory() => this;
        private async Task<StorageFolder> AsStorageFolderAsync()
        {
            if (asStorageFolder is null)
            {
                asStorageFolder = await StorageFolder.GetFolderFromPathAsync(Path);
            }

            return asStorageFolder;
        }

    }
}
