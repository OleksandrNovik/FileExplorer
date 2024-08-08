#nullable enable
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services
{
    public class SettingsNavigationViewService : ISettingsNavigationViewService
    {
        private readonly ISettingsNavigationService navigationService;
        private readonly ISettingsPagesService pagesService;
        private NavigationView? navigation;

        public SettingsNavigationViewService(ISettingsNavigationService navigationService, ISettingsPagesService pagesService)
        {
            this.navigationService = navigationService;
            this.pagesService = pagesService;
        }

        public void Initialize(NavigationView navigationView)
        {
            navigation = navigationView;
            navigation.ItemInvoked += OnItemInvoked;
        }

        public void UnregisterEvents()
        {
            if (navigation != null)
            {
                navigation.ItemInvoked -= OnItemInvoked;
            }
        }

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string key)
            {
                navigationService.NavigateTo(key);
            }
        }

        public NavigationViewItem? GetSelectedItem(Type pageType)
        {
            return GetSelectedItem(navigation!.MenuItems, pageType);
        }

        private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }
            }

            return null;
        }
        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            bool isMenuItem = false;

            if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                isMenuItem = pagesService.GetPage(pageKey) == sourcePageType;
            }

            return isMenuItem;
        }
    }
}
