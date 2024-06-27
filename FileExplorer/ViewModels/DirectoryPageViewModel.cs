#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Contracts;
using FileExplorer.Models;
using System.Collections.Generic;
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
        private List<DirectoryItemModel> selectedItems;

        public DirectoryPageViewModel(IDirectoryManager manager)
        {
            _manager = manager;
            var models = CurrentDirectory.GetFileSystemInfos()
                .Select(info => new DirectoryItemModel(info, info is FileInfo));

            directoryItems = new ObservableCollection<DirectoryItemModel>(models);
            selectedItems = new List<DirectoryItemModel>();
        }

        public RelayCommand RenameItemCommand => new RelayCommand(BeginRenamingItem);

        private void BeginRenamingItem()
        {
            if (SelectedItems.Count > 0)
            {
                SelectedItems[0].BeginEdit();
            }
        }

        private void EndRenamingItem(DirectoryItemModel item)
        {
            var newFullName = $@"{CurrentDirectory.FullName}\{item.Name}";
            // File or folder already exists, so we can't rename item or name is empty
            if (Path.Exists(newFullName) || item.Name == string.Empty)
            {
                item.CancelEdit();
            }
            else
            {
                // Trying to move item (if it does not exist it means that new item has to be created)
                if (!_manager.TryMove(item, newFullName))
                {
                    // Item is being created
                    _manager.Create(item, CurrentDirectory.FullName);
                }
                item.EndEdit();
            }
        }
    }
}
