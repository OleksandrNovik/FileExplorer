#nullable enable
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Services.NavigationViewServices;
using FileExplorer.ViewModels.General;

namespace FileExplorer.ViewModels.Settings
{
    public sealed class SettingsViewModel : BaseNavigationViewModel
    {
        public BaseNavigationViewService<string> NavigationViewService { get; }
        public ISettingsNavigationService NavigationService { get; }

        public SettingsViewModel(BaseNavigationViewService<string> navigationViewService, ISettingsNavigationService navigationService)
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
