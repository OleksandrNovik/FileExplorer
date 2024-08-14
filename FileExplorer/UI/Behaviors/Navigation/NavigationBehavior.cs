#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Models.StorageWrappers;

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
        { }

        /// <summary>
        /// Extracts path from <see cref="NavigationViewItem"/> if possible,
        /// which is used to identify directory that we are navigated into
        /// </summary>
        protected override void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            string? key = selectedItem?.GetValue(NavigationHelper.NavigationKeyProperty) as string;

            if (!string.IsNullOrEmpty(key))
            {
                navigationService.NavigateTo(key, new DirectoryWrapper(key));
            }
        }
    }
}
