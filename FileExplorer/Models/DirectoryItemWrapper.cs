#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using FileAttributes = System.IO.FileAttributes;
using IOPath = System.IO.Path;

namespace FileExplorer.Models
{
    public abstract partial class DirectoryItemWrapper : ObservableObject, IEditableObject
    {
        private string backupName;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private bool isRenamed;

        protected FileSystemInfo info;

        [ObservableProperty]
        private BitmapImage thumbnail;

        public FileAttributes Attributes => info.Attributes;
        public string Path { get; private set; }

        protected DirectoryItemWrapper() { }
        protected DirectoryItemWrapper(FileSystemInfo info)
        {
            this.info = info;
            InitializeData();
        }

        protected void InitializeData()
        {
            Name = info.Name;
            Path = info.FullName;
        }

        /// <summary>
        /// Copy item from current directory to a destination directory
        /// </summary>
        /// <param name="destination"> Path to the destination directory </param>
        public abstract void Copy(string destination);
        /// <summary>
        /// Moves item from current directory to destination
        /// </summary>
        /// <param name="destination"> Path of a destination directory </param>
        public abstract void Move(string destination);

        /// <summary>
        /// Moves physical item to a recycle bin
        /// </summary>
        public abstract Task RecycleAsync();

        /// <summary>
        /// Permanently deletes physical item 
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Creates physical interpretation of wrapper in system
        /// </summary>
        /// <param name="destination"> Path to the folder that item is created in </param>
        public abstract void CreatePhysical(string destination);

        /// <summary>
        /// Converts item for a <see cref="IStorageItemProperties"/> thumbnail
        /// </summary>
        /// <returns> <see cref="IStorageItemProperties"/> representation of item </returns>
        public abstract Task<IStorageItemProperties> GetStorageItemPropertiesAsync();

        /// <summary>
        /// Gets current directory for an item. If item is directory itself, returns item.
        /// For a file gets parent directory
        /// </summary>
        /// <returns> this if item is <see cref="DirectoryWrapper"/> otherwise parent directory </returns>
        public abstract DirectoryWrapper GetCurrentDirectory();

        /// <summary>
        /// Gets parent directory for an item
        /// </summary>
        /// <returns> If item is root directory returns null, otherwise returns parent directory for the item </returns>
        public DirectoryWrapper? GetParentDirectory()
        {
            var directoryInfo = Directory.GetParent(Path);
            DirectoryWrapper? wrapper = null;

            if (directoryInfo != null)
                wrapper = new DirectoryWrapper(directoryInfo);

            return wrapper;
        }

        /// <summary>
        /// Generates unique name for a current item to avoid collisions.
        /// Method not only creates, but also can use item's current name if it is unique
        /// </summary>
        /// <param name="destination"> Path to a destination </param>
        /// <param name="template"> Template for name </param>
        /// <returns> Unique name for current item </returns>
        protected string GenerateUniqueName(string destination, string template)
        {
            var itemsCounter = 1;
            var nameWithoutExtension = IOPath.GetFileNameWithoutExtension(template);
            var extension = IOPath.GetExtension(template);
            var newName = template;

            var currentPath = IOPath.Combine(destination, newName);

            while (currentPath != Path && IOPath.Exists(currentPath))
            {
                itemsCounter++;
                newName = $"{nameWithoutExtension} ({itemsCounter}){extension}";
                currentPath = IOPath.Combine(destination, newName);
            }

            return newName;
        }

        public void BeginEdit()
        {
            if (!IsRenamed)
            {
                backupName = Name;
                IsRenamed = true;
            }
        }

        public void CancelEdit()
        {
            if (IsRenamed)
            {
                Name = backupName;
                IsRenamed = false;
            }
        }

        public void EndEdit()
        {
            if (IsRenamed)
            {
                backupName = Name;
                IsRenamed = false;
            }
        }
    }
}
