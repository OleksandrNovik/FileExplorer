#nullable enable
using Models.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using IOPath = System.IO.Path;
using SearchOption = System.IO.SearchOption;

namespace Models.StorageWrappers
{
    public sealed class DirectoryWrapper : DirectoryItemWrapper, ISearchable<ConcurrentWrappersCollection>
    {
        private StorageFolder? asStorageFolder;
        public DirectoryWrapper() { }
        public DirectoryWrapper(DirectoryInfo info) : base(info) { }
        public DirectoryWrapper(string path) : base(new DirectoryInfo(path)) { }

        public IEnumerable<ISearchable<ConcurrentWrappersCollection>> EnumerateSubDirectories()
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

        public IEnumerable<DirectoryItemWrapper> EnumerateItems(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return EnumerateWrappers(Directory.EnumerateFileSystemEntries(Path, pattern, option));
        }

        public IEnumerable<DirectoryItemWrapper> EnumerateItems(EnumerationOptions enumeration, string pattern = "*")
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

        public async Task SearchAsync(ConcurrentWrappersCollection destination, SearchOptionsModel options, CancellationToken token)
        {
            await ShallowSearchAsync(destination, options, token);

            if (options.IsNestedSearch)
            {
                await SearchSubdirectoriesAsync(destination, options, token);
            }
        }

        /// <summary>
        /// Initiates shallow search (only-top level of current directory)
        /// </summary>
        /// <param name="destination"> Destination collection to add items into </param>
        /// <param name="options"> Search options for a current search </param>
        /// <param name="token"> Token for canceling operation </param>
        public async Task ShallowSearchAsync(ConcurrentWrappersCollection destination, SearchOptionsModel options, CancellationToken token)
        {
            await destination.EnqueueEnumerationAsync(SearchDirectory(options), token);
        }

        /// <summary>
        /// Searches through subdirectories of current directory (recursive search) to get any file at any folder
        /// </summary>
        /// <param name="destination"> Destination collection to add items into </param>
        /// <param name="options"> Search options for a current search </param>
        /// <param name="token"> Token for canceling operation </param>
        public async Task SearchSubdirectoriesAsync(ConcurrentWrappersCollection destination, SearchOptionsModel options, CancellationToken token)
        {
            var subdirectories = EnumerateSubDirectories();

            var parallelOption = new ParallelOptions
            {
                MaxDegreeOfParallelism = 1,
                CancellationToken = token
            };
            try
            {
                await Parallel.ForEachAsync(subdirectories, parallelOption,
                    async (subdirectory, ct) => { await subdirectory.SearchAsync(destination, options, ct); });
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("SearchSubdirectoriesAsync Cancelled");
            }
        }

        private IEnumerable<DirectoryItemWrapper> SearchDirectory(SearchOptionsModel options)
        {
            var enumeration = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false
            };

            var found = EnumerateItems(enumeration, options.SearchPattern)
                // Any item that is used at provided date range
                .Where(item => options.AccessDateRange.Includes(item.LastAccess))
                // Any file types that satisfy filter
                .Where(item => options.ExtensionFilter(item.Name));

            if (options.SearchName is not null)
            {
                found = found.Where(item =>
                    item.Name.Contains(options.SearchName, StringComparison.OrdinalIgnoreCase));
            }

            return found;
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
