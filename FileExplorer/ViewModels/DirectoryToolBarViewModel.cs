#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Models.StorageWrappers;
using FileExplorer.ViewModels.Messages;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class DirectoryToolBarViewModel : ObservableRecipient
    {
        private IList<DirectoryItemWrapper> selectedItems = [];
        public CommonFileOperationsViewModel CommonFileOperationsViewModel { get; }
        public DirectoryToolBarViewModel(CommonFileOperationsViewModel operationsVM)
        {
            CommonFileOperationsViewModel = operationsVM;
            // Listens to a new DirectoryPageViewModel opened, to get reference for selectedItems and also current directory
            Messenger.Register<DirectoryToolBarViewModel, InitializeToolBarMessage>(this, (_, message) =>
            {
                if (message.SelectedItems is not null)
                {
                    selectedItems = message.SelectedItems;
                    message.SelectedItems.CollectionChanged += OnSelectedItemsChanged;
                }
            });
        }

        private void OnSelectedItemsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingSelectedItemCommand.NotifyCanExecuteChanged();
            DeleteSelectedItemsCommand.NotifyCanExecuteChanged();
            RecycleSelectedItemsCommand.NotifyCanExecuteChanged();
            ShowDetailsCommand.NotifyCanExecuteChanged();
        }

        #region CreateLogic

        [RelayCommand]
        private async Task CreateFile()
        {
            var wrapper = await CommonFileOperationsViewModel.CreateFile();
            SendCreatingMessage(wrapper);
        }

        [RelayCommand]
        private async Task CreateDirectory()
        {
            var wrapper = await CommonFileOperationsViewModel.CreateDirectory();
            SendCreatingMessage(wrapper);
        }

        /// <summary>
        /// Uses manager to create new item in current directory
        /// </summary>
        /// <param name="wrapper"> Wrapper that we are creating physical item for </param>
        private void SendCreatingMessage(DirectoryItemWrapper wrapper)
        {
            Messenger.Send(new DirectoryChangedMessage([wrapper]));
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
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(DirectoryItemWrapper item)
        {
            await CommonFileOperationsViewModel.EndRenamingItemCommand.ExecuteAsync(item);
            //TODO: Sorting message needed
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
                var removedItem = await CommonFileOperationsViewModel.TryDeleteItem(selectedItems[0], isPermanent);

                if (removedItem is not null)
                {
                    removed.Add(removedItem);
                }
                selectedItems.RemoveAt(0);
            }

            Messenger.Send(new DirectoryChangedMessage(Removed: removed));
        }

        #endregion

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task ShowDetails()
        {
            var info = await CommonFileOperationsViewModel.ShowDetailsAsync(selectedItems[0]);
            Messenger.Send(info);
        }
    }
}
