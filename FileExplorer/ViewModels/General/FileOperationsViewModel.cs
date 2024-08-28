﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Models.Contracts.Storage;
using Models.Messages;
using Models.Storage.Abstractions;
using Models.Storage.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class FileOperationsViewModel : ObservableRecipient
    {
        private IDirectory directory;

        [ObservableProperty]
        private bool canAlterDirectory;

        public ObservableCollection<IDirectoryItem> OperatedItems { get; } = new();

        public FileOperationsViewModel()
        {
            OperatedItems.CollectionChanged += NotifyCanExecute;

            Messenger.Register<FileOperationsViewModel, TabStorageChangedMessage>(this, (_, message) =>
            {
                if (message.Storage is IDirectory dir)
                {
                    directory = dir;
                    CanAlterDirectory = true;
                }
                else
                {
                    CanAlterDirectory = false;
                }
            });
        }

        private void NotifyCanExecute(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BeginRenamingSelectedItemCommand.NotifyCanExecuteChanged();
            NotifyCanChangeDirectory();
        }

        private void NotifyCanChangeDirectory()
        {
            RecycleSelectedItemsCommand.NotifyCanExecuteChanged();
            DeleteSelectedItemsCommand.NotifyCanExecuteChanged();
        }

        partial void OnCanAlterDirectoryChanged(bool value)
        {
            NotifyCanChangeDirectory();
        }

        private bool HasOperatedItems() => OperatedItems.Count > 0;

        private bool CanChangeDirectory() => CanAlterDirectory && HasOperatedItems();

        #region Create

        /// <summary>
        /// Creates file in <see cref="directory"/>
        /// </summary>
        [RelayCommand]
        private async Task CreateFile()
        {
            Debug.Assert(CanAlterDirectory);
            await directory.CreateAsync(false);
        }

        /// <summary>
        /// Creates folder in <see cref="directory"/>
        /// </summary>
        [RelayCommand]
        private async Task CreateDirectory()
        {
            Debug.Assert(CanAlterDirectory);
            await directory.CreateAsync(true);
        }

        #endregion

        #region Open

        [RelayCommand]
        public async Task Open(InteractiveStorageItem item)
        {
            switch (item)
            {
                case ILaunchable launchable:
                    await launchable.LaunchAsync();
                    break;
                case IStorage<IDirectoryItem> storage:
                    // Send message for directory page (new directory should be opened)
                    Messenger.Send(new NavigationRequiredMessage(storage));
                    // Send message to navigation view model to notify that new directory is opened
                    Messenger.Send(new StorageNavigatedMessage(storage));
                    break;
                default:
                    throw new ArgumentException("Cannot open provided item. It is not a directory or file.", nameof(item));
            }
        }

        [RelayCommand]
        public void OpenInNewTab(IDirectory directory)
        {
            Messenger.Send(new OpenTabMessage(directory));
        }

        #endregion

        #region Rename

        [RelayCommand(CanExecute = nameof(HasOperatedItems))]
        private void BeginRenamingSelectedItem() => BeginRenamingItem(OperatedItems[0]);

        /// <summary>
        /// Begins renaming provided object
        /// </summary>
        /// <param name="item"> Object that is renamed </param>
        [RelayCommand]
        public void BeginRenamingItem(IRenameableObject item) => item.BeginEdit();

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        public async Task EndRenamingItem(IRenameableObject item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.CancelEdit();
                await App.MainWindow.ShowMessageDialogAsync("Item's name cannot be empty", "Empty name is illegal");
                return;
            }
            item.Rename();
        }

        /// <summary>
        /// Ends renaming item when it is renamed.
        /// This method is called before any operation with item to be sure it's not renamed while executing operation
        /// </summary>
        /// <param name="item"> Item that we are checking for renaming </param>
        [RelayCommand]
        public async Task EndRenamingIfNeeded(IDirectoryItem item)
        {
            if (item.IsRenamed)
            {
                await EndRenamingItem(item);
            }
        }

        #endregion


        #region Delete

        /// <summary>
        /// Moves selected items to a recycle bin
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanChangeDirectory))]
        private async Task RecycleSelectedItems()
        {
            while (OperatedItems.Count > 0)
            {
                await TryDeleteItem(OperatedItems[0]);
            }
        }

        /// <summary>
        /// Permanently deletes selected items
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanChangeDirectory))]
        public async Task DeleteSelectedItems()
        {
            var content = $"Do you really want to delete {(OperatedItems.Count > 1 ? "selected items" : $"\"{OperatedItems[0].Path}\""
                )} permanently?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            while (OperatedItems.Count > 0)
            {
                await TryDeleteItem(OperatedItems[0], true);
            }
        }

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        private async Task TryDeleteItem(IDirectoryItem item, bool isPermanent = false)
        {
            var removed = new List<IDirectoryItem>();
            await EndRenamingIfNeeded(item);

            try
            {
                if (isPermanent)
                {
                    item.Delete();
                }
                else
                {
                    await item.RecycleAsync();
                }

                removed.Add(item);
            }
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync(e.Message, "File operation canceled");
            }
            finally
            {
                OperatedItems.Remove(item);
            }

            //TODO: Send message with deleted items
        }

        #endregion

        [RelayCommand]
        public async Task ShowDetails(DirectoryItemWrapper item)
        {
            var details = item.GetBasicInfo();

            if (item is FileWrapper file)
            {
                details.TitleInfo = await file.GetFileTypeAsync();
            }

            Messenger.Send(new ShowDetailsMessage(details));
        }

        [RelayCommand]
        private void Pin(DirectoryWrapper directory)
        {

        }

        [RelayCommand]
        private void Unpin(DirectoryWrapper directory)
        {

        }

        [RelayCommand]
        private void Copy(DirectoryItemWrapper item)
        {

        }
    }
}
