#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Clipboard;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Contracts.Storage;
using FileExplorer.Helpers;
using FileExplorer.Helpers.Application;
using FileExplorer.Models;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Messages;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.ViewModels.Abstractions;
using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer.ViewModels.Pages
{
    public sealed partial class DirectoryPageViewModel : BaseSelectionViewModel
    {
        private IDirectory? currentDirectory;

        private readonly IStorageSortingService sortingService;

        /// <summary>
        /// Clipboard service that provides access to the clipboard
        /// </summary>
        private readonly IClipboardService clipboard;

        /// <summary>
        /// Service that gets all necessarily properties from local settings
        /// </summary>
        private readonly ILocalSettingsService localSettings;
        public ConcurrentWrappersCollection DirectoryItems { get; private set; }

        [ObservableProperty]
        private bool canCreateItems;

        public DirectoryPageViewModel(FileOperationsViewModel fileOperations, IStorageSortingService directorySorter, ILocalSettingsService settingsService,
            IClipboardService clipboardService, INameValidator validator)
            : base(fileOperations, validator)
        {
            clipboard = clipboardService;
            localSettings = settingsService;
            sortingService = directorySorter;

            clipboard.FileDropListChanged += NotifyCanPaste;
            clipboard.CutOperationStarted += OnCutOperation;
        }

        private void OnCutOperation(object? sender, CutOperationData e)
        {
            if (currentDirectory is not null && e.CutDirectory.Path == currentDirectory.Path)
            {
                DirectoryItems.RemovePaths(e.Paths);
            }
        }

        /// <summary>
        /// Checks if user can paste inside current directory
        /// </summary>
        private bool CanPasteInside() => CanCreateItems && clipboard.HasFiles;

        /// <inheritdoc />
        protected override void OnSelectedItemsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnSelectedItemsChanged(sender, e);

            DeleteOperationCommand.NotifyCanExecuteChanged();
            RecycleOperationCommand.NotifyCanExecuteChanged();
            CutSelectedItemsCommand.NotifyCanExecuteChanged();
        }

        private void NotifyCanPaste(object? sender, EventArgs args)
        {
            PasteInsideCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new Directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            var sorted = sortingService.SortByKey(Storage as IDirectory, item => item.Name);

            DirectoryItems = new ConcurrentWrappersCollection(sorted);

            await DirectoryItems.UpdateIconsAsync(Constants.ThumbnailSizes.Big, CancellationToken.None);

            SelectedItems.Clear();
        }

        /// <summary>
        /// Changes current storage and initializes its items
        /// </summary>
        /// <param name="storage"> Given storage that is opened </param>
        private async Task MoveToDirectoryAsync(IStorage storage)
        {
            Storage = storage;
            await InitializeDirectoryAsync();
        }

        [RelayCommand]
        private async Task CreateFile()
        {
            await CreateItemAsync(false);
        }

        [RelayCommand]
        private async Task CreateDirectory()
        {
            await CreateItemAsync(true);
        }

        /// <summary>
        /// Saves selected items to the clipboard with required operation "cut"
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CutSelectedItems()
        {
            clipboard.SetFiles(SelectedItems, DragDropEffects.Move);
        }

        /// <summary>
        /// Pastes items from clipboard inside current directory
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanPasteInside))]
        private async Task PasteInside()
        {
            Debug.Assert(currentDirectory is not null);
            var data = clipboard.GetFiles();

            if (currentDirectory is not null && data is not null)
            {
                var items = FileOperations.PasteAndGetItems(data, currentDirectory);

                await DirectoryItems.EnqueueEnumerationAsync(items, CancellationToken.None);
            }
            else
            {
                throw new ArgumentException("Invalid operation for a current directory");
            }
        }

        /// <summary>
        /// Adding new item to physical Directory and sending message to update Directory on UI layer
        /// </summary>
        /// <param name="isDirectory"> Is added item a Directory </param>
        [RelayCommand]
        public async Task CreateItemAsync(bool isDirectory)
        {
            Debug.Assert(currentDirectory is not null);
            var created = currentDirectory.Create(isDirectory);

            await created.UpdateThumbnailAsync(Constants.ThumbnailSizes.Big);

            DirectoryItems.Insert(0, created);
            FileOperations.BeginRenamingItem(created);
        }

        /// <summary>
        /// Moves selected items to a recycle bin
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task RecycleOperationAsync()
        {
            await DeleteSelectedAsync(false);
        }

        /// <summary>
        /// Permanently deletes selected items
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task DeleteOperationAsync()
        {
            var confirmUser = localSettings.ReadBool(LocalSettings.Keys.ShowConfirmationMessage);

            if (confirmUser is true)
            {
                var content =
                    $"Do you really want to delete {(SelectedItems.Count > 1 ? "selected items" : $"\"{SelectedItems[0].Path}\""
                        )} permanently?";
                var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

                if (result == ContentDialogResult.Secondary) return;
            }

            await DeleteSelectedAsync(true);
        }

        /// <summary>
        /// Deletes all selected items and removes them from Directory items collection if operation was successful
        /// </summary>
        /// <param name="isPermanent"> Is delete permanent or recycle </param>
        private async Task DeleteSelectedAsync(bool isPermanent)
        {
            var deleteCount = SelectedItems.Count;

            while (deleteCount > 0)
            {
                var item = SelectedItems[0];
                var hasDeleted = await FileOperations.TryDeleteItem(item, isPermanent);

                if (hasDeleted)
                {
                    DirectoryItems.Remove(item);
                }

                SelectedItems.Remove(item);
                deleteCount--;
            }
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
            clipboard.FileDropListChanged -= NotifyCanPaste;
            clipboard.CutOperationStarted -= OnCutOperation;
        }

        /// <inheritdoc />
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

            currentDirectory = Storage as IDirectory;

            // User cannot create files when viewing search result
            CanCreateItems = currentDirectory is not null;

            NotifyCanPaste(this, EventArgs.Empty);
        }

        [RelayCommand]
        private async Task Refresh()
        {
            //TODO: Fix this later
            await MoveToDirectoryAsync(Storage);
        }

        /// <inheritdoc />
        public override IReadOnlyList<MenuFlyoutItemViewModel> BuildMenu(object parameter)
        {
            IReadOnlyList<MenuFlyoutItemViewModel> menu;

            if (parameter is InteractiveStorageItem)
            {
                menu = base.BuildMenu(parameter);
            }
            else
            {
                var list = new List<MenuFlyoutItemViewModel>();

                list.WithRefresh(RefreshCommand)
                    .WithCreate(CreateItemCommand)
                    .WithPaste(FileOperations.PasteCommand, currentDirectory);

                //TODO:  Add view and sort options;

                menu = list;
            }

            return menu;
        }
    }
}