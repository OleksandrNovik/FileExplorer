using CommunityToolkit.Mvvm.Input;
using FileExplorer.ViewModels.Abstractions;
using FileExplorer.ViewModels.General;
using Helpers;
using Models;
using Models.ModelHelpers;
using Models.Storage.Drives;
using Models.Storage.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.Pages
{
    public sealed partial class HomePageViewModel : BaseSelectionViewModel
    {
        public ObservableDrivesCollection Drives { get; private set; }
        public List<DirectoryWrapper> Libraries { get; }
        public ConcurrentWrappersCollection RecentItems { get; set; }

        public HomePageViewModel(FileOperationsViewModel fileOperations)
            : base(fileOperations)
        {
            Libraries = KnownFoldersHelper.Libraries.ToList();
        }

        [RelayCommand]
        private void InitializeLibrariesThumbnails()
        {
            foreach (var library in Libraries)
            {
                library.UpdateThumbnail(Constants.ThumbnailSizes.Big);
            }
        }

        [RelayCommand]
        private void InitializeDrivesThumbnails()
        {
            foreach (var drive in Drives)
            {
                drive.UpdateThumbnail(Constants.ThumbnailSizes.Medium);
            }
        }

        [RelayCommand]
        private async Task InitializeRecentItems()
        {
            RecentItems = [.. KnownFoldersHelper.TopRecentItems.Take(20)];
            OnPropertyChanged(nameof(RecentItems));
            await RecentItems.UpdateIconsAsync(Constants.ThumbnailSizes.Big, CancellationToken.None);
        }

        public override void OnNavigatedTo(object parameter)
        {
            base.OnNavigatedTo(parameter);
            Drives = Storage as ObservableDrivesCollection;
            ArgumentNullException.ThrowIfNull(Drives);
        }

        public override IReadOnlyList<MenuFlyoutItemViewModel> BuildMenu(object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
