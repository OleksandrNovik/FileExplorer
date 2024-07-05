#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
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
            var fileName = _manager.GetDefaultName($"New {(isFile ? "File" : "Folder")}", isFile);
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


        [RelayCommand]
        private async Task EndRenamingItem(DirectoryItemModel item)
        {
            var newFullName = $@"{CurrentDirectory.FullName}\{item.Name}";
            // File or folder already exists, so we can't rename item or name is empty
            if (Path.Exists(newFullName) || item.Name == string.Empty)
            {
                //TODO: File exists message
                item.CancelEdit();
                return;
            }

            await TryMoveItem(item, newFullName);
            //TODO: New Sorting of items is required
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
                SelectedItems.Remove(item);
                DirectoryItems.Remove(item);
            }
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync(e.Message, $"File operation canceled");
            }
        }

        #endregion

        #region Copy+Paste logic

        private void CopySelectedFiles()
        {
            _manager.CopyToClipboard(SelectedItems);
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
