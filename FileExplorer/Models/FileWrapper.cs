#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace FileExplorer.Models
{
    public class FileWrapper : DirectoryItemWrapper
    {
        public FileWrapper() { }

        public FileWrapper(FileInfo info) : base(info) { }

        public FileWrapper(string path) : base(new FileInfo(path)) { }

        public async Task LaunchAsync()
        {
            var storageFile = await AsStorageFileAsync();
            await Launcher.LaunchFileAsync(storageFile);
        }

        public override void Copy(string destination)
        {
            Name = GenerateUniqueName(destination, Name + " - Copy");
            var newName = $@"{destination}\{Name}";
            File.Copy(Path, newName);
            info = new FileInfo(newName);
        }

        public override void Move(string destination)
        {
            Name = GenerateUniqueName(destination, Name);
            var newName = $@"{destination}\{Name}";
            File.Move(Path, newName);
            info = new FileInfo(newName);
        }

        public override void Recycle()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            File.Delete(Path);
        }

        public override void CreatePhysical(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, "New File");
            var fullName = destination + uniqueName;

            using (File.Create(fullName)) { }

            info = new FileInfo(fullName);
            Name = info.Name;
        }

        public override async Task<IStorageItemProperties> GetStorageItemPropertiesAsync()
        {
            return await AsStorageFileAsync();
        }

        private async Task<StorageFile> AsStorageFileAsync()
        {
            return await StorageFile.GetFileFromPathAsync(Path);
        }
    }
}
