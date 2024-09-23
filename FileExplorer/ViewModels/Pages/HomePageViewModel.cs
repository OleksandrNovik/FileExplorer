using CommunityToolkit.Mvvm.Input;
using FileExplorer.Helpers;
using FileExplorer.Models;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Drives;
using FileExplorer.Models.Storage.Windows;
using FileExplorer.ViewModels.Abstractions;
using FileExplorer.ViewModels.General;
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

        /// <summary>
        /// Updates thumbnail for each library
        /// </summary>
        [RelayCommand]
        private void InitializeLibrariesThumbnails()
        {
            foreach (var library in Libraries)
            {
                library.UpdateThumbnail(Constants.ThumbnailSizes.Big);
            }
        }

        /// <summary>
        /// Updates thumbnail for each drive
        /// </summary>
        [RelayCommand]
        private void InitializeDrivesThumbnails()
        {
            foreach (var drive in Drives)
            {
                drive.UpdateThumbnail(Constants.ThumbnailSizes.Medium);
            }
        }

        /// <summary>
        /// Gets recent items and updates thumbnails for each item
        /// </summary>
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
    }
}
