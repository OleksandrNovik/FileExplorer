using FileExplorer.Core.Contracts.General;
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
        /// <summary>
        /// Navigation service to initiate navigation
        /// </summary>
        protected readonly IBasicNavigationService<TNavigationParam> navigationService;

        /// <summary>
        /// Page service to get page type from key before navigation 
        /// </summary>
        protected readonly IBasicPageService<TNavigationParam> pageService;

        public BaseNavigationBehavior(IBasicNavigationService<TNavigationParam> navigationService,
            IBasicPageService<TNavigationParam> pageService)
        {
            this.navigationService = navigationService;
            this.pageService = pageService;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ItemInvoked += OnItemInvoked;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemInvoked -= OnItemInvoked;
        }

        /// <summary>
        /// Basic version of ItemInvoked event handler
        /// </summary>
        protected virtual void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigationKeyProperty) is TNavigationParam key)
            {
                navigationService.NavigateTo(key);
            }
        }



    }
}
