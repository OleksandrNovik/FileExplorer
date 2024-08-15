using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models.General;
using Models.Messages;
using Models.StorageWrappers;
using System;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class FileOperationsViewModel : ObservableRecipient
    {
        [RelayCommand]
        public async Task Open(DirectoryItemWrapper wrapper)
        {
            switch (wrapper)
            {
                case FileWrapper fileWrapper:
                    await fileWrapper.LaunchAsync();
                    break;
                case DirectoryWrapper directoryWrapper:
                    // Send message for directory page (new directory should be opened)
                    Messenger.Send(new NavigationRequiredMessage(directoryWrapper));
                    // Send message to navigation view model to notify that new directory is opened
                    var navigationModel = new DirectoryNavigationInfo(directoryWrapper);
                    Messenger.Send(navigationModel);
                    break;
                default:
                    throw new ArgumentException("Cannot open provided item. It is not a directory or file.", nameof(wrapper));
            }
        }

        [RelayCommand]
        public void OpenInNewTab(DirectoryWrapper directory)
        {
            Messenger.Send(new OpenTabMessage(directory));
        }

        /// <summary>
        /// Begins renaming provided item
        /// </summary>
        /// <param name="item"> Item that is renamed </param>
        [RelayCommand]
        public void BeginRenamingItem(DirectoryItemWrapper item) => item.BeginEdit();

        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        public async Task EndRenamingItem(DirectoryItemWrapper item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.CancelEdit();
                await App.MainWindow.ShowMessageDialogAsync("Item's name cannot be empty", "Empty name is illegal");
                return;
            }
            item.Rename();

            // If item's extension changed we need to update icon
            if (item.HasExtensionChanged)
            {
                await item.UpdateThumbnailAsync();
            }
        }


        /// <summary>
        /// Ends renaming item when it is renamed.
        /// This method is called before any operation with item to be sure it's not renamed while executing operation
        /// </summary>
        /// <param name="item"> Item that we are checking for renaming </param>
        [RelayCommand]
        public async Task EndRenamingIfNeeded(DirectoryItemWrapper item)
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

            await item.UpdateThumbnailAsync();

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
