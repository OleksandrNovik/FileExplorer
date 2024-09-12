#nullable enable
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Models;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.ViewModels.General;
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


        #region Has selected items

        /// <summary>
        /// Checks if there are selected items
        /// </summary>
        protected bool HasSelectedItems() => SelectedItems.Count > 0;

        /// <summary>
        /// Occurs when selection is changed
        /// </summary>
        protected virtual void OnSelectedItemsChanged(object? sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingSelectedItemCommand.NotifyCanExecuteChanged();
            ShowDetailsCommand.NotifyCanExecuteChanged();
            CopySelectedItemsCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Saves selected items to the clipboard with required operation "copy"
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        protected void CopySelectedItems()
        {
            FileOperations.CopyItems(SelectedItems);
        }

        #endregion

        /// <summary>
        /// Begins renaming first selected item
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        protected void BeginRenamingSelectedItem() => FileOperations.BeginRenamingItem(SelectedItems[0]);

        /// <summary>
        /// Shows details for the first selected item
        /// </summary>
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
                        .WithPin(FileOperations.PinCommand, parameter)
                        .WithPaste(FileOperations.PasteCommand, parameter);
                }

                // And each item can have show details (or properties) command
                menu.WithDetails(FileOperations.ShowDetailsCommand, parameter);
            }

            return menu;
        }
    }
}
