#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using IOPath = System.IO.Path;

namespace Models.Storage.Windows
{
    public sealed class FileWrapper : DirectoryItemWrapper
    {
        private StorageFile? asStorageFile;
        public FileWrapper() { }

        public FileWrapper(FileInfo info) : base(info) { }

        public FileWrapper(string path) : base(new FileInfo(path)) { }

        public async Task LaunchAsync()
        {
            var storageFile = await AsStorageFileAsync();
            await Launcher.LaunchFileAsync(storageFile);
        }

        /// <inheritdoc />
        public override void Copy(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, Name + " - Copy");
            var newName = $@"{destination}\{uniqueName}";
            File.Copy(Path, newName);
            info = new FileInfo(newName);
            InitializeData();
        }

        /// <inheritdoc />
        public override void Move(string destination)
        {
            Name = GenerateUniqueName(destination, Name);
            var newPath = IOPath.Combine(destination, Name);

            // File is being moved to the same directory
            if (newPath == Path) return;

            File.Move(Path, newPath);
            info = new FileInfo(newPath);
            InitializeData();
            asStorageFile = null;
        }

        /// <inheritdoc />
        public override async Task RecycleAsync()
        {
            var storageFile = await AsStorageFileAsync();
            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        /// <inheritdoc />
        public override void Delete()
        {
            File.Delete(Path);
        }

        /// <inheritdoc />
        public override void CreatePhysical(string destination)
        {
            var uniqueName = GenerateUniqueName(destination, "New File");
            var fullName = IOPath.Combine(destination, uniqueName);

            using (File.Create(fullName)) { }

            info = new FileInfo(fullName);
            InitializeData();
        }

        /// <inheritdoc />
        public override async Task<IStorageItemProperties> GetStorageItemPropertiesAsync()
        {
            return await AsStorageFileAsync();
        }

        /// <inheritdoc />
        public override DirectoryWrapper GetCurrentDirectory()
        {
            var parent = GetParentDirectory();

            ArgumentNullException.ThrowIfNull(parent);

            return parent;
        }
        public async Task<string> GetFileTypeAsync()
        {
            var storageFile = await AsStorageFileAsync();

            return $"{storageFile.DisplayType} ({info.Extension})";
        }

        private async Task<StorageFile> AsStorageFileAsync()
        {
            if (asStorageFile is null)
            {
                try
                {
                    asStorageFile = await StorageFile.GetFileFromPathAsync(Path);
                }
                catch
                {
                    asStorageFile = await StorageFile.GetFileFromPathAsync(@"D:\Навчальння\chistyi-cod.pdf");
                }
            }

            return asStorageFile;
        }
    }
}
