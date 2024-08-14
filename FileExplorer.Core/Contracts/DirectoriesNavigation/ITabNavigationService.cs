using FileExplorer.Core.Contracts.General;
using Models.TabRelated;

namespace FileExplorer.Core.Contracts.DirectoriesNavigation
{
    /// <summary>
    /// Interface to identify service that provides tab navigation
    /// </summary>
    public interface ITabNavigationService : IBasicNavigationService<TabModel>;
}
