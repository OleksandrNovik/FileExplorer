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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class DirectoryPageViewModel : StorageViewModel
    {
        public ObservableCollection<IDirectoryItem> SelectedItems => FileOperations.SelectedItems;

        [ObservableProperty]
        private ConcurrentWrappersCollection directoryItems;

        public DirectoryPageViewModel(FileOperationsViewModel fileOperations, IMenuFlyoutFactory factory) : base(fileOperations, factory)
        {
            Messenger.Register<DirectoryPageViewModel, LaunchRequiredMessage>(this, OnFileOpenRequired);
        }

        private async void OnFileOpenRequired(DirectoryPageViewModel _, LaunchRequiredMessage message)
        {
            await message.ItemToLaunch.LaunchAsync();
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            DirectoryItems = new ConcurrentWrappersCollection(Storage.EnumerateItems());

            await DirectoryItems.UpdateIconsAsync(25, CancellationToken.None);

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

        /// <summary>
        /// Creates new item in current directory
        /// </summary>
        /// <param name="isDirectory"> True - if directory should be created, False - if file is being created </param>
        [RelayCommand]
        private async Task CreateItem(bool isDirectory)
        {
            var creationCommand = isDirectory
                ? FileOperations.CreateDirectoryCommand
                : FileOperations.CreateFileCommand;

            await creationCommand.ExecuteAsync(null);
        }

        #region Renaming logic

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(IRenameableObject item)
        {
            await FileOperations.EndRenamingItemCommand.ExecuteAsync(item);

            //TODO: New Sorting of items is required
        }

        [RelayCommand]
        private async Task EndRenamingIfNeeded(IRenameableObject item)
        {
            if (item.IsRenamed)
            {
                await EndRenamingItem(item);
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

        [RelayCommand]
        private void CopySelectedItems()
        {
            //MoveToClipboard(SelectedItems, DataPackageOperation.Copy);
        }


        [RelayCommand]
        private void CutSelectedItems()
        {
            //MoveToClipboard(SelectedItems, DataPackageOperation.Move);
        }

        [RelayCommand]
        private async Task PasteItems()
        {
            //var pastedItems = await manager.PasteFromClipboard();
            //await AddDirectoryItemsAsync(pastedItems);
        }

        #endregion

        [RelayCommand]
        private void ShowDetailsOfSelectedItem()
        {
            //await FileOperations.ShowDetails(FileOperations.SelectedItems[0]);
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

                if (parameter is IDirectory)
                {
                    menu.WithOpenInNewTab(FileOperations.OpenInNewTabCommand, parameter)
                        .WithPin(FileOperations.PinCommand, parameter);
                }

                menu.WithCopy(CopySelectedItemsCommand)
                    .WithFileOperations(
                    [
                        CutSelectedItemsCommand,
                        FileOperations.BeginRenamingSelectedItemCommand

                    ]).WithDelete(FileOperations.RecycleSelectedItemsCommand);
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