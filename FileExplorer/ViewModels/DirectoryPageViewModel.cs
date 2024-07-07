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

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient, INavigationAware
    {

        private readonly IDirectoryManager _manager;

        [ObservableProperty]
        private DirectoryInfo currentDirectory;

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

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, (_, massage) =>
            {
                //TODO: Handle file or folder opening here
                MoveToDirectory(new DirectoryInfo(massage.NavigationPath));
            });

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
        private void InitializeDirectory()
        {
            var models = CurrentDirectory.GetFileSystemInfos()
                .Select(info => new DirectoryItemModel(info, info is FileInfo));

            DirectoryItems = new ObservableCollection<DirectoryItemModel>(models);
            SelectedItems.Clear();
        }

        [RelayCommand]
        private async Task Open(DirectoryItemModel item)
        {
            await EndRenamingIfNeeded(item);

            if (item.IsFile)
            {
                //TODO: Open File 
            }
            else
            {
                if (item.FullInfo is DirectoryInfo dir)
                {
                    MoveToDirectory(dir);

                    Messenger.Send(new DirectoryNavigationModel(dir));
                }
            }
        }

        private void MoveToDirectory(DirectoryInfo directory)
        {
            CurrentDirectory = directory;
            _manager.CurrentDirectory = CurrentDirectory;
            InitializeDirectory();
            Messenger.Send(directory);
        }


        #region Creating logic

        [RelayCommand]
        private void CreateFile() => CreateItem(true);

        [RelayCommand]
        private void CreateDirectory() => CreateItem(false);

        private void CreateItem(bool isFile)
        {
            var fileName = _manager.GetDefaultName($"New {(isFile ? "File" : "Folder")}");
            var emptyWrapper = new DirectoryItemModel(fileName, isFile);
            DirectoryItems.Insert(0, emptyWrapper);
            RenameNewItem(emptyWrapper);
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
            var newFullName = $@"{CurrentDirectory.FullName}\{item.Name}";

            if (await IsItemsNameLegal(item, newFullName))
            {
                await TryMoveItem(item, newFullName);
            }
            //TODO: New Sorting of items is required
        }

        /// <summary>
        /// Checks if new item's name is legal (does not exist and is not empty).
        /// If item has illegal name show message for a user.
        /// And if physically item does not exist at the same time creates new physical folder or file to represent item
        /// </summary>
        /// <param name="item"> Item to check name for </param>
        /// <param name="newFullName"> New full name (full path) of physical item</param>
        /// <returns> true if name is legal and no canceling was executed and false if name was illegal </returns>
        private async Task<bool> IsItemsNameLegal(DirectoryItemModel item, string newFullName)
        {
            var isItemNameLegal = true;
            var itemsNameIsEmpty = string.IsNullOrWhiteSpace(item.Name);
            // File or folder already exists, so we can't rename item or name is empty
            if (Path.Exists(newFullName) || itemsNameIsEmpty)
            {
                // If item has been named with illegal name we revert changes
                item.CancelEdit();
                isItemNameLegal = false;

                // Show user feedback message
                if (itemsNameIsEmpty)
                {
                    await App.MainWindow.ShowMessageDialogAsync("Item's name cannot be empty", "Empty name is illegal");
                }
                else
                {
                    await App.MainWindow.ShowMessageDialogAsync(
                        $"Item \"{newFullName}\" already exists. Old name will be applied...", "Item exists");
                }

                // If item was being created we should create it anyway (with old legal name)
                if (item.FullInfo is null)
                {
                    _manager.Create(item);
                }
            }

            return isItemNameLegal;
        }

        /// <summary>
        /// Tries to "Move" (rename) file and handles exceptions that might occur in this process
        /// </summary>
        /// <param name="item"> Items that is renamed </param>
        /// <param name="newFullName"> New name for this item </param>
        /// <returns> Completed task </returns>
        private async Task TryMoveItem(DirectoryItemModel item, string newFullName)
        {
            try
            {
                _manager.Move(item, newFullName);
                item.EndEdit();
            }
            // Case when item is empty wrapper and we should create file or folder first
            catch (ArgumentNullException)
            {
                _manager.Create(item);
                item.EndEdit();
            }
            // Object is in use in other process
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync($"{e.Message} Item can be used in other process.", "Cannot rename item");
                item.CancelEdit();
            }
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

        #region Delete logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        public async Task DeleteSelectedItems()
        {
            var content = $"Do you really want to delete {(SelectedItems.Count > 1 ? "selected items" : $"\"{SelectedItems[0].FullPath}\""
                )}?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            await ClearSelectedItems();
        }

        private async Task ClearSelectedItems()
        {
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
                _manager.Delete(item);
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
        private async Task CutSelectedItems()
        {
            _manager.CopyToClipboard(SelectedItems);
            NotifyHasCopied();
            await ClearSelectedItems();
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

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is TabModel tab)
            {
                MoveToDirectory(tab.TabDirectory);
                var directoryInfoModel = new DirectoryNavigationModel(tab.TabDirectory);
                Messenger.Send(new NewTabOpened(directoryInfoModel, tab.TabHistory));
            }
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }
    }
}
