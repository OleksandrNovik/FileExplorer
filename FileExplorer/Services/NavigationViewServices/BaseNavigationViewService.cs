#nullable enable
using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace FileExplorer.Services.NavigationViewServices
{
    /// <summary>
    /// Base class for services that work with navigation views
    /// </summary>
    /// <typeparam name="TNavigationParam"> Navigation parameter for navigation service </typeparam>
    public abstract class BaseNavigationViewService<TNavigationParam> : IBaseNavigationViewService
    {
        protected readonly IBasicNavigationService<TNavigationParam> TabNavigationService;

        protected readonly IBasicPageService pageService;

        protected NavigationView? navigation;

        public BaseNavigationViewService(IBasicNavigationService<TNavigationParam> tabNavigationService, IBasicPageService pageService)
        {
            this.TabNavigationService = tabNavigationService;
            this.pageService = pageService;
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

        public abstract NavigationViewItem? GetSelectedItem(Type pageType);

        protected abstract NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType);

        protected bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            bool isMenuItem = false;

            if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                isMenuItem = pageService.GetPage(pageKey) == sourcePageType;
            }

            return isMenuItem;
        }

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is TNavigationParam key)
            {
                TabNavigationService.NavigateTo(key);
            }
        }
    }
}
