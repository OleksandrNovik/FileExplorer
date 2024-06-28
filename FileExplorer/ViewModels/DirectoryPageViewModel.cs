#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Contracts;
using FileExplorer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient
    {
        private readonly IDirectoryManager _manager;

        [ObservableProperty]
        private DirectoryInfo currentDirectory = new DirectoryInfo(@"D:\Навчальння");

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> directoryItems;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> selectedItems;

        private DirectoryItemModel? previousEditedItem;

        public DirectoryPageViewModel(IDirectoryManager manager)
        {
            _manager = manager;
            var models = CurrentDirectory.GetFileSystemInfos()
                .Select(info => new DirectoryItemModel(info, info is FileInfo));

            directoryItems = new ObservableCollection<DirectoryItemModel>(models);
            selectedItems = new ObservableCollection<DirectoryItemModel>();
            SelectedItems.CollectionChanged += (_, _) => BeginRenamingItemCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void CreateFile() => CreateItem(true);

        [RelayCommand]
        private void CreateDirectory() => CreateItem(false);

        private void CreateItem(bool isFile)
        {
            var emptyWrapper = new DirectoryItemModel(isFile);
            DirectoryItems.Insert(0, emptyWrapper);
            RenameNewItem(emptyWrapper);
        }

        #region Renaming logic

        [RelayCommand(CanExecute = nameof(CanRename))]
        private void BeginRenamingItem()
        {
            RenameNewItem(SelectedItems[0]);
        }

        /// <summary>
        /// Ends renaming previous item calling <see cref="EndRenamingItem"/>
        /// and begins editing selected or created item
        /// </summary>
        /// <param name="item"> New item that is renamed </param>
        private void RenameNewItem(DirectoryItemModel item)
        {
            // If item is being edited we should finish renaming it
            if (previousEditedItem != null)
            {
                EndRenamingItem(previousEditedItem);
            }

            item.BeginEdit();
            previousEditedItem = item;
        }

        private bool CanRename() => SelectedItems.Count > 0;


        [RelayCommand]
        private void EndRenamingItem(DirectoryItemModel item)
        {
            var newFullName = $@"{CurrentDirectory.FullName}\{item.Name}";
            // File or folder already exists, so we can't rename item or name is empty
            if (Path.Exists(newFullName) || item.Name == string.Empty)
            {
                //TODO: File exists message
                item.CancelEdit();
            }
            // Trying to move item (if it does not exist it means that new item has to be created)
            if (!_manager.TryMove(item, newFullName))
            {
                // Item is being created
                _manager.Create(item, CurrentDirectory.FullName);
            }

            item.EndEdit();
            //TODO: New Sorting of items is required
        }
        #endregion
    }
}
