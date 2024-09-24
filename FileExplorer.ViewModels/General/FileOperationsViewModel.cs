#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts.Clipboard;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Helpers;
using FileExplorer.Helpers.Application;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Contracts.Storage.Properties;
using FileExplorer.Models.Messages;
using FileExplorer.Models.Storage.Abstractions;
using System;
using System.Collections.Generic;
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
        private readonly ILocalSettingsService localSettings;

        private readonly StorageItemsNamingViewModel namingViewModel;

        public FileOperationsViewModel(
            IClipboardService clipboardService,
            ILocalSettingsService localSettingsService,
            StorageItemsNamingViewModel namingViewModel)
        {
            clipboard = clipboardService;
            this.namingViewModel = namingViewModel;
            localSettings = localSettingsService;

            clipboard.FileDropListChanged += NotifyCanPaste;
            clipboard.CutOperationStarted += OnCutOperation;
        }

        /// <summary>
        /// When cut event occurs we remove item drop list from clipboard
        /// </summary>
        private void OnCutOperation(object? sender, CutOperationData e)
        {
            clipboard.Clear();
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
                    OpenStorage(storage);
                    break;
                default:
                    throw new ArgumentException("Cannot open provided item. It is not a Directory or file.", nameof(item));
            }
        }

        /// <summary>
        /// Opens storage depending on "Open in new tab" setting value 
        /// </summary>
        /// <param name="storage"> Storage to open </param>
        private void OpenStorage(IStorage storage)
        {
            var openInNewTab = localSettings.ReadBool(LocalSettings.Keys.OpenFolderInNewTab);

            if (openInNewTab is true)
            {
                OpenInNewTab(storage);
            }
            else
            {
                // Send message for Directory page (new Directory should be opened)
                Messenger.Send(new NavigationRequiredMessage(storage));
                // Send message to navigation view model to notify that new Directory is opened
                Messenger.Send(new StorageNavigatedMessage(storage));
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

        #region EndRename

        /// <summary>
        /// Begins renaming provided object
        /// </summary>
        /// <param name="item"> Object that is renamed </param>
        [RelayCommand]
        public void BeginRenamingItem(IRenameableObject item)
        {
            namingViewModel.BeginRenamingItemCommand.Execute(item);
        }


        /// <summary>
        /// Ends renaming item when it is renamed.
        /// This method is called before any operation with item to be sure it's not renamed while executing operation
        /// </summary>
        /// <param name="item"> ItemProperties that we are checking for renaming </param>
        [RelayCommand]
        public async Task ForceRenamingAsync(IRenameableObject item)
        {
            if (item.IsRenamed)
            {
                await namingViewModel.EndRenamingItemCommand.ExecuteAsync(item);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Fully deletes item (if it is possible)
        /// </summary>
        /// <param name="item"> Wrapper item to delete </param>
        /// <param name="isPermanent"> Is item being deleted permanently or not </param>
        public async Task DeleteItem(IDirectoryItem item, bool isPermanent)
        {
            await ForceRenamingAsync(item);

            if (isPermanent)
            {
                item.Delete();
            }
            else
            {
                await item.RecycleAsync();
            }
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
        public async Task CopyItemsAsync(IReadOnlyCollection<IDirectoryItem> items)
        {
            foreach (var item in items)
            {
                await ForceRenamingAsync(item);
            }
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
