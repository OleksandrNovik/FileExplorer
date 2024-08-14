using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.Navigation
{
    /// <summary>
    /// Base navigation behaviour for <see cref="NavigationView"/>
    /// Contains all logic needed to perform navigation using navigation service when navigation view items are invoked
    /// </summary>
    /// <typeparam name="TNavigationParam"> Navigation parameter type </typeparam>
    public abstract class BaseNavigationBehavior<TNavigationParam> : Behavior<NavigationView>
    {
        protected readonly IBasicNavigationService<TNavigationParam> navigationService;

        protected readonly IBasicPageService pageService;

        public BaseNavigationBehavior(IBasicNavigationService<TNavigationParam> navigationService, IBasicPageService pageService)
        {
            this.navigationService = navigationService;
            this.pageService = pageService;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ItemInvoked += OnItemInvoked;
            AssociatedObject.Loaded += OnNavigationViewLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemInvoked -= OnItemInvoked;
            AssociatedObject.Loaded -= OnNavigationViewLoaded;
        }

        protected virtual void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is TNavigationParam key)
            {
                navigationService.NavigateTo(key);
            }
        }

        private void OnNavigationViewLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            AssociatedObject.SelectedItem = AssociatedObject.MenuItems[0];
            navigationService.NavigateTo();
        }
    }
}
