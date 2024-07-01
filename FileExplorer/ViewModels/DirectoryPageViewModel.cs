#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentDialog = Microsoft.UI.Xaml.Controls.ContentDialog;

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
            var models = CurrentDirectory.GetFileSystemInfos()
                .Select(info => new DirectoryItemModel(info, info is FileInfo));

            directoryItems = new ObservableCollection<DirectoryItemModel>(models);
            selectedItems = new ObservableCollection<DirectoryItemModel>();
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;
        }

        private void NotifyCommandsCanExecute(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingItemCommand.NotifyCanExecuteChanged();
            DeleteSelectedItemsCommand.NotifyCanExecuteChanged();
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
                //TODO: New Sorting of items is required
            }
            // Case when item is empty wrapper and we should create file or folder first
            catch (ArgumentNullException)
            {
                _manager.Create(item);
                item.EndEdit();
                //TODO: New Sorting of items is required
            }
            // Object is in use in other process
            catch (IOException e)
            {
                item.CancelEdit();
                var dialog = new ContentDialog
                {
                    Title = $"Cannot rename {(item.IsFile ? "File" : "Folder")}",
                    Content = e.Message,
                    CloseButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }
        }
        #endregion

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        public async Task DeleteSelectedItems()
        {
            while (SelectedItems.Count > 0)
            {
                if (!TryDeleteItem(SelectedItems[0]))
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Cannot Delete file",
                        Content = $"Cannot access the \"{SelectedItems[0].Name}\" because it is being used by another process.",
                        CloseButtonText = "Ok"
                    };
                    await dialog.ShowAsync();
                }
            }
        }

        private bool TryDeleteItem(DirectoryItemModel item)
        {
            if (item.IsRenamed)
            {
                EndRenamingItem(item);
            }

            var isItemDeleted = _manager.TryDelete(item);

            if (isItemDeleted)
            {
                DirectoryItems.Remove(item);
            }

            return isItemDeleted;
        }
    }
}
