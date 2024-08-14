#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using Models.StorageWrappers;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Services
{
    public class TabsService : ITabService
    {
        private readonly IPageTypesService pageTypesService;
        public ITabNavigationService TabNavigationService { get; }
        public ObservableCollection<TabModel> Tabs { get; } = new();

        public TabsService(IPageTypesService pageTypesService, ITabNavigationService navigationService)
        {
            this.pageTypesService = pageTypesService;
            TabNavigationService = navigationService;
        }

        public void CreateNewTab(DirectoryWrapper? directory)
        {
            var newTab = pageTypesService.CreateTabFromDirectory(directory);
            Tabs.Add(newTab);
        }
    }
}
