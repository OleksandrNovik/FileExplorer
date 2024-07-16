#nullable enable
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace FileExplorer.Contracts
{
    public interface ITabService
    {
        public Frame? CurrentTab { get; set; }
        public ObservableCollection<TabModel> Tabs { get; }
        public void CreateNewTab(DirectoryWrapper? directory);
        public void Navigate(TabModel tab);
    }
}
