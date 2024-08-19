#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Models.Storage.Windows;
using Models.TabRelated;
using System.Collections;

namespace FileExplorer.UI.Behaviors.Navigation
{
    /// <summary>
    /// Behavior that used to initiate navigation in left pane of main window
    /// </summary>
    public class NavigationBehavior : BaseNavigationBehavior<string>
    {
        public NavigationBehavior() : base(
            App.GetService<INavigationService>(),
            App.GetService<IPageTypesService>())
        {
            if (navigationService is INavigationService eventProvider)
            {
                eventProvider.TabOpened += OnTabOpened;
            }
        }

        /// <summary>
        /// Extracts path from <see cref="NavigationViewItem"/> if possible,
        /// which is used to identify directory that we are navigated into
        /// </summary>
        protected override void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigationKeyProperty) is string key)
            {
                if (key != string.Empty)
                {
                    navigationService.NavigateTo(key, new DirectoryWrapper(key));
                }
                else
                {
                    navigationService.NavigateTo(key);
                }
            }
        }

        private void OnTabOpened(object? sender, TabModel openTab)
        {
            AssociatedObject.SelectedItem = openTab.Selected;

            if (openTab.Selected is null && AssociatedObject.MenuItemsSource is IList source)
            {
                AssociatedObject.SelectedItem = source[0];
            }
        }
    }
}
