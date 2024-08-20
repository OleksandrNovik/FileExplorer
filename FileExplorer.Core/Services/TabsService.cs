#nullable enable
using FileExplorer.Core.Contracts;
using Models.Contracts.Storage;
using Models.Storage.Windows;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Services
{
    public class TabsService : ITabService
    {
        public ObservableCollection<TabModel> Tabs { get; } = new();
        public TabModel SelectedTab { get; set; }

        public void CreateNewTab(IStorage<DirectoryItemWrapper>? directory)
        {
            Tabs.Add(new TabModel(directory));
        }

    }
}
