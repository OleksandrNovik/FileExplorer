using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    public sealed class DataGridContextDialogBehavior : BaseContextMenuBehavior<CommunityToolkit.WinUI.UI.Controls.DataGrid>
    {
        protected override void OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var tappedElement = e.OriginalSource as FrameworkElement;

            // If grid item was right-clicked
            if (tappedElement?.DataContext is not null)
            {
                // Item is not selected
                if (!AssociatedObject.SelectedItems.Contains(tappedElement.DataContext))
                {
                    // Clear previously selected items
                    AssociatedObject.SelectedItems.Clear();
                    // Select item that user right-clicked on
                    AssociatedObject.SelectedItem = tappedElement.DataContext;
                }
            }

            RightClickWithParameter(tappedElement?.DataContext);
        }
    }
}
