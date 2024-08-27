#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.Abstractions;
using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml.Controls;
using Models;
using Models.Contracts.Storage;
using Models.Messages;
using Models.ModelHelpers;
using Models.Storage.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DirectoryItemWrapper = Models.Storage.Windows.DirectoryItemWrapper;

namespace FileExplorer.ViewModels
{
    public sealed partial class DirectoryPageViewModel : StorageViewModel
    {
        public FileOperationsViewModel FileOperations { get; }
        public ObservableCollection<IDirectoryItem> SelectedItems => FileOperations.OperatedItems;

        [ObservableProperty]
        private ConcurrentWrappersCollection directoryItems;

        public DirectoryPageViewModel(FileOperationsViewModel fileOperations, IMenuFlyoutFactory factory) : base(factory)
        {
            FileOperations = fileOperations;
            FileOperations.OperatedItems.CollectionChanged += NotifyCommandsCanExecute;

            Messenger.Register<DirectoryPageViewModel, FileOpenRequiredMessage>(this, OnFileOpenRequired);
        }

        private async void OnFileOpenRequired(DirectoryPageViewModel _, FileOpenRequiredMessage message)
        {
            await message.OpenFile.LaunchAsync();
        }

        /// <summary>
        /// Notifies each command that require at least one selected item if they can execute 
        /// </summary>
        private void NotifyCommandsCanExecute(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingSelectedItemCommand.NotifyCanExecuteChanged();
            DeleteSelectedItemsCommand.NotifyCanExecuteChanged();
            CopySelectedItemsCommand.NotifyCanExecuteChanged();
            CutSelectedItemsCommand.NotifyCanExecuteChanged();
            RecycleSelectedItemsCommand.NotifyCanExecuteChanged();
            ShowDetailsOfSelectedItemCommand.NotifyCanExecuteChanged();
            OpenSelectedItemCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            DirectoryItems = new ConcurrentWrappersCollection(Storage.EnumerateItems());

            await DirectoryItems.UpdateIconsAsync(90, CancellationToken.None);

            SelectedItems.Clear();
        }

        /// <summary>
        /// Changes current storage and initializes its items
        /// </summary>
        /// <param name="storage"> Given storage that is opened </param>
        private async Task MoveToDirectoryAsync(IStorage<IDirectoryItem> storage)
        {
            Storage = storage;
            await InitializeDirectoryAsync();
        }

        #region Open logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task OpenSelectedItem()
        {
            await FileOperations.Open(SelectedItems[0]);
        }

        #endregion

        #region Creating logic

        /// <summary>
        /// Creates file in <see cref="CurrentDirectory"/>
        /// </summary>
        [RelayCommand]
        private async Task CreateFile()
        {
            await CreateItem(true);
        }

        /// <summary>
        /// Creates folder in <see cref="CurrentDirectory"/>
        /// </summary>
        [RelayCommand]
        private async Task CreateDirectory()
        {
            await CreateItem(false);
        }

        /// <summary>
        /// Creates new item in <see cref="Storage"/> 
        /// </summary>
        /// <param name="isFile"> True - if file should be created, false - if directory is being created </param>
        [RelayCommand]
        private async Task CreateItem(bool isFile)
        {
            DirectoryItemWrapper wrapper = isFile ? new FileWrapper() : new DirectoryWrapper();

            wrapper.CreatePhysical(Storage.Path);

            await wrapper.UpdateThumbnailAsync(90);

            DirectoryItems.Insert(0, wrapper);
            FileOperations.BeginRenamingItem(wrapper);
        }

        #endregion

        #region Renaming logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void BeginRenamingSelectedItem()
        {
            FileOperations.BeginRenamingItem(SelectedItems[0]);
        }

        private bool HasSelectedItems() => SelectedItems.Count > 0;

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(DirectoryItemWrapper item)
        {
            await FileOperations.EndRenamingItem(item);

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
            while (SelectedItems.Count > 0)
            {
                await TryDeleteItem(SelectedItems[0]);
            }
        }

        /// <summary>
        /// Permanently deletes selected items
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        public async Task DeleteSelectedItems()
        {
            var content = $"Do you really want to delete {(SelectedItems.Count > 1 ? "selected items" : $"\"{SelectedItems[0].Path}\""
                )} permanently?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            while (SelectedItems.Count > 0)
            {
                await TryDeleteItem(SelectedItems[0], true);
            }
        }

        [RelayCommand]
        private async Task RecycleItem(DirectoryItemWrapper item) => await TryDeleteItem(item);

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        private async Task TryDeleteItem(IDirectoryItem item, bool isPermanent = false)
        {
            await FileOperations.EndRenamingIfNeeded(item);

            try
            {
                if (isPermanent)
                {
                    item.Delete();
                }
                else
                {
                    await item.RecycleAsync();
                }

                DirectoryItems.Remove(item);
            }
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync(e.Message, "File operation canceled");
            }
            finally
            {
                SelectedItems.Remove(item);
            }
        }

        #endregion

        #region Copy+Paste logic

        //private void MoveToClipboard(IEnumerable<DirectoryItemModel> items, DataPackageOperation operation)
        //{
        //    manager.CopyToClipboard(items, operation);
        //    HasCopiedFiles = true;
        //    OnPropertyChanged(nameof(HasCopiedFiles));
        //}

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CopySelectedItems()
        {
            //MoveToClipboard(SelectedItems, DataPackageOperation.Copy);
        }

        [RelayCommand]
        private void CopyItem(DirectoryItemWrapper item)
        {

        }

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CutSelectedItems()
        {
            //MoveToClipboard(SelectedItems, DataPackageOperation.Move);
        }

        [RelayCommand]
        private void CutItem(DirectoryItemWrapper item)
        {

        }

        [RelayCommand]
        private async Task PasteItems()
        {
            //var pastedItems = await manager.PasteFromClipboard();
            //await AddDirectoryItemsAsync(pastedItems);
        }

        #endregion

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void ShowDetailsOfSelectedItem()
        {
            //await FileOperations.ShowDetails(FileOperations.OperatedItems[0]);
        }

        public override async void OnNavigatedTo(object parameter)
        {
            base.OnNavigatedTo(parameter);

            if (Storage is not null)
            {
                await MoveToDirectoryAsync(Storage);
            }
            else if (parameter is SearchStorageTransferObject transferredSearchData)
            {
                NavigateStorage(transferredSearchData.Storage);
                DirectoryItems = transferredSearchData.Source;
            }
            else
            {
                throw new ArgumentException("Invalid parameter", nameof(parameter));
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            //TODO: Fix this later
            await MoveToDirectoryAsync(Storage);
        }

        public override IList<MenuFlyoutItemBase> BuildContextMenu(object? parameter = null)
        {
            List<MenuFlyoutItemViewModel> menu = new();

            if (parameter is not null)
            {
                menu.WithOpen(FileOperations.OpenCommand, parameter);

                if (parameter is DirectoryWrapper)
                {
                    menu.WithOpenInNewTab(FileOperations.OpenInNewTabCommand, parameter)
                        .WithPin(FileOperations.PinCommand, parameter);
                }

                menu.WithCopy(CopySelectedItemsCommand)
                    .WithFileOperations(
                    [
                        CutSelectedItemsCommand,
                        BeginRenamingSelectedItemCommand

                    ]).WithDelete(RecycleSelectedItemsCommand);
            }
            else
            {
                parameter = Storage;
                menu.WithRefresh(RefreshCommand)
                    .WithCreate(CreateItemCommand)
                    .WithPaste(PasteItemsCommand);
            }

            return menuFactory.Create(menu
                .WithDetails(FileOperations.ShowDetailsCommand, parameter));
        }
    }
}