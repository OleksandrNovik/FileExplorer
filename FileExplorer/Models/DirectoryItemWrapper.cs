#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using FileAttributes = System.IO.FileAttributes;

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
        public string Path => info.FullName;

        protected DirectoryItemWrapper() { }
        protected DirectoryItemWrapper(FileSystemInfo info)
        {
            Name = info.Name;
            this.info = info;
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
        public abstract void Recycle();

        /// <summary>
        /// Permanently deletes physical item 
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Creates physical interpretation of wrapper in system
        /// </summary>
        /// <param name="destination"> Path to the folder that item is created in </param>
        public abstract void CreatePhysical(string destination);

        public abstract Task<IStorageItemProperties> GetStorageItemPropertiesAsync();

        public DirectoryWrapper? GetParentDirectory()
        {
            var directoryInfo = Directory.GetParent(Path);
            DirectoryWrapper? wrapper = null;

            if (directoryInfo != null)
                wrapper = new DirectoryWrapper(directoryInfo);

            return wrapper;
        }

        protected string GenerateUniqueName(string destination, string template)
        {
            return "";
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
