using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Models
{
    public class DirectoryWrapper : DirectoryItemWrapper
    {
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

        public override void Copy(string destination)
        {
            throw new NotImplementedException();
        }

        public override void Move(string destination)
        {
            Name = GenerateUniqueName(destination, Name);
            var newName = $@"{destination}\{Name}";
            Directory.Move(Path, newName);
            info = new DirectoryInfo(newName);
        }

        public override void Recycle()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            Directory.Delete(Path, true);
        }

        public override void CreatePhysical(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, "New Folder");
            info = Directory.CreateDirectory(destination + uniqueName);
            Name = info.Name;
        }

        public override async Task<IStorageItemProperties> GetStorageItemPropertiesAsync()
        {
            return await AsStorageFolderAsync();
        }

        private async Task<StorageFolder> AsStorageFolderAsync()
        {
            return await StorageFolder.GetFolderFromPathAsync(Path);
        }
    }
}
