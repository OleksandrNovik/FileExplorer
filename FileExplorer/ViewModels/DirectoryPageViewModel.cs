﻿#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using FileExplorer.ViewModels.Search;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DirectoryItemWrapper = Models.Storage.Windows.DirectoryItemWrapper;
using FileAttributes = System.IO.FileAttributes;

namespace FileExplorer.ViewModels
{
    public sealed partial class DirectoryPageViewModel : ContextMenuCreatorViewModel, INavigationAware
    {
        public SearchOperationViewModel SearchOperations { get; } = new();
        public FileOperationsViewModel FileOperations { get; } = new();
        public IStorage<DirectoryItemWrapper> CurrentDirectory { get; private set; }

        [ObservableProperty]
        private ConcurrentWrappersCollection directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemWrapper> selectedItems;

        public DirectoryPageViewModel(IMenuFlyoutFactory factory) : base(factory)
        {
            SelectedItems = [];
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;

            //Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, OnNavigationRequired);
            Messenger.Register<DirectoryPageViewModel, FileOpenRequiredMessage>(this, OnFileOpenRequired);

            Messenger.Register<DirectoryPageViewModel, NavigateToSearchResult<DirectoryItemWrapper>>(this, OnSearchResultNavigation);
        }

        private async void OnSearchResultNavigation(DirectoryPageViewModel _, NavigateToSearchResult<DirectoryItemWrapper> message)
        {
            DirectoryItems.Clear();
            await DirectoryItems.EnqueueEnumerationAsync(message.SearchResult.SearchResultItems, CancellationToken.None);

            SearchOperations.InitializeSearchData(CurrentDirectory, DirectoryItems, message.SearchResult);
        }

        private async void OnFileOpenRequired(DirectoryPageViewModel _, FileOpenRequiredMessage message)
        {
            await message.OpenFile.LaunchAsync();
        }


        /// <summary>
        /// Handles navigation messages from <see cref="DirectoriesNavigationViewModel"/>
        /// and decides how to execute new navigation command
        /// </summary>
        /// <param name="_"> Message receiver (this) </param>
        /// <param name="massage"> Navigation message that contains new path </param>
        private async void OnNavigationRequired(DirectoryPageViewModel _, NavigationRequiredMessage massage)
        {
            await MoveToDirectoryAsync(massage.NavigatedStorage);
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
            DirectoryItems = [];
            var directoryContent = CurrentDirectory.EnumerateItems()
                .Where(i => (i.Attributes & FileAttributes.System) == 0);

            await DirectoryItems.EnqueueEnumerationAsync(directoryContent, CancellationToken.None);

            SelectedItems.Clear();
        }

        /// <summary>
        /// Changes current directory and initializes its items
        /// </summary>
        /// <param name="directory"> Given directory that is opened </param>
        private async Task MoveToDirectoryAsync(IStorage<DirectoryItemWrapper> directory)
        {
            Messenger.Send(new StopSearchMessage());
            Messenger.Send(new TabStorageChangedMessage(directory));

            CurrentDirectory = directory;
            await InitializeDirectoryAsync();

            SearchOperations.InitializeSearchData(CurrentDirectory, DirectoryItems);
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
        /// Creates new item in <see cref="CurrentDirectory"/> 
        /// </summary>
        /// <param name="isFile"> True - if file should be created, false - if directory is being created </param>
        [RelayCommand]
        private async Task CreateItem(bool isFile)
        {
            DirectoryItemWrapper wrapper = isFile ? new FileWrapper() : new DirectoryWrapper();

            wrapper.CreatePhysical(CurrentDirectory.Path);

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
        private async Task TryDeleteItem(DirectoryItemWrapper item, bool isPermanent = false)
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
        private async Task ShowDetailsOfSelectedItem()
        {
            await FileOperations.ShowDetails(SelectedItems[0]);
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is IStorage<DirectoryItemWrapper> storage)
            {
                await MoveToDirectoryAsync(storage);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            //TODO: Fix this later
            await MoveToDirectoryAsync(CurrentDirectory);
        }

        public void OnNavigatedFrom()
        {
            Messenger.UnregisterAll(this);
            SearchOperations.UnregisterAll();
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
                parameter = CurrentDirectory;
                menu.WithRefresh(RefreshCommand)
                    .WithCreate(CreateItemCommand)
                    .WithPaste(PasteItemsCommand);
            }

            return menuFactory.Create(menu
                .WithDetails(FileOperations.ShowDetailsCommand, parameter));
        }
    }
}