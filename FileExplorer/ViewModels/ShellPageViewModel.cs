#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using Models.Messages;
using Models.StorageWrappers;
using Models.TabRelated;
using System.Collections.ObjectModel;

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

            Messenger.Register<ShellPageViewModel, DirectoryWrapper>(this, (_, message) =>
            {
                if (selectedTab != null)
                {
                    selectedTab.TabDirectory = message;
                }
            });

            Messenger.Register<ShellPageViewModel, OpenTabMessage>(this, (_, message) =>
            {
                NewTab(message.TabDirectory);
            });
        }

        [RelayCommand]
        private void OpenNewTab(DirectoryWrapper? directory = null)
        {
            NewTab(directory);
        }

        private void NewTab(DirectoryWrapper? directory = null)
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
