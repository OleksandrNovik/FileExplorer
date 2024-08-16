#nullable enable
using FileExplorer.Core.Contracts;
using Models.Storage.Windows;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Services
{
    public class TabsService : ITabService
    {
        private readonly IPageTypesService pageTypesService;
        public ObservableCollection<TabModel> Tabs { get; } = new();
        public TabModel SelectedTab { get; set; }

        public TabsService(IPageTypesService pageTypesService)
        {
            this.pageTypesService = pageTypesService;
        }

        public void CreateNewTab(DirectoryWrapper? directory)
        {
            var newTab = pageTypesService.CreateTabFromDirectory(directory);
            Tabs.Add(newTab);
        }

    }
}
