#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using FileExplorer.ViewModels.Messages;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;
using DirectoryItemModel = FileExplorer.Models.DirectoryItemModel;
using FileAttributes = System.IO.FileAttributes;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient, INavigationAware
    {
        private readonly IDirectoryManager _manager;
        private readonly IPicturesService iconService;

        [ObservableProperty]
        private FileInfoModel selectedFileDetails;

        [ObservableProperty]
        private bool isDetailsShown;

        [ObservableProperty]
        private StorageFolder currentDirectory;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> selectedItems;
        public bool HasCopiedFiles { get; private set; }

        public DirectoryPageViewModel(IDirectoryManager manager, IPicturesService iconService)
        {
            _manager = manager;
            this.iconService = iconService;
            SelectedItems = new ObservableCollection<DirectoryItemModel>();
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, HandleDirectoryNavigationMessage);
            Messenger.Register<DirectoryPageViewModel, FileOpenRequiredMessage>(this, OpenFileRequired);
        }

        private async void OpenFileRequired(DirectoryPageViewModel recipient, FileOpenRequiredMessage message)
        {
            await OpenStorageItem(message.OpenFile);
        }

        /// <summary>
        /// Handles navigation messages from <see cref="DirectoriesNavigationViewModel"/>
        /// and decides how to execute new navigation command
        /// </summary>
        /// <param name="receiver"> Message receiver (this) </param>
        /// <param name="massage"> Navigation message that contains new path </param>
        private async void HandleDirectoryNavigationMessage(DirectoryPageViewModel receiver, NavigationRequiredMessage massage)
        {
            await MoveToDirectoryAsync(massage.NavigatedFolder);
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
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            var dir = new DirectoryInfo(CurrentDirectory.Path);
            var dirItems = dir.EnumerateFileSystemInfos().Where(f => (f.Attributes & FileAttributes.Hidden) == 0);

            DirectoryItems = new ObservableCollection<DirectoryItemModel>(dirItems.Select(i => new DirectoryItemModel(i.FullName, i.Name, i is FileInfo)));

            //var directoryContent = await CurrentFolder.GetItemsAsync();
            //await AddDirectoryItemsAsync(directoryContent);

            foreach (var model in DirectoryItems)
            {
                model.Thumbnail = await iconService.IconToImageAsync(model.FullPath, model.IsFile);
            }

            SelectedItems.Clear();
        }

        private async Task AddDirectoryItemsAsync(IEnumerable<IStorageItem> items)
        {
            var models = items.AsParallel()
                                                .Select(folderItem => new DirectoryItemModel(folderItem))
                                                .ToArray();

            DirectoryItems.AddRange(models);

            foreach (var model in models)
            {
                model.Thumbnail = await iconService.IconToImageAsync(model.FullInfo as IStorageItemProperties);
            }
        }

        /// <summary>
        /// Changes current directory and initializes its items
        /// </summary>
        /// <param name="directory"> Given directory that is opened </param>
        private async Task MoveToDirectoryAsync(StorageFolder directory)
        {
            CurrentDirectory = directory;
            _manager.CurrentFolder = CurrentDirectory;
            await InitializeDirectoryAsync();
            Messenger.Send(directory);
        }

        #region Open logic

        private async Task OpenStorageItem(IStorageItem item)
        {
            if (item is StorageFile file)
            {
                await Launcher.LaunchFileAsync(file);
            }
            else if (item is StorageFolder dir)
            {
                await MoveToDirectoryAsync(dir);

                var navigationModel = new DirectoryNavigationModel();
                await navigationModel.InitializeDataAsync(dir);

                Messenger.Send(navigationModel);
            }
            else
                throw new ArgumentException("Storage item is neither a file nor a folder.", nameof(item));
        }

        [RelayCommand]
        private async Task Open(DirectoryItemModel item)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await EndRenamingIfNeeded(item);

            await OpenStorageItem(item.FullInfo);
        }

        #endregion

        #region Creating logic

        [RelayCommand]
        private async Task CreateFile() => await CreateItemAsync(true);

        [RelayCommand]
        private async Task CreateDirectory() => await CreateItemAsync(false);

        /// <summary>
        /// Uses manager to create new item in current directory
        /// </summary>
        /// <param name="isFile"> Is item a file or a folder </param>
        private async Task CreateItemAsync(bool isFile)
        {
            var wrapper = await _manager.CreateAsync(isFile);
            wrapper.Thumbnail = await iconService.IconToImageAsync(wrapper.FullInfo);
            DirectoryItems.Insert(0, wrapper);
            RenameNewItem(wrapper);
        }

        #endregion

        #region Renaming logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void BeginRenamingItem()
        {
            RenameNewItem(SelectedItems[0]);
        }

        /// <summary>
        /// Begins renaming provided item
        /// </summary>
        /// <param name="item"> Item that is renamed </param>
        private void RenameNewItem(DirectoryItemModel item) => item.BeginEdit();

        private bool HasSelectedItems() => SelectedItems.Count > 0;

        /// <summary>
        /// Ends renaming only when needed to prevent double renaming of item at the same time
        /// (Example: It can happen when user presses enter and renames file and after that lost focus event is called renaming same file again)
        /// </summary>
        public AsyncRelayCommand<DirectoryItemModel> EndRenamingIfNeededCommand =>
            new(async item =>
            {
                if (item.IsRenamed)
                {
                    await EndRenamingItem(item);
                }
            });

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(DirectoryItemModel item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.CancelEdit();
                await App.MainWindow.ShowMessageDialogAsync("Item's name cannot be empty", "Empty name is illegal");
                return;
            }

            await _manager.RenameAsync(item);
            item.EndEdit();

            //TODO: New Sorting of items is required
        }

        /// <summary>
        /// Ends renaming item when it is renamed.
        /// This method is called before any operation with item to be sure it's not renamed while executing operation
        /// </summary>
        /// <param name="item"> Item that we are checking for renaming </param>
        private async Task EndRenamingIfNeeded(DirectoryItemModel item)
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
            var content = $"Do you really want to delete {(SelectedItems.Count > 1 ? "selected items" : $"\"{SelectedItems[0].FullPath}\""
                )} permanently?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            while (SelectedItems.Count > 0)
            {
                await TryDeleteItem(SelectedItems[0], true);
            }
        }

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        private async Task TryDeleteItem(DirectoryItemModel item, bool isPermanent = false)
        {
            await EndRenamingIfNeeded(item);
            try
            {
                if (isPermanent)
                {
                    await _manager.DeleteAsync(item);
                }
                else
                {
                    await _manager.MoveToRecycleBinAsync(item);
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

        private void MoveToClipboard(IEnumerable<DirectoryItemModel> items, DataPackageOperation operation)
        {
            _manager.CopyToClipboard(items, operation);
            HasCopiedFiles = true;
            OnPropertyChanged(nameof(HasCopiedFiles));
        }

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CopySelectedItems()
        {
            MoveToClipboard(SelectedItems, DataPackageOperation.Copy);
        }

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CutSelectedItems()
        {
            MoveToClipboard(SelectedItems, DataPackageOperation.Move);
        }

        [RelayCommand]
        private async Task PasteItems()
        {
            var pastedItems = await _manager.PasteFromClipboard();
            await AddDirectoryItemsAsync(pastedItems);
        }

        #endregion

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task ShowDetailsOfSelectedItem()
        {
            await ShowDetails(SelectedItems[0]);
        }

        private async Task ShowDetails(DirectoryItemModel item)
        {
            SelectedFileDetails = await FileInfoModel.InitializeAsync(item);
            IsDetailsShown = true;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is TabModel tab)
            {
                await MoveToDirectoryAsync(tab.TabDirectory);
                var directoryInfoModel = new DirectoryNavigationModel();
                await directoryInfoModel.InitializeDataAsync(tab.TabDirectory);

                Messenger.Send(new NewTabOpened(directoryInfoModel, tab.TabHistory));
            }
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }
    }
}
