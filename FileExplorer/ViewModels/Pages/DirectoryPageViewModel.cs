#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.ViewModels.Abstractions;
using FileExplorer.ViewModels.General;
using Helpers;
using Helpers.Application;
using Microsoft.UI.Xaml.Controls;
using Models;
using Models.Contracts.Storage;
using Models.Contracts.Storage.Directory;
using Models.Messages;
using Models.ModelHelpers;
using Models.Storage.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.Pages
{
    public sealed partial class DirectoryPageViewModel : BaseSelectionViewModel
    {
        private IDirectory? currentDirectory;

        /// <summary>
        /// Service that gets all necessarily properties from local settings
        /// </summary>
        private readonly ILocalSettingsService localSettings;
        public ConcurrentWrappersCollection DirectoryItems { get; private set; }

        [ObservableProperty]
        private bool canCreateItems;

        public DirectoryPageViewModel(FileOperationsViewModel fileOperations, ILocalSettingsService settingsService)
            : base(fileOperations)
        {
            localSettings = settingsService;
        }

        protected override void OnSelectedItemsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnSelectedItemsChanged(sender, e);

            DeleteOperationCommand.NotifyCanExecuteChanged();
            RecycleOperationCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new Directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            var rejectedAttributes = localSettings.GetSkippedAttributes();

            DirectoryItems = new ConcurrentWrappersCollection(Storage.EnumerateItems(rejectedAttributes));

            //TODO change number to a constant
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
            while (SelectedItems.Count > 0)
            {
                var item = SelectedItems[0];
                var hasDeleted = await FileOperations.TryDeleteItem(item, isPermanent);

                if (hasDeleted)
                {
                    DirectoryItems.Remove(item);
                }

                SelectedItems.Remove(item);
            }
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

            currentDirectory = Storage as IDirectory;
            CanCreateItems = currentDirectory is not null;
        }

        [RelayCommand]
        private async Task Refresh()
        {
            //TODO: Fix this later
            await MoveToDirectoryAsync(Storage);
        }

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
                    .WithCreate(CreateItemCommand);

                //TODO:  Add view and sort options + paste;

                menu = list;
            }

            return menu;
        }
    }
}