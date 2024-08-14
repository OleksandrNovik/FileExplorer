#nullable enable
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Contracts.Settings;
using Microsoft.UI.Xaml.Controls;
using Models.TabRelated;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services.NavigationViewServices
{
    public class NavigationViewService : BaseNavigationViewService<TabModel>
    {
        public NavigationViewService(ITabNavigationService tabNavigationService, IBasicPageService pageService) :
            base(tabNavigationService, pageService)
        {
        }

        public override NavigationViewItem? GetSelectedItem(Type pageType)
        {
            NavigationViewItem? result = null;

            if (navigation != null)
            {
                result = GetSelectedItem(navigation.MenuItems, pageType);
            }

            return result;
        }

        protected override NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            NavigationViewItem? result = null;

            foreach (var item in menuItems.OfType<NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    result = item;
                    break;
                }
                result = GetSelectedItem(item.MenuItems, pageType);
            }

            return result;
        }
    }
}
