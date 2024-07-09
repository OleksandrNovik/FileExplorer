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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient, INavigationAware
    {

        private readonly IDirectoryManager _manager;

        [ObservableProperty]
        private StorageFolder currentDirectory;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> selectedItems;

        public bool HasCopiedFiles => _manager.HasCopiedFiles;

        public DirectoryPageViewModel(IDirectoryManager manager)
        {
            _manager = manager;

            SelectedItems = new ObservableCollection<DirectoryItemModel>();
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, HandleDirectoryNavigationMessage);

        }

        private async void HandleDirectoryNavigationMessage(DirectoryPageViewModel receiver, NavigationRequiredMessage massage)
        {
            //TODO: Handle file or folder opening here
            var navigatedFolder = await StorageFolder.GetFolderFromPathAsync(massage.NavigationPath);
            await MoveToDirectoryAsync(navigatedFolder);
        }

        private void NotifyCommandsCanExecute(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingItemCommand.NotifyCanExecuteChanged();
            DeleteSelectedItemsCommand.NotifyCanExecuteChanged();
            CopySelectedItemsCommand.NotifyCanExecuteChanged();
            CutSelectedItemsCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            var models = (await CurrentDirectory.GetItemsAsync())
                .Select(folderItem => new DirectoryItemModel(folderItem));

            DirectoryItems = new ObservableCollection<DirectoryItemModel>(models);
            SelectedItems.Clear();
        }

        [RelayCommand]
        private async Task Open(DirectoryItemModel item)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await EndRenamingIfNeeded(item);

            if (item.FullInfo.IsOfType(StorageItemTypes.File))
            {
                //TODO: Open File 
            }
            else
            {
                if (item.FullInfo is StorageFolder dir)
                {
                    await MoveToDirectoryAsync(dir);

                    Messenger.Send(new DirectoryNavigationModel(dir.Path));
                }
            }
        }

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

        private async Task CreateItemAsync(bool isFile)
        {
            var wrapper = await _manager.CreateAsync(isFile);
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
        /// <returns> Completed task </returns>
        private async Task EndRenamingIfNeeded(DirectoryItemModel item)
        {
            if (item.IsRenamed)
            {
                await EndRenamingItem(item);
            }
        }

        #endregion

        #region DeleteAsync logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        public async Task DeleteSelectedItems()
        {
            var content = $"Do you really want to delete {(SelectedItems.Count > 1 ? "selected items" : $"\"{SelectedItems[0].FullPath}\""
                )} permanently?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            while (SelectedItems.Count > 0)
            {
                await TryDeleteItem(SelectedItems[0]);
            }
        }

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        private async Task TryDeleteItem(DirectoryItemModel item)
        {
            await EndRenamingIfNeeded(item);
            try
            {
                await _manager.DeleteAsync(item);
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

        private void NotifyHasCopied()
        {
            OnPropertyChanged(nameof(HasCopiedFiles));

            if (!HasCopiedFiles)
            {
                PasteItemsCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Creates copies of directory items (calling <see cref="ICloneable.Clone"/> method).
        /// And saves them into cache to paste later
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CopySelectedItems()
        {
            _manager.CopyToClipboard(SelectedItems.Select(item => item.Clone() as DirectoryItemModel));
            NotifyHasCopied();
        }

        /// <summary>
        /// Moves original versions of directory items into cache and deletes them from current directory.
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private void CutSelectedItems()
        {
            _manager.CopyToClipboard(SelectedItems);
            NotifyHasCopied();
            //await ClearSelectedItems();
        }

        /// <summary>
        /// Uses information about items from cache to create new files and folders in current location
        /// </summary>
        [RelayCommand]
        private void PasteItems()
        {
            DirectoryItems.AddRange(_manager.PasteFromClipboard());
            OnPropertyChanged(nameof(DirectoryItems));
        }

        #endregion

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is TabModel tab)
            {
                await MoveToDirectoryAsync(tab.TabDirectory);
                var directoryInfoModel = new DirectoryNavigationModel(tab.TabDirectory.Path);
                Messenger.Send(new NewTabOpened(directoryInfoModel, tab.TabHistory));
            }
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }
    }
}
