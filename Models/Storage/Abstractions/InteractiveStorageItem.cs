using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage;

namespace Models.Storage.Abstractions
{
    /// <summary>
    /// Abstract class to contain logic that needed to rename item 
    /// </summary>
    public abstract partial class InteractiveStorageItem : BaseThumbnailProvider, IRenameableObject
    {
        /// <summary>
        /// Item's name that can be changed using UI
        /// </summary>
        [ObservableProperty]
        protected string name;

        /// <summary>
        /// Is item currently renamed
        /// </summary>
        [ObservableProperty]
        protected bool isRenamed;

        /// <summary>
        /// Backup name to restore name if it was not allowed
        /// </summary>
        protected string backupName;

        /// <summary>
        /// Read-only path of item
        /// </summary>
        public string Path { get; protected set; }

        /// <inheritdoc />
        public bool CanRename { get; protected set; }
        public abstract void Rename();

        public abstract IBasicStorageItemProperties GetBasicProperties();

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
