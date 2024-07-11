#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Enums;
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
using Windows.Storage;
using Windows.System;
using DirectoryItemModel = FileExplorer.Models.DirectoryItemModel;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient, INavigationAware
    {

        private readonly IDirectoryManager _manager;
        private readonly IPicturesService iconService;
        [ObservableProperty]
        private StorageFolder currentDirectory;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> selectedItems;

        private CopyOperation executedOperation = CopyOperation.None;

        public bool HasCopiedFiles => executedOperation != CopyOperation.None;

        public DirectoryPageViewModel(IDirectoryManager manager, IPicturesService iconService)
        {
            _manager = manager;
            this.iconService = iconService;
            SelectedItems = new ObservableCollection<DirectoryItemModel>();
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, HandleDirectoryNavigationMessage);
        }

        /// <summary>
        /// Handles navigation messages from <see cref="DirectoriesNavigationViewModel"/>
        /// and decides how to execute new navigation command
        /// </summary>
        /// <param name="receiver"> Message receiver (this) </param>
        /// <param name="massage"> Navigation message that contains new path </param>
        private async void HandleDirectoryNavigationMessage(DirectoryPageViewModel receiver, NavigationRequiredMessage massage)
        {
            //TODO: Handle file or folder opening here
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
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            var dirItems = await CurrentDirectory.GetItemsAsync();

            var models = dirItems.AsParallel().Select(folderItem => new DirectoryItemModel(folderItem)).ToArray();

            foreach (var model in models)
            {
                model.Thumbnail = await iconService.IconToImageAsync(model.FullInfo);
            }

            DirectoryItems = new ObservableCollection<DirectoryItemModel>(models);

            SelectedItems.Clear();
        }

        [RelayCommand]
        private async Task Open(DirectoryItemModel item)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await EndRenamingIfNeeded(item);

            if (item.FullInfo is StorageFile file)
            {
                await Launcher.LaunchFileAsync(file);
            }
            else
            {
                if (item.FullInfo is StorageFolder dir)
                {
                    await MoveToDirectoryAsync(dir);

                    var navigationModel = new DirectoryNavigationModel();
                    await navigationModel.InitializeDataAsync(dir);

                    Messenger.Send(navigationModel);
                }
            }
        }

        /// <summary>
        /// Changes current directory and initializes its items
        /// </summary>
        /// <param name="directory"> Given directory that is opened </param>
        private async Task MoveToDirectoryAsync(StorageFolder directory)
        {
            CurrentDirectory = directory;
            _manager.CurrentDirectory = CurrentDirectory;
            await InitializeDirectoryAsync();
            Messenger.Send(directory);
        }


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

        private void SaveToClipboard(IEnumerable<DirectoryItemModel> items)
        {
            _manager.CopyToClipboard(items.ToArray(), executedOperation);
            OnPropertyChanged(nameof(HasCopiedFiles));
        }

        /// <summary>
        /// Creates copies of directory items (calling <see cref="ICloneable.Clone"/> method).
        /// And saves them into cache to paste later
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CopySelectedItems()
        {
            executedOperation = CopyOperation.Copy;
            SaveToClipboard(SelectedItems.Select(i => i.Clone() as DirectoryItemModel));
        }

        /// <summary>
        /// Moves original versions of directory items into cache and deletes them from current directory.
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CutSelectedItems()
        {
            executedOperation = CopyOperation.Cut;
            SaveToClipboard(SelectedItems);

            DirectoryItems.RemoveRange(SelectedItems);
            SelectedItems.Clear();
        }

        /// <summary>
        /// Uses information about items from cache to create new files and folders in current location
        /// </summary>
        [RelayCommand]
        private async void PasteItems()
        {
            var pastedItems = await _manager.PasteFromClipboard(executedOperation);
            DirectoryItems.AddRange(pastedItems);
            OnPropertyChanged(nameof(DirectoryItems));
        }

        #endregion

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
