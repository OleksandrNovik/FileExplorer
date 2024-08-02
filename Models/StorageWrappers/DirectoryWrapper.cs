#nullable enable
using Models.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using IOPath = System.IO.Path;
using SearchOption = System.IO.SearchOption;

namespace Models.StorageWrappers
{
    public class DirectoryWrapper : DirectoryItemWrapper, ISearchable<DirectoryItemWrapper>
    {
        private StorageFolder? asStorageFolder;
        public DirectoryWrapper() { }
        public DirectoryWrapper(DirectoryInfo info) : base(info) { }
        public DirectoryWrapper(string path) : base(new DirectoryInfo(path)) { }

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

        public override void Copy(string destination)
        {
            throw new NotImplementedException();
        }

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

        public ParallelQuery<DirectoryItemWrapper> SearchParallel(SearchOptionsModel options)
        {
            var enumeration = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = options.IsNestedSearch
            };

            var found = EnumerateItems(enumeration, options.SearchPattern)
                .AsParallel()
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

        public override async Task RecycleAsync()
        {
            var storageFolder = await AsStorageFolderAsync();
            await storageFolder.DeleteAsync(StorageDeleteOption.Default);
        }

        public override void Delete()
        {
            Directory.Delete(Path, true);
        }

        public override void CreatePhysical(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, "New Folder");
            var newPath = IOPath.Combine(destination, uniqueName);
            info = Directory.CreateDirectory(newPath);
            InitializeData();
        }

        public override async Task<IStorageItemProperties> GetStorageItemPropertiesAsync()
        {
            return await AsStorageFolderAsync();
        }

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
