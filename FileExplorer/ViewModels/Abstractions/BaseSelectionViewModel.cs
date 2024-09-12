#nullable enable
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Clipboard;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using Models;
using Models.Contracts.Storage.Directory;
using Models.ModelHelpers;
using Models.Storage.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// Base class that has selected items logic for storage page
    /// </summary>
    public abstract partial class BaseSelectionViewModel : StorageViewModel, IMenuFlyoutBuilder
    {
        /// <summary>
        /// Clipboard service that provides access to the clipboard
        /// </summary>
        protected readonly IClipboardService clipboard;

        /// <summary>
        /// Selected items in currently viewed storage
        /// </summary>
        public ObservableCollection<IDirectoryItem> SelectedItems { get; }

        protected BaseSelectionViewModel(FileOperationsViewModel fileOperations, IClipboardService clipboardService) : base(fileOperations)
        {
            SelectedItems = [];
            SelectedItems.CollectionChanged += OnSelectedItemsChanged;

            clipboard = clipboardService;
            clipboard.FileDropListChanged += NotifyCanPaste;
        }

        #region Can paste

        /// <summary>
        /// Checks if there is any files inside of clipboard 
        /// </summary>
        protected bool CanPaste() => clipboard.HasFiles;

        /// <summary>
        /// Notifies if user can paste files from the clipboard
        /// </summary>
        protected virtual void NotifyCanPaste(object? sender, EventArgs args)
        {
            PasteCommand.NotifyCanExecuteChanged();
        }

        #endregion

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

        /// <summary>
        /// Saves selected items to the clipboard with required operation "copy"
        /// </summary>
        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        protected void CopySelectedItems()
        {
            clipboard.SetFiles(SelectedItems, DragDropEffects.Copy);
        }

        /// <summary>
        /// Pastes items from clipboard into the directory
        /// </summary>
        /// <param name="directory"> Destination directory </param>
        [RelayCommand(CanExecute = nameof(CanPaste))]
        protected void Paste(IDirectory directory)
        {

        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
            clipboard.FileDropListChanged -= NotifyCanPaste;
        }

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
                        .WithPaste(PasteCommand, parameter);
                }

                // And each item can have show details (or properties) command
                menu.WithDetails(FileOperations.ShowDetailsCommand, parameter);
            }

            return menu;
        }
    }
}
