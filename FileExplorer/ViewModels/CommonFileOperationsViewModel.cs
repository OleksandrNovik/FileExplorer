#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class CommonFileOperationsViewModel : ObservableRecipient
    {
        private readonly IDirectoryManager manager;

        [ObservableProperty]
        private bool canCreateItems;

        public DirectoryWrapper? CurrentDirectory
        {
            get => manager.CurrentDirectory;
            set
            {
                if (value is not null && !value.Equals(manager.CurrentDirectory))
                {
                    manager.CurrentDirectory = value;
                    CanCreateItems = true;
                }
                else
                {
                    CanCreateItems = false;
                }
            }
        }

        public CommonFileOperationsViewModel(IDirectoryManager manager)
        {
            this.manager = manager;
        }


        #region CreateLogic

        public async Task<DirectoryItemWrapper> CreateFile()
        {
            var wrapper = new FileWrapper();
            await CreateItemAsync(wrapper);
            return wrapper;
        }

        public async Task<DirectoryItemWrapper> CreateDirectory()
        {
            var wrapper = new DirectoryWrapper();
            await CreateItemAsync(wrapper);
            return wrapper;
        }

        /// <summary>
        /// Uses manager to create new item in current directory
        /// </summary>
        /// <param name="wrapper"> Wrapper that we are creating physical item for </param>
        private async Task CreateItemAsync(DirectoryItemWrapper wrapper)
        {
            manager.CreatePhysical(wrapper);

            await wrapper.UpdateThumbnailAsync();
        }

        #endregion

        /// <summary>
        /// Ends renaming item when it is renamed.
        /// This method is called before any operation with item to be sure it's not renamed while executing operation
        /// </summary>
        /// <param name="item"> Item that we are checking for renaming </param>
        [RelayCommand]
        private async Task EndRenamingIfNeeded(DirectoryItemWrapper item)
        {
            if (item.IsRenamed)
            {
                await EndRenamingItem(item);
            }
        }

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(DirectoryItemWrapper item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.CancelEdit();
                await App.MainWindow.ShowMessageDialogAsync("Item's name cannot be empty", "Empty name is illegal");
                return;
            }
            manager.Rename(item);

            // If item's extension changed we need to update icon
            if (item.HasExtensionChanged)
            {
                await item.UpdateThumbnailAsync();
            }

            item.EndEdit();
        }


        #region Delete logic

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        public async Task<DirectoryItemWrapper?> TryDeleteItem(DirectoryItemWrapper item, bool isPermanent)
        {
            await EndRenamingIfNeeded(item);

            try
            {
                if (isPermanent)
                {
                    manager.Delete(item);
                }
                else
                {
                    await manager.MoveToRecycleBinAsync(item);
                }

                return item;
            }
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync(e.Message, "Cannot delete item");
                return null;
            }
        }

        #endregion

        public async Task<DirectoryItemAdditionalInfo> ShowDetailsAsync(DirectoryItemWrapper item)
        {
            var details = item.GetBasicInfo();

            switch (item)
            {
                case DirectoryWrapper dir:
                    details.TitleInfo = $"Files: {dir.CountFiles()} Folders: {dir.CountFolders()}";
                    break;
                case FileWrapper file:
                    details.TitleInfo = await file.GetFileTypeAsync();
                    break;
                default:
                    throw new ArgumentException("Item not a directory or file.", nameof(item));
            }

            return details;
        }
    }
}
