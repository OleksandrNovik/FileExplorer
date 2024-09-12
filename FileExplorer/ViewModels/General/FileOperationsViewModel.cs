#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts.Clipboard;
using FileExplorer.Helpers;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Contracts.Storage.Properties;
using FileExplorer.Models.Messages;
using FileExplorer.Models.Storage.Abstractions;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer.ViewModels.General
{
    /// <summary>
    /// View model that contains logic to operate files and other storage items
    /// </summary>
    public sealed partial class FileOperationsViewModel : ObservableRecipient
    {
        private readonly IClipboardService clipboard;
        public FileOperationsViewModel()
        {
            clipboard = App.GetService<IClipboardService>();
            clipboard.FileDropListChanged += NotifyCanPaste;
        }

        /// <summary>
        /// Checks if there is any files inside of clipboard 
        /// </summary>
        private bool CanPaste() => clipboard.HasFiles;

        private void NotifyCanPaste(object? sender, EventArgs args)
        {
            PasteCommand.NotifyCanExecuteChanged();
        }

        #region Open

        /// <summary>
        /// Opens item that provided as parameter
        /// </summary>
        /// <exception cref="ArgumentException"> System could not open item </exception>
        [RelayCommand]
        public void Open(InteractiveStorageItem item)
        {
            switch (item)
            {
                case ILaunchable launchable:
                    launchable.Launch();
                    break;
                case IStorage storage:
                    // Send message for Directory page (new Directory should be opened)
                    Messenger.Send(new NavigationRequiredMessage(storage));
                    // Send message to navigation view model to notify that new Directory is opened
                    Messenger.Send(new StorageNavigatedMessage(storage));
                    break;
                default:
                    throw new ArgumentException("Cannot open provided item. It is not a Directory or file.", nameof(item));
            }
        }

        /// <summary>
        /// Opens storage in new tab
        /// </summary>
        /// <param name="storage"> Storage to open in new tab </param>
        [RelayCommand]
        public void OpenInNewTab(IStorage storage)
        {
            Messenger.Send(new OpenTabMessage(storage));
        }

        #endregion

        #region Rename

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
        /// <param name="item"> ItemProperties that has to be given new name </param>
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
        /// <param name="item"> ItemProperties that we are checking for renaming </param>
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
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        /// <returns> true if item is successfully deleted, otherwise - false </returns>
        public async Task<bool> TryDeleteItem(IDirectoryItem item, bool isPermanent)
        {
            bool hasDeleted;
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

                hasDeleted = true;
            }
            catch (IOException e)
            {
                await App.MainWindow.ShowMessageDialogAsync(e.Message, "File cannot be deleted");
                hasDeleted = false;
            }

            return hasDeleted;
        }

        #endregion

        [RelayCommand]
        public void ShowDetails(IBasicPropertiesProvider propertiesProvider)
        {
            var properties = propertiesProvider.GetBasicProperties();
            properties.UpdateThumbnail(Constants.ThumbnailSizes.Details);

            Messenger.Send(new ShowPropertiesMessage(properties));
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
        public void CopyItems(IReadOnlyCollection<IDirectoryItem> items)
        {
            clipboard.SetFiles(items, DragDropEffects.Copy);
        }

        /// <summary>
        /// Pastes items from clipboard into the directory
        /// </summary>
        /// <param name="destination"> Destination directory </param>
        [RelayCommand(CanExecute = nameof(CanPaste))]
        public void Paste(IDirectory destination)
        {
            var data = clipboard.GetFiles();

            if (data is not null)
            {
                PasteAndGetItems(data, destination);
            }
            else
            {
                throw new NullReferenceException("Cannot paste items, since data provided from clipboard is null");
            }
        }

        public ICollection<IDirectoryItem> PasteAndGetItems(ClipboardFileOperation data, IDirectory destination)
        {
            ICollection<IDirectoryItem> operationResult;

            // Contains copy flag
            if ((data.Operation & DragDropEffects.Copy) != 0)
            {
                operationResult = Copy(data.DirectoryItems, destination.Path).ToArray();
            }
            // Contains cut flag
            else if ((data.Operation & DragDropEffects.Move) != 0)
            {
                operationResult = Move(data.DirectoryItems, destination.Path).ToArray();
            }
            else
            {
                throw new ArgumentException($"Illegal operation. Value: {data.Operation}", nameof(data.Operation));
            }

            return operationResult;
        }

        private IEnumerable<IDirectoryItem> Copy(IEnumerable<IDirectoryItem> items, string destination)
        {
            foreach (var item in items)
            {
                var copy = item.Copy(destination);
                yield return copy;
            }
        }
        private IEnumerable<IDirectoryItem> Move(IEnumerable<IDirectoryItem> items, string destination)
        {
            foreach (var item in items)
            {
                item.Move(destination);
                yield return item;
            }
        }
    }
}
