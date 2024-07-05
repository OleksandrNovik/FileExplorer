#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Contracts;
using FileExplorer.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.ViewModels
{
    public sealed partial class ShellPageViewModel : ObservableRecipient
    {
        public ITabService TabService { get; }

        [ObservableProperty]
        private ObservableCollection<TabModel> tabs;

        public ShellPageViewModel(ITabService tabService)
        {
            TabService = tabService;
            Tabs = TabService.Tabs;
        }

        [RelayCommand]
        private void OpenNewTab(DirectoryInfo? directory = null)
        {
            NewTab(directory);
            NavigateToTab(Tabs.Count - 1);
        }

        public void NewTab(DirectoryInfo? directory = null)
        {
            TabService.CreateNewTab(directory);
        }

        private void NavigateToTab(int index)
        {
            TabService.Navigate(index);
        }

        [RelayCommand]
        private void RemoveTab(TabModel item)
        {
            Tabs.Remove(item);
        }
    }
}
