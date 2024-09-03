using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.Abstractions;
using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml.Controls;
using Models;
using Models.ModelHelpers;
using Models.Storage.Drives;
using Models.Storage.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class HomePageViewModel : StorageViewModel
    {
        public ObservableDrivesCollection Drives { get; private set; }
        public List<DirectoryWrapper> Libraries { get; }
        public ConcurrentWrappersCollection RecentItems { get; set; }

        public HomePageViewModel(FileOperationsViewModel fileOperations, IMenuFlyoutFactory factory) : base(fileOperations, factory)
        {
            Libraries = KnownFoldersHelper.Libraries.ToList();
        }

        [RelayCommand]
        private void InitializeLibrariesThumbnails()
        {
            foreach (var library in Libraries)
            {
                library.UpdateThumbnail(80);
            }
        }

        [RelayCommand]
        private void InitializeDrivesThumbnails()
        {
            foreach (var drive in Drives)
            {
                drive.UpdateThumbnail(80);
            }
        }

        [RelayCommand]
        private async Task InitializeRecentItems()
        {
            RecentItems = [.. KnownFoldersHelper.TopRecentItems.Take(20)];
            OnPropertyChanged(nameof(RecentItems));
            await RecentItems.UpdateIconsAsync(90, CancellationToken.None);
        }

        public override void OnNavigatedTo(object parameter)
        {
            base.OnNavigatedTo(parameter);
            Drives = Storage as ObservableDrivesCollection;
            ArgumentNullException.ThrowIfNull(Drives);
        }

        public override IList<MenuFlyoutItemBase> BuildContextMenu(object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
