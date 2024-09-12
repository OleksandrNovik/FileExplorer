#nullable enable
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.Models.Storage.Additional;
using FileExplorer.Models.Storage.Additional.Properties;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using FileAttributes = System.IO.FileAttributes;
using IOPath = System.IO.Path;

namespace FileExplorer.Models.Storage.Windows
{
    public abstract class DirectoryItemWrapper : InteractiveStorageItem, IDirectoryItem
    {
        protected FileSystemInfo info;
        public FileAttributes Attributes => info.Attributes;
        public DateTime LastAccess => info.LastAccessTime;
        public ByteSize? Size { get; protected set; }

        /// <summary>
        /// Empty constructor to create empty wrapper
        /// </summary>
        protected DirectoryItemWrapper() { }

        /// <summary>
        /// Creates wrapper that has physical item
        /// </summary>
        /// <param name="info"> Information about physical item </param>
        protected DirectoryItemWrapper(FileSystemInfo info)
        {
            this.info = info;

            InitializeData();
        }

        /// <summary>
        /// When physical item is changed sets new Path and Name for this wrapper
        /// </summary>
        protected void InitializeData()
        {
            Name = info.Name;
            Path = info.FullName;

            // if file or folder declared as read only we cannot rename them
            CanRename = this.HasAttributes(FileAttributes.ReadOnly) is not true;
        }

        /// <inheritdoc />
        public abstract void Copy(string destination);

        protected abstract void CopyPhysical(string destination, string newName);

        /// <inheritdoc />
        public override void Rename()
        {
            RenamePhysical();
            EndEdit();
        }

        /// <summary>
        /// Renames physical item that this wrapper is representing
        /// </summary>
        protected void RenamePhysical()
        {
            var elementsDirectory = GetParentDirectory();
            ArgumentNullException.ThrowIfNull(elementsDirectory);
            Move(elementsDirectory.Path);
        }

        /// <inheritdoc />
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

        public StorageItemProperties GetBasicProperties()
        {
            return new DirectoryItemBasicProperties(Name, Path)
            {
                CreationTime = info.CreationTime,
                ModifiedDate = info.LastWriteTime,
            };
        }
    }
}
