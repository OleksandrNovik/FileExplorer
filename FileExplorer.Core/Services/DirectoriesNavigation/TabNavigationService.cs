#nullable enable
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Services.General;
using Models.TabRelated;

namespace FileExplorer.Core.Services.DirectoriesNavigation
{
    public class TabNavigationService : BaseNavigationService, ITabNavigationService
    {
        public void NavigateTo(TabModel? value = default, object? parameter = null)
        {
            if (value is not null)
            {
                UseNavigationFrame(value.TabType, parameter ?? value);
            }
        }
    }
}
