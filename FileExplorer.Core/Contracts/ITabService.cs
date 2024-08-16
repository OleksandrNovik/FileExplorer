#nullable enable
using Models.Storage.Windows;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Contracts
{
    public interface ITabService
    {
        public ObservableCollection<TabModel> Tabs { get; }
        public TabModel SelectedTab { get; set; }
        public void CreateNewTab(DirectoryWrapper? directory);
    }
}
