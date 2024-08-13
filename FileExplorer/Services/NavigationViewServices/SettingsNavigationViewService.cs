#nullable enable
using FileExplorer.Core.Contracts.Settings;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services.NavigationViewServices
{
    public sealed class SettingsNavigationViewService : BaseNavigationViewService<string>
    {
        public SettingsNavigationViewService(ISettingsNavigationService navigationService,
            IBasicPageService pagesService) :
            base(navigationService, pagesService)
        {
        }

        public override NavigationViewItem? GetSelectedItem(Type pageType)
        {
            NavigationViewItem? navigationItem = null;

            if (navigation is not null)
            {
                navigationItem = GetSelectedItem(navigation.MenuItems, pageType) ??
                                 GetSelectedItem(navigation.FooterMenuItems, pageType);
            }

            return navigationItem;
        }

        protected override NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
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
    }
}
