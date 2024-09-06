using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using Models.Contracts.Storage;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// Base class that has selected items logic for storage page
    /// </summary>
    public abstract partial class BaseSelectionViewModel : StorageViewModel
    {
        /// <summary>
        /// Selected items in currently viewed storage
        /// </summary>
        public ObservableCollection<IDirectoryItem> SelectedItems { get; }

        protected BaseSelectionViewModel(FileOperationsViewModel fileOperations, IMenuFlyoutFactory factory) : base(fileOperations, factory)
        {
            SelectedItems = [];
            SelectedItems.CollectionChanged += OnSelectedItemsChanged;
        }

        /// <summary>
        /// Occurs when selection is changed
        /// </summary>
        protected virtual void OnSelectedItemsChanged(object? sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingSelectedItemCommand.NotifyCanExecuteChanged();
        }

        protected bool HasSelectedItems() => SelectedItems.Count > 0;

        /// <summary>
        /// Begins renaming first selected item
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        protected void BeginRenamingSelectedItem() => FileOperations.BeginRenamingItem(SelectedItems[0]);
    }
}
