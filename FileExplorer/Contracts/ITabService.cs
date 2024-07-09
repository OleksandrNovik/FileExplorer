#nullable enable
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface ITabService
    {
        public Frame? CurrentTab { get; set; }
        public ObservableCollection<TabModel> Tabs { get; }
        public void CreateNewTab(StorageFolder? directory);
        public void Navigate(TabModel tab);
    }
}
