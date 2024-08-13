using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Services.General;
using Microsoft.UI.Xaml.Navigation;
using Models.TabRelated;

namespace FileExplorer.Core.Services.DirectoriesNavigation
{
    public class NavigationService : BaseNavigationService, INavigationService
    {
        public void NavigateTo(TabModel value)
        {
            UseNavigationFrame(value.TabType, value);
        }

        public event NavigatedEventHandler Navigated;
    }
}
