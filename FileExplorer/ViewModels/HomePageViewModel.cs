using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.Abstractions;
using Helpers.General;
using Microsoft.UI.Xaml.Controls;
using Models.ModelHelpers;
using Models.Storage.Drives;
using Models.Storage.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class HomePageViewModel : StorageViewModel
    {
        public ObservableDrivesCollection Drives { get; private set; }
        public List<DirectoryWrapper> Libraries { get; }

        public HomePageViewModel(IMenuFlyoutFactory factory) : base(factory)
        {
            Libraries = KnownFoldersHelper.Libraries.ToList();
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

            foreach (var library in Libraries)
            {
                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    await library.UpdateThumbnailAsync(80);
                });
            }
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
