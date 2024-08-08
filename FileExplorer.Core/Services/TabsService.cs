#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Services.General;
using Helpers.Application;
using Microsoft.UI.Xaml.Controls;
using Models.StorageWrappers;
using Models.TabRelated;
using System;
using System.Collections.ObjectModel;

namespace FileExplorer.Core.Services
{
    public class TabsService : BaseNavigationService, ITabService
    {
        private readonly IPageService pageService;
        public ObservableCollection<TabModel> Tabs { get; } = new();

        public TabsService(IPageService pageService)
        {
            this.pageService = pageService;
        }

        public void CreateNewTab(DirectoryWrapper? directory)
        {
            var newTab = pageService.CreateTabFromDirectory(directory);
            Tabs.Add(newTab);
        }

        public void NavigateTo(TabModel tab)
        {
            var previousViewModel = Frame.GetPageViewModel();

            ArgumentNullException.ThrowIfNull(Frame);

            var navigated = Frame.Navigate(tab.TabType, tab);

            if (navigated)
            {
                if (previousViewModel is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

        }
    }
}
