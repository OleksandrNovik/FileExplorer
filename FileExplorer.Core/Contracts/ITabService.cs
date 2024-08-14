#nullable enable
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using Models.StorageWrappers;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Contracts
{
    public interface ITabService
    {
        public ITabNavigationService TabNavigationService { get; }
        public ObservableCollection<TabModel> Tabs { get; }
        public void CreateNewTab(DirectoryWrapper? directory);
    }
}
