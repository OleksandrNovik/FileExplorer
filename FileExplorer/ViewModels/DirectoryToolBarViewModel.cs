#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using FileExplorer.ViewModels.Messages;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class DirectoryToolBarViewModel : ObservableRecipient
    {
        private readonly IDirectoryManager manager;

        private IList<DirectoryItemWrapper> selectedItems = [];

        [ObservableProperty]
        private bool canCreateItems;

        public DirectoryToolBarViewModel(IDirectoryManager manager)
        {
            this.manager = manager;

            // Listens to a new DirectoryPageViewModel opened, to get reference for selectedItems and also current directory
            Messenger.Register<DirectoryToolBarViewModel, InitializeToolBarMessage>(this, (_, message) =>
            {
                if (message.CurrentDirectory is not null)
                {
                    manager.CurrentDirectory = message.CurrentDirectory;
                }

                if (message.SelectedItems is not null)
                {
                    selectedItems = message.SelectedItems;
                }
            });

            // Listens from DirectoriesNavigationViewModel to change current directory every navigation action
            Messenger.Register<DirectoryToolBarViewModel, NavigationRequiredMessage>(this, (_, message) =>
            {
                SetNewDirectory(message.NavigatedDirectory);
            });

            // Listens to DirectoryPageViewModel to change current directory when new directory is opened
            Messenger.Register<DirectoryToolBarViewModel, DirectoryNavigationInfo>(this, (_, message) =>
            {
                SetNewDirectory(new DirectoryWrapper(message.FullPath));
            });
        }

        private void SetNewDirectory(DirectoryWrapper directory)
        {
            if (!directory.Equals(manager.CurrentDirectory))
            {
                manager.CurrentDirectory = directory;
            }
        }

        #region CreateLogic

        [RelayCommand]
        private async Task CreateFile()
        {
            var wrapper = new FileWrapper();
            await CreateItemAsync(wrapper);
        }

        [RelayCommand]
        private async Task CreateDirectory()
        {
            var wrapper = new DirectoryWrapper();
            await CreateItemAsync(wrapper);
        }

        /// <summary>
        /// Uses manager to create new item in current directory
        /// </summary>
        /// <param name="wrapper"> Wrapper that we are creating physical item for </param>
        private async Task CreateItemAsync(DirectoryItemWrapper wrapper)
        {
            manager.CreatePhysical(wrapper);

            await wrapper.UpdateThumbnailAsync();

            Messenger.Send(new DirectoryChangedMessage([wrapper]));

            //TODO: Send message to insert new item  DirectoryItems.Insert(0, wrapper);
            BeginRenamingItem(wrapper);
        }

        #endregion

        #region RenameLogic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void BeginRenamingSelectedItem()
        {
            BeginRenamingItem(selectedItems[0]);
        }

        /// <summary>
        /// Begins renaming provided item
        /// </summary>
        /// <param name="item"> Item that is renamed </param>
        private void BeginRenamingItem(DirectoryItemWrapper item) => item.BeginEdit();

        private bool HasSelectedItems() => selectedItems.Count > 0;

        /// <summary>
        /// Ends renaming only when needed to prevent double renaming of item at the same time
        /// (Example: It can happen when user presses enter and renames file and after that lost focus event is called renaming same file again)
        /// </summary>
        public AsyncRelayCommand<DirectoryItemWrapper> EndRenamingIfNeededCommand => new(EndRenamingIfNeeded!);

        /// <summary>
        /// Ends renaming item when it is renamed.
        /// This method is called before any operation with item to be sure it's not renamed while executing operation
        /// </summary>
        /// <param name="item"> Item that we are checking for renaming </param>
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

            //TODO: New Sorting of items is required
        }

        #endregion

        #region Delete logic

        /// <summary>
        /// Moves selected items to a recycle bin
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task RecycleSelectedItems()
        {
            await RemoveRange(false);
        }

        /// <summary>
        /// Permanently deletes selected items
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        public async Task DeleteSelectedItems()
        {
            var content = $"Do you really want to delete {(selectedItems.Count > 1 ? "selected items" : $"\"{selectedItems[0].Path}\""
                )} permanently?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            await RemoveRange(true);
        }

        private async Task RemoveRange(bool isPermanent)
        {
            var removed = new List<DirectoryItemWrapper>();

            while (selectedItems.Count > 0)
            {
                var removedItem = await TryDeleteItem(selectedItems[0], isPermanent);

                if (removedItem is not null)
                {
                    removed.Add(removedItem);
                }
            }

            Messenger.Send(new DirectoryChangedMessage(Removed: removed));
        }

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        private async Task<DirectoryItemWrapper?> TryDeleteItem(DirectoryItemWrapper item, bool isPermanent)
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
            finally
            {
                selectedItems.Remove(item);
            }
        }

        #endregion

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void ShowDetails()
        {
            //TODO: Send show details message
        }
    }
}
