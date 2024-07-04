#nullable enable
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.Services
{
    public class TabsService : ITabService
    {
        private readonly IPageService pageService;
        private Frame? currTab;

        public Frame? CurrentTab
        {
            get => currTab;
            set
            {
                UnregisterFrameEvents();
                currTab = value;
                RegisterFrameEvents();
            }
        }

        private void RegisterFrameEvents()
        {
            if (currTab != null)
            {
                currTab.Navigated += OnNavigated;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (currTab != null)
            {
                currTab.Navigated -= OnNavigated;
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

        public ObservableCollection<TabModel> Tabs { get; } = new();

        public TabsService(IPageService pageService)
        {
            this.pageService = pageService;
        }

        public void OpenNewTab(DirectoryInfo? directory)
        {
            CreateNewTab(directory);
            Navigate(Tabs.Count - 1);
        }

        public void CreateNewTab(DirectoryInfo? directory)
        {
            var newTab = pageService.CreateTabFromDirectory(directory);
            Tabs.Add(newTab);
        }

        public void Navigate(int tabIndex)
        {
            var selectedTab = Tabs[tabIndex];
            var n = CurrentTab?.Navigate(selectedTab.TabType, selectedTab.TabDirectory);

            if (n.Value)
            {

            }
        }

    }
}
