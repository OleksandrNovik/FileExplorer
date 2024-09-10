using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using Models;
using Models.Contracts.Storage.Directory;
using Models.ModelHelpers;
using Models.Storage.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// Base class that has selected items logic for storage page
    /// </summary>
    public abstract partial class BaseSelectionViewModel : StorageViewModel, IMenuFlyoutBuilder
    {
        /// <summary>
        /// Selected items in currently viewed storage
        /// </summary>
        public ObservableCollection<IDirectoryItem> SelectedItems { get; }

        protected BaseSelectionViewModel(FileOperationsViewModel fileOperations) : base(fileOperations)
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
            ShowDetailsCommand.NotifyCanExecuteChanged();
        }

        protected bool HasSelectedItems() => SelectedItems.Count > 0;

        /// <summary>
        /// Begins renaming first selected item
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        protected void BeginRenamingSelectedItem() => FileOperations.BeginRenamingItem(SelectedItems[0]);

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        protected void ShowDetails() => FileOperations.ShowDetails(SelectedItems[0]);

        public virtual IReadOnlyList<MenuFlyoutItemViewModel> BuildMenu(object parameter)
        {
            List<MenuFlyoutItemViewModel> menu = new();

            if (parameter is InteractiveStorageItem)
            {
                // Each item can have open command
                menu.WithOpen(FileOperations.OpenCommand, parameter);

                // If item is directory it can be opened it another tab or pinned
                if (parameter is IDirectory)
                {
                    menu.WithOpenInNewTab(FileOperations.OpenInNewTabCommand, parameter)
                        .WithPin(FileOperations.PinCommand, parameter);
                }

                // And each item can have show details (or properties) command
                menu.WithDetails(FileOperations.ShowDetailsCommand, parameter);
            }

            return menu;
        }
    }
}
