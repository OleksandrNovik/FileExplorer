#nullable enable
using FileExplorer.Helpers.General;
using FileExplorer.Models.Contracts.ModelServices;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Enums;
using FileExplorer.Models.General;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Additional;
using FileExplorer.Models.Storage.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using FileAttributes = System.IO.FileAttributes;
using IOPath = System.IO.Path;

namespace FileExplorer.Models.Storage.Windows
{
    public sealed class DirectoryWrapper : DirectoryItemWrapper, IDirectory
    {
        private StorageFolder? asStorageFolder;

        private readonly IWindowsDirectoryItemsFactory factory = new WindowsDirectoryItemsFactory();

        public DirectoryWrapper() { }
        public DirectoryWrapper(DirectoryInfo info) : base(info) { }
        public DirectoryWrapper(string path) : base(new DirectoryInfo(path)) { }

        #region ISearchCatalog logic

        public async Task SearchAsync(SearchOptions searchOptions)
        {
            await ShallowSearchAsync(searchOptions);

            if (searchOptions.Filter.IsNestedSearch)
            {
                await EnumerateSubDirectories().SearchCatalogsAsync(searchOptions);
            }
        }

        /// <summary>
        /// Initiates shallow search (only-top level of current directory)
        /// </summary>
        /// <param name="searchOptions"> Provided options for this search </param>
        public async Task ShallowSearchAsync(SearchOptions searchOptions)
        {
            await searchOptions.Destination.EnqueueEnumerationAsync(await SearchDirectory(searchOptions.Filter), searchOptions.Token);
        }

        private async Task<IDirectoryItem[]> SearchDirectory(SearchFilter options)
        {
            return await Task.Run(() =>
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
                        StringExtensions.ContainsPattern(item.Name, options.SearchName));
                }

                return found.ToArray();
            });
        }

        #endregion

        #region IStorage logic

        public IStorage? Parent => GetParentDirectory();
        public StorageContentType ContentType => StorageContentType.Files;

        public IEnumerable<IDirectoryItem> EnumerateItems(FileAttributes rejectedAttributes = 0)
        {
            //TODO: Handle deleted folder and open tab with that folder
            return EnumerateWrappers(System.IO.Directory.EnumerateFileSystemEntries(Path), rejectedAttributes);
        }
        public IEnumerable<IStorage> EnumerateSubDirectories(FileAttributes rejectedAttributes = 0)
        {
            try
            {
                return System.IO.Directory.EnumerateDirectories(Path).Select(path => new DirectoryWrapper(path));
            }
            catch
            {
                return [];
            }
        }

        #endregion

        #region IDirectory logic

        /// <inheritdoc />
        public IDirectoryItem Create(bool isDirectory)
        {
            DirectoryItemWrapper element = isDirectory ? new DirectoryWrapper() : new FileWrapper();

            element.CreatePhysical(Path);

            return element;
        }

        #endregion

        private IEnumerable<DirectoryItemWrapper> EnumerateItems(EnumerationOptions enumeration, string pattern = "*")
        {
            return EnumerateWrappers(System.IO.Directory.EnumerateFileSystemEntries(Path, pattern, enumeration));
        }

        private IEnumerable<DirectoryItemWrapper> EnumerateWrappers(IEnumerable<string> paths, FileAttributes skipped = 0)
        {
            foreach (var path in paths)
            {
                var wrapper = factory.Create(path);

                // Item has attributes that should be skipped
                if (wrapper.HasAttributes(skipped))
                    continue;

                yield return wrapper;
            }
        }

        public IEnumerable<IDirectoryItem> EnumerateFiles(FileAttributes rejectedAttributes = FileAttributes.None)
        {
            return EnumerateWrappers(System.IO.Directory.EnumerateFiles(Path), rejectedAttributes);
        }

        /// <inheritdoc />
        public override IDirectoryItem Copy(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, Name + " - Copy");

            var newPath = CopyPhysical(destination, uniqueName);
            var currentPath = IOPath.Combine(destination, uniqueName);

            foreach (var item in EnumerateItems())
            {
                item.Copy(currentPath);
            }

            return new DirectoryWrapper(newPath);
        }

        protected override string CopyPhysical(string destination, string newName)
        {
            var newPath = IOPath.Combine(destination, newName);
            System.IO.Directory.CreateDirectory(newPath);

            return newPath;
        }

        /// <inheritdoc />
        public override void Move(string destination)
        {
            Name = GenerateUniqueName(destination, Name);
            var newPath = IOPath.Combine(destination, Name);

            // Folder is being moved to the same directory
            if (newPath == Path) return;

            System.IO.Directory.Move(Path, newPath);
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
            System.IO.Directory.Delete(Path, true);
        }

        /// <inheritdoc />
        public override void CreatePhysical(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, "New Folder");
            var newPath = IOPath.Combine(destination, uniqueName);
            info = System.IO.Directory.CreateDirectory(newPath);
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
