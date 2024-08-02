#nullable enable
using Microsoft.UI.Xaml.Controls;
using Models.StorageWrappers;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Contracts
{
    public interface ITabService
    {
        public Frame? CurrentTab { get; set; }
        public ObservableCollection<TabModel> Tabs { get; }
        public void CreateNewTab(DirectoryWrapper? directory);
        public void Navigate(TabModel tab);
    }
}
