using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.IO;

namespace FileExplorer.Models
{
    public abstract partial class BasicExplorerItem : ObservableObject, IEditableObject
    {
        private string backupName;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private bool isRenamed;

        private FileSystemInfo info;

        [ObservableProperty]
        private BitmapImage thumbnail = new();

        public string Path => info.FullName;

        protected BasicExplorerItem(FileSystemInfo info)
        {
            Name = info.Name;
            this.info = info;
        }

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
