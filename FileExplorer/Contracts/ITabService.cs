#nullable enable
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.Contracts
{
    public interface ITabService
    {
        public Frame? CurrentTab { get; set; }
        public ObservableCollection<TabModel> Tabs { get; }
        public void CreateNewTab(DirectoryInfo? directory);
        public void OpenNewTab(DirectoryInfo? directory);
        public void Navigate(int tabIndex);
    }
}
