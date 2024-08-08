#nullable enable
using FileExplorer.Core.Contracts.General;
using Microsoft.UI.Xaml.Navigation;

namespace FileExplorer.Core.Contracts.Settings
{
    public interface ISettingsNavigationService : INavigationService<string>
    {
        event NavigatedEventHandler Navigated;

        public void NavigateToDefault();
    }
}
