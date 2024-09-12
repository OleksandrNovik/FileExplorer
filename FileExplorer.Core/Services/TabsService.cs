#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Services
{
    public class TabsService : ITabService
    {
        public ObservableCollection<TabModel> Tabs { get; } = new();
        public TabModel SelectedTab { get; set; }

        public void CreateNewTab(IStorage? directory)
        {
            Tabs.Add(new TabModel(directory));
        }

    }
}
