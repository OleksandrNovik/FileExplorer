#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using FileExplorer.Services;
using FileExplorer.ViewModels.Messages;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DirectoryItemWrapper = FileExplorer.Models.StorageWrappers.DirectoryItemWrapper;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient, INavigationAware
    {
        private readonly IDirectoryManager manager;
        private readonly IMenuFlyoutFactory menuFactory;
        private readonly ContextMenuMetadataBuilder menuBuilder;

        [ObservableProperty]
        private DirectoryItemAdditionalInfo _selectedDirectoryItemAdditionalDetails;

        [ObservableProperty]
        private bool isDetailsShown;

        [ObservableProperty]
        private DirectoryWrapper currentDirectory;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemWrapper> directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemWrapper> selectedItems;
        public bool HasCopiedFiles { get; private set; }

        public DirectoryPageViewModel(IDirectoryManager manager, IMenuFlyoutFactory factory)
        {
            this.manager = manager;
            menuFactory = factory;
            menuBuilder = new ContextMenuMetadataBuilder(this);
            SelectedItems = [];
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, HandleNavigationMessage);
            Messenger.Register<DirectoryPageViewModel, FileOpenRequiredMessage>(this, HandlerFileOpen);
        }

        private async void HandlerFileOpen(DirectoryPageViewModel recipient, FileOpenRequiredMessage message)
        {
            await message.OpenFile.LaunchAsync();
        }


        /// <summary>
        /// Handles navigation messages from <see cref="DirectoriesNavigationViewModel"/>
        /// and decides how to execute new navigation command
        /// </summary>
        /// <param name="receiver"> Message receiver (this) </param>
        /// <param name="massage"> Navigation message that contains new path </param>
        private async void HandleNavigationMessage(DirectoryPageViewModel receiver, NavigationRequiredMessage massage)
        {
            await MoveToDirectoryAsync(massage.NavigatedDirectory);
        }

        /// <summary>
        /// Notifies each command that require at least one selected item if they can execute 
        /// </summary>
        private void NotifyCommandsCanExecute(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingItemCommand.NotifyCanExecuteChanged();
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
                                                            .Where(i => (i.Attributes & FileAttributes.System) == 0)
                                                            .ToArray();
            await AddDirectoryItemsAsync(directoryContent);
            SelectedItems.Clear();
        }

        private async Task AddDirectoryItemsAsync(ICollection<DirectoryItemWrapper> items)
        {
            DirectoryItems.AddRange(items);

            foreach (var item in items)
            {
                if ((item.Attributes & FileAttributes.Hidden) == 0)
                {
                    await item.UpdateThumbnailAsync();
                }
            }
        }

        /// <summary>
        /// Changes current directory and initializes its items
        /// </summary>
        /// <param name="directory"> Given directory that is opened </param>
        private async Task MoveToDirectoryAsync(DirectoryWrapper directory)
        {
            CurrentDirectory = directory;
            manager.CurrentDirectory = directory;
            await InitializeDirectoryAsync();
            Messenger.Send(directory);
        }

        #region Open logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task OpenSelectedItem()
        {
            await Open(SelectedItems[0]);
        }

        [RelayCommand]
        private async Task Open(DirectoryItemWrapper item)
        {
            await EndRenamingIfNeeded(item);

            switch (item)
            {
                case FileWrapper fileWrapper:
                    await fileWrapper.LaunchAsync();
                    break;
                case DirectoryWrapper directoryWrapper:
                    await MoveToDirectoryAsync(directoryWrapper);
                    var navigationModel = new DirectoryNavigationInfo(directoryWrapper);
                    Messenger.Send(navigationModel);
                    break;
                default:
                    throw new ArgumentException("Cannot open provided item. It is not a directory or file.", nameof(item));
            }
        }

        [RelayCommand]
        private void OpenInNewTab(DirectoryWrapper directory)
        {

        }

        #endregion

        #region Creating logic

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

            DirectoryItems.Insert(0, wrapper);
            BeginRenamingItem(wrapper);
        }

        #endregion

        #region Renaming logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void BeginRenamingSelectedItem()
        {
            BeginRenamingItem(SelectedItems[0]);
        }

        /// <summary>
        /// Begins renaming provided item
        /// </summary>
        /// <param name="item"> Item that is renamed </param>
        [RelayCommand]
        private void BeginRenamingItem(DirectoryItemWrapper item) => item.BeginEdit();

        private bool HasSelectedItems() => SelectedItems.Count > 0;

        /// <summary>
        /// Ends renaming only when needed to prevent double renaming of item at the same time
        /// (Example: It can happen when user presses enter and renames file and after that lost focus event is called renaming same file again)
        /// </summary>
        public AsyncRelayCommand<DirectoryItemWrapper> EndRenamingIfNeededCommand => new(EndRenamingIfNeeded!);

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

                DirectoryItems.Remove(item);
            }
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync(e.Message, $"File operation canceled");
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
            await ShowDetails(SelectedItems[0]);
        }

        [RelayCommand]
        private async Task ShowDetails(DirectoryItemWrapper item)
        {
            SelectedDirectoryItemAdditionalDetails = item.GetBasicInfo();
            IsDetailsShown = true;

            switch (item)
            {
                case DirectoryWrapper dir:
                    {
                        int files = await dir.CountFilesAsync(option: SearchOption.AllDirectories);
                        int folders = await dir.CountFoldersAsync(option: SearchOption.AllDirectories);
                        SelectedDirectoryItemAdditionalDetails.TitleInfo = $"Files: {files} Folders: {folders}";
                        break;
                    }
                case FileWrapper file:
                    SelectedDirectoryItemAdditionalDetails.TitleInfo = await file.GetFileTypeAsync();
                    break;
                default:
                    throw new ArgumentException("Item not a directory or file.", nameof(item));
            }
        }

        [RelayCommand]
        private void CloseDetailsMenu() => IsDetailsShown = false;

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is TabModel tab)
            {
                await MoveToDirectoryAsync(tab.TabDirectory);
                var navigationInfo = new DirectoryNavigationInfo(tab.TabDirectory);
                Messenger.Send(new NewTabOpened(navigationInfo, tab.TabHistory));
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
            throw new NotImplementedException();
        }

        public List<MenuFlyoutItemBase> OnContextMenuRequired()
        {
            List<MenuFlyoutItemViewModel> menu;

            if (HasSelectedItems())
            {
                menu = menuBuilder.BuildMenuForItem(SelectedItems[0]);
            }
            else
            {
                menu = menuBuilder.BuildDefaultMenu();
            }

            return menuFactory.Create(menu);
        }

    }
}
