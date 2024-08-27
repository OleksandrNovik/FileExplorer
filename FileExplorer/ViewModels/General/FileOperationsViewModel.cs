using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models.Contracts.Storage;
using Models.Messages;
using Models.Storage.Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class FileOperationsViewModel : ObservableRecipient
    {
        private IDirectory directory;

        [ObservableProperty]
        private bool canCreateItems;

        public ObservableCollection<IDirectoryItem> OperatedItems { get; } = new();

        public FileOperationsViewModel()
        {
            Messenger.Register<FileOperationsViewModel, TabStorageChangedMessage>(this, (_, message) =>
            {
                if (message.Storage is IDirectory dir)
                {
                    directory = dir;
                    CanCreateItems = true;
                }
                else
                {
                    CanCreateItems = false;
                }
            });
        }

        /// <summary>
        /// Creates file in <see cref="directory"/>
        /// </summary>
        [RelayCommand]
        private async Task CreateFile()
        {
            Debug.Assert(CanCreateItems);
            await directory.CreateAsync(false);
        }

        /// <summary>
        /// Creates folder in <see cref="directory"/>
        /// </summary>
        [RelayCommand]
        private async Task CreateDirectory()
        {
            Debug.Assert(CanCreateItems);
            await directory.CreateAsync(true);
        }

        [RelayCommand]
        public async Task Open(IDirectoryItem item)
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
        public void OpenInNewTab(DirectoryWrapper directory)
        {
            Messenger.Send(new OpenTabMessage(directory));
        }

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
