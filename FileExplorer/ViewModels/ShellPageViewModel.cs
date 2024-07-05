#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

        private TabModel selectedTab;

        public ShellPageViewModel(ITabService tabService)
        {
            TabService = tabService;
            Tabs = TabService.Tabs;

            Messenger.Register<ShellPageViewModel, DirectoryInfo>(this, (_, message) =>
            {
                if (selectedTab != null)
                {
                    selectedTab.TabDirectory = message;
                }
            });
        }

        [RelayCommand]
        private void OpenNewTab(DirectoryInfo? directory = null)
        {
            NewTab(directory);
        }

        public void NewTab(DirectoryInfo? directory = null)
        {
            TabService.CreateNewTab(directory);
        }

        private void NavigateToTab(TabModel item)
        {
            selectedTab = item;
            TabService.Navigate(item);
        }

        [RelayCommand]
        private void RemoveTab(TabModel item)
        {
            Tabs.Remove(item);
        }

        [RelayCommand]
        private void SelectTab(TabModel item)
        {
            NavigateToTab(item);
        }
    }
}
