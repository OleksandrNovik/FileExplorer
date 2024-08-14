using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Models.StorageWrappers;

namespace FileExplorer.UI.Behaviors.Navigation
{
    public class NavigationBehavior : BaseNavigationBehavior<string>
    {
        public NavigationBehavior() : base(
            App.GetService<INavigationService>(),
            App.GetService<IPageTypesService>())
        { }

        protected override void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string key)
            {
                if (selectedItem.GetValue(PathIdentifyHelper.PathProperty) is string path)
                {
                    navigationService.NavigateTo(key, new DirectoryWrapper(path));
                }
            }
        }
    }
}
