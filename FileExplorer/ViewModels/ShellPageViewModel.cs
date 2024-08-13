#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Services.NavigationViewServices;
using FileExplorer.ViewModels.General;
using Models.Messages;
using Models.StorageWrappers;
using Models.TabRelated;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels
{
    public sealed partial class ShellPageViewModel : BaseNavigationViewModel
    {
        public INavigationService NavigationService { get; }
        public BaseNavigationViewService<TabModel> NavigationViewService { get; }

        public readonly ITabService tabService;

        [ObservableProperty]
        private ObservableCollection<TabModel> tabs;

        private TabModel selectedTab;

        public ShellPageViewModel(ITabService tabService, INavigationService navigationService, BaseNavigationViewService<TabModel> navigationViewService)
        {
            this.tabService = tabService;
            tabs = this.tabService.Tabs;
            NavigationService = navigationService;
            NavigationViewService = navigationViewService;

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
            tabService.CreateNewTab(directory);
        }

        private void NavigateToTab(TabModel item)
        {
            selectedTab = item;
            NavigationService.NavigateTo(item);
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
