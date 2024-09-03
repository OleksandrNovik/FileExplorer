using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Models.Contracts.Storage;
using Models.Messages;
using Models.Storage.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.General
{
    /// <summary>
    /// View model that contains logic to operate files and other storage items
    /// </summary>
    public sealed partial class FileOperationsViewModel : ObservableRecipient
    {
        /// <summary>
        /// Field that contains directory to create items in (if it is allowed on the page)
        /// </summary>
        private IDirectory directory;

        /// <summary>
        /// Field that marks if there can be executed create/delete operations on the page
        /// </summary>
        [ObservableProperty]
        private bool canAlterDirectory;

        public ViewOptionsViewModel ViewOptions { get; }

        /// <summary>
        /// Contains selected items to operate them using toolbar
        /// </summary>
        public ObservableCollection<IDirectoryItem> SelectedItems { get; } = new();

        public FileOperationsViewModel()
        {
            SelectedItems.CollectionChanged += NotifyCanExecute;
            ViewOptions = App.GetService<ViewOptionsViewModel>();

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

        private bool HasOperatedItems() => SelectedItems.Count > 0;

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
        public void Open(InteractiveStorageItem item)
        {
            switch (item)
            {
                case ILaunchable launchable:
                    launchable.Launch();
                    break;
                case IStorage storage:
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
        private void BeginRenamingSelectedItem() => BeginRenamingItem(SelectedItems[0]);

        /// <summary>
        /// Begins renaming provided object
        /// </summary>
        /// <param name="item"> Object that is renamed </param>
        [RelayCommand]
        public void BeginRenamingItem(IRenameableObject item)
        {
            item.BeginEdit();

            if (!item.CanRename)
                Messenger.Send(new ShowInfoBarMessage(InfoBarSeverity.Informational, "This item cannot be renamed"));
        }

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(IRenameableObject item)
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
        public async Task EndRenamingIfNeeded(IRenameableObject item)
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
            while (SelectedItems.Count > 0)
            {
                await TryDeleteItem(SelectedItems[0]);
            }
        }

        /// <summary>
        /// Permanently deletes selected items
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanChangeDirectory))]
        public async Task DeleteSelectedItems()
        {
            var content = $"Do you really want to delete {(SelectedItems.Count > 1 ? "selected items" : $"\"{SelectedItems[0].Path}\""
                )} permanently?";
            var result = await App.MainWindow.ShowYesNoDialog(content, "Deleting items");

            if (result == ContentDialogResult.Secondary) return;

            while (SelectedItems.Count > 0)
            {
                await TryDeleteItem(SelectedItems[0], true);
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
                SelectedItems.Remove(item);
            }

            //TODO: Send message with deleted items
        }

        #endregion

        [RelayCommand]
        public void ShowDetails(InteractiveStorageItem item)
        {
            //var details = item.GetBasicInfo();

            //if (item is FileWrapper file)
            //{
            //    details.TitleInfo = await file.GetFileTypeAsync();
            //}

            //Messenger.Send(new ShowDetailsMessage(details));
        }

        [RelayCommand]
        private void Pin(IDirectory directory)
        {

        }

        [RelayCommand]
        private void Unpin(IDirectory directory)
        {

        }

        [RelayCommand]
        private void Copy(IDirectoryItem item)
        {

        }
    }
}
