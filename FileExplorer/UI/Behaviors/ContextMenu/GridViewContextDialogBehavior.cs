﻿using CommunityToolkit.WinUI.UI;
using FileExplorer.Models.Storage.Abstractions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    public sealed class GridViewContextDialogBehavior : BaseContextMenuBehavior<GridView>
    {
        protected override void OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var tappedElement = e.OriginalSource as FrameworkElement;
            // If grid item was right-clicked
            if (tappedElement?.DataContext is InteractiveStorageItem)
            {
                // ItemProperties is not selected
                if (!AssociatedObject.SelectedItems.Contains(tappedElement.DataContext))
                {
                    // Clear previously selected items
                    AssociatedObject.DeselectAll();
                    // Select item that user right-clicked on
                    AssociatedObject.SelectedItem = tappedElement.DataContext;
                }
            }

            RightClickWithParameter(tappedElement?.DataContext);
        }
    }
}
