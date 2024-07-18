#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using IOPath = System.IO.Path;

namespace FileExplorer.Models.StorageWrappers
{
    public class DirectoryWrapper : DirectoryItemWrapper
    {
        private StorageFolder? asStorageFolder;
        public DirectoryWrapper() { }
        public DirectoryWrapper(DirectoryInfo info) : base(info) { }
        public DirectoryWrapper(string path) : base(new DirectoryInfo(path)) { }

        public IEnumerable<FileWrapper> EnumerateFiles(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return Directory.EnumerateFiles(Path, pattern, option).Select(path => new FileWrapper(path));
        }

        public IEnumerable<DirectoryWrapper> EnumerateFolders(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return Directory.EnumerateDirectories(Path, pattern, option).Select(path => new DirectoryWrapper(path));
        }

        public IEnumerable<DirectoryItemWrapper> EnumerateItems(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            foreach (var itemPath in Directory.EnumerateFileSystemEntries(Path, pattern, option))
            {
                if (File.Exists(itemPath))
                    yield return new FileWrapper(itemPath);
                else
                    yield return new DirectoryWrapper(itemPath);
            }
        }

        public async Task<int> CountFilesAsync(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return await Task.Run(() => Directory.EnumerateFiles(Path, pattern, option)
                .AsParallel().Count());
        }

        public async Task<int> CountFoldersAsync(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return await Task.Run(() => Directory.EnumerateDirectories(Path, pattern, option)
                .AsParallel().Count());
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
        public override Task<uint> CalculateSizeAsync()
        {
            throw new NotImplementedException();
        }

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
