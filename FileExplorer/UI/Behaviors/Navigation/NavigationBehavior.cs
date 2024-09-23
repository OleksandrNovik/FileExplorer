#nullable enable
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Models.Enums;
using FileExplorer.Models.TabRelated;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using System.Collections;

namespace FileExplorer.UI.Behaviors.Navigation
{
    /// <summary>
    /// Behavior that used to initiate navigation in left pane of main window
    /// </summary>
    public class NavigationBehavior : BaseNavigationBehavior<StorageContentType>
    {
        /// <summary>
        /// Command that is executed when navigation item is invoked
        /// </summary>
        public IRelayCommand? ItemInvokedCommand { get; set; }

        /// <summary>
        /// Storage factory to create storage from navigation path
        /// </summary>
        private readonly IStorageFactory factory;
        public NavigationBehavior() : base(
            App.GetService<INavigationService>(),
            App.GetService<IPageTypesService>())
        {
            if (navigationService is INavigationService eventProvider)
            {
                eventProvider.TabOpened += OnTabOpened;
            }

            factory = App.GetService<IStorageFactory>();
        }

        /// <summary>
        /// Extracts path from <see cref="NavigationViewItem"/> if possible,
        /// which is used to identify Directory that we are navigated into
        /// </summary>
        protected override void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigationKeyProperty) is string key)
            {
                var selectedStorage = factory.CreateFromKey(key);

                if (ItemInvokedCommand is not null)
                {
                    ItemInvokedCommand.Execute(selectedStorage);
                }

                navigationService.NavigateTo(selectedStorage.ContentType, selectedStorage);
            }
        }

        /// <summary>
        /// Selects item in navigation menu when tab is opened
        /// </summary>
        private void OnTabOpened(object? sender, TabModel openTab)
        {
            AssociatedObject.SelectedItem = openTab.Selected;

            // If selected item is not in menu, selects first menu item (home)
            if (openTab.Selected is null && AssociatedObject.MenuItemsSource is IList source)
            {
                AssociatedObject.SelectedItem = source[0];
            }
        }
    }
}
