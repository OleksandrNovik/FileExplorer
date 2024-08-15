#nullable enable
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    /// <summary>
    /// Implements generic context flyout behavior for any <see cref="NavigationViewItem"/>
    /// </summary>
    public sealed class NavigationViewItemContextMenuBehavior : BaseContextMenuBehavior<NavigationViewItem>
    {
        /// <summary>
        /// Parameter which is used to build menu flyout
        /// </summary>
        public object? Parameter { get; set; }

        protected override void OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            // If current item has subitems we skip it and handle right click of subitem
            if (AssociatedObject.MenuItemsSource is not IList { Count: > 0 })
            {
                RightClickWithParameter(Parameter);
            }

            e.Handled = true;
        }
    }
}
