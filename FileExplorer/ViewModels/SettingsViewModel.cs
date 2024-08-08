#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts.Settings;

namespace FileExplorer.ViewModels
{
    public sealed partial class SettingsViewModel : ObservableObject
    {
        public ISettingsNavigationViewService NavigationViewService { get; }
        public ISettingsNavigationService NavigationService { get; }

        [ObservableProperty]
        public object? selected;

        public SettingsViewModel(ISettingsNavigationViewService navigationViewService, ISettingsNavigationService navigationService)
        {
            NavigationViewService = navigationViewService;
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigation;
        }

        private void OnNavigation(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);

            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }
    }
}
