using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Models.Contracts.Storage;

namespace FileExplorer.Models.Storage.Abstractions
{
    /// <summary>
    /// Abstract class to contain logic that needed to rename item 
    /// </summary>
    public abstract partial class InteractiveStorageItem : StorageItemProperties, IRenameableObject
    {
        /// <summary>
        /// Is item currently renamed
        /// </summary>
        [ObservableProperty]
        protected bool isRenamed;

        /// <summary>
        /// Backup name to restore name if it was not allowed
        /// </summary>
        protected string backupName;

        /// <inheritdoc />
        public bool CanRename { get; protected set; }

        [ObservableProperty]
        private string contentType;

        /// <summary>
        /// Renames item
        /// </summary>
        public abstract void Rename();

        /// <summary>
        /// Saves old name into backup and starts renaming item if it was not already renamed
        /// </summary>
        public void BeginEdit()
        {
            if (!IsRenamed && CanRename)
            {
                backupName = Name;
                IsRenamed = true;
            }
        }

        /// <summary>
        /// Cancels renaming by restoring old name (this method is useful when new name is invalid) 
        /// </summary>
        public void CancelEdit()
        {
            if (IsRenamed)
            {
                Name = backupName;
                IsRenamed = false;
            }
        }

        /// <summary>
        /// Ends renaming item by giving it new valid name 
        /// </summary>
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
