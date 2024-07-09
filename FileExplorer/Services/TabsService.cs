#nullable enable
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class TabsService : ITabService
    {
        private readonly IPageService pageService;
        public ObservableCollection<TabModel> Tabs { get; } = new();



        private Frame? currentTab;
        public Frame? CurrentTab
        {
            get => currentTab;
            set
            {
                UnregisterFrameEvents();
                currentTab = value;
                RegisterFrameEvents();
            }
        }

        private void OnNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                if (frame.GetPageViewModel() is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.Parameter);
                }
            }
        }

        public TabsService(IPageService pageService)
        {
            this.pageService = pageService;
        }

        public void CreateNewTab(StorageFolder? directory)
        {
            var newTab = pageService.CreateTabFromDirectory(directory);
            Tabs.Add(newTab);
        }

        public void Navigate(TabModel tab)
        {
            CurrentTab?.Navigate(tab.TabType, tab);
        }

        private void RegisterFrameEvents()
        {
            if (currentTab != null)
            {
                currentTab.Navigated += OnNavigated;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (currentTab != null)
            {
                currentTab.Navigated -= OnNavigated;
            }
        }

    }
}
