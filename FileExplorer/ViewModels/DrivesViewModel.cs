using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Helpers.General;
using Models.Storage.Drives;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class DrivesViewModel : ObservableRecipient
    {
        //TODO: Send navigation messages
        public ObservableCollection<DriveWrapper> Drives { get; }
        public DrivesViewModel()
        {
            //Getting only ready drives
            var availableDrives = DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .Select(drive => new DriveWrapper(drive));

            Drives = new ObservableCollection<DriveWrapper>(availableDrives);
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            foreach (var drive in Drives)
            {
                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    await drive.UpdateThumbnailAsync(40);
                });
            }
        }
    }
}
