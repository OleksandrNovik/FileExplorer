#nullable enable
using FileExplorer.Contracts;
using FileExplorer.Models;
using Helpers;
using Microsoft.UI.Xaml.Controls;
using Models.StorageWrappers;
using System.Collections.ObjectModel;

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

        public void CreateNewTab(DirectoryWrapper? directory)
        {
            var newTab = pageService.CreateTabFromDirectory(directory);
            Tabs.Add(newTab);
        }

        public void Navigate(TabModel tab)
        {
            var previousViewModel = CurrentTab.GetPageViewModel();

            var navigated = CurrentTab.Navigate(tab.TabType, tab);

            if (navigated)
            {
                if (previousViewModel is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

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
