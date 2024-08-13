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

    }
}
