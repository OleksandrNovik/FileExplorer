#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.ViewModels.General;
using Models.Messages;
using Models.StorageWrappers;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels
{
    public sealed partial class ShellPageViewModel : BaseNavigationViewModel
    {
        public NavigationPaneViewModel NavigationPaneViewModel { get; } = new();
        public ITabService TabService { get; }
        public INavigationService NavigationService { get; }

        [ObservableProperty]
        private ObservableCollection<TabModel> tabs;

        private TabModel selectedTab;

        public ShellPageViewModel(ITabService tabService, INavigationService navigationService)
        {
            TabService = tabService;
            NavigationService = navigationService;
            tabs = TabService.Tabs;

            Messenger.Register<ShellPageViewModel, TabDirectoryChangedMessage>(this, (_, message) =>
            {
                if (selectedTab != null)
                {
                    selectedTab.TabDirectory = message.Directory;
                }
                NavigationPaneViewModel.SetIcons();
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
            NavigationService.NavigateTo(item.TabDirectory.Path, item);
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
