#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.Services;
using FileExplorer.ViewModels.Messages;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient
    {
        private readonly IDirectoryManager _manager;

        [ObservableProperty]
        private DirectoryInfo currentDirectory = new DirectoryInfo(@"D:\");

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> selectedItems;

        public DirectoryPageViewModel()
        {
            _manager = new DirectoryManager(currentDirectory);

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, (_, massage) =>
            {
                MoveToDirectory(massage.NavigationPath);
            });

            InitializeDirectory();
        }

        private void NotifyCommandsCanExecute(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingItemCommand.NotifyCanExecuteChanged();
            DeleteSelectedItemsCommand.NotifyCanExecuteChanged();
        }

        private void InitializeDirectory()
        {
            var models = CurrentDirectory.GetFileSystemInfos()
                .Select(info => new DirectoryItemModel(info, info is FileInfo));

            DirectoryItems = new ObservableCollection<DirectoryItemModel>(models);
            SelectedItems = new ObservableCollection<DirectoryItemModel>();
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;
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
                MoveToDirectory(item.FullPath);

                var directoryInfo = item.FullInfo as DirectoryInfo;

                Messenger.Send(new DirectoryNavigationModel(directoryInfo));
            }
        }

        private void MoveToDirectory(string dirName)
        {
            CurrentDirectory = new DirectoryInfo(dirName);
            _manager.MoveToNewDirectory(CurrentDirectory);
            InitializeDirectory();
        }


        #region Creating logic

        [RelayCommand]
        private void CreateFile() => CreateItem(true);

        [RelayCommand]
        private void CreateDirectory() => CreateItem(false);

        private void CreateItem(bool isFile)
        {
            var emptyWrapper = new DirectoryItemModel(_manager.GetDefaultName(isFile), isFile);
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
            //TODO: New Sorting of items is required
        }

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
                await App.MainWindow.ShowMessageDialogAsync(e.Message, $"Cannot delete item");
            }
        }

        #endregion
    }
}
