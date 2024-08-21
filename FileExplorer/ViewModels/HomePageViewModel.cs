using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using Helpers.General;
using Models.Messages;
using Models.Storage.Drives;
using System;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class HomePageViewModel : ObservableRecipient, INavigationAware
    {
        //TODO: Send navigation messages
        public ObservableDrivesCollection ObservableDrives { get; private set; }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            foreach (var drive in ObservableDrives)
            {
                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    await drive.UpdateThumbnailAsync(40);
                });
            }
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is ObservableDrivesCollection drives)
            {
                ObservableDrives = drives;
                Messenger.Send(new TabStorageChangedMessage(drives));
            }
            else
                throw new ArgumentException("Parameter is not ObservableDrivesCollection", nameof(parameter));
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
