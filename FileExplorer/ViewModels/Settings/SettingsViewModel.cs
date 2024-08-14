#nullable enable
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.ViewModels.General;

namespace FileExplorer.ViewModels.Settings
{
    public sealed class SettingsViewModel : BaseNavigationViewModel
    {
        public ISettingsNavigationService NavigationService { get; }

        public SettingsViewModel(ISettingsNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
