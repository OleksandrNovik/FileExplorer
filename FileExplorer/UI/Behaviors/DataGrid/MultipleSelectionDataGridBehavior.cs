using FileExplorer.UI.Behaviors.BaseBehaviors;
using Helpers.General;
using Microsoft.UI.Xaml.Controls;
using System.Collections;

namespace FileExplorer.UI.Behaviors.DataGrid
{
    public sealed class
        MultipleSelectionDataGridBehavior : MultipleSelectionBase<CommunityToolkit.WinUI.UI.Controls.DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        protected override void SelectItemInCollection(IList items)
        {
            AssociatedObject.SelectedItems.AddRange(items);
        }

        protected override void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bindingSelectedItems)
            {
                SelectedItems.RemoveRange(e.RemovedItems);
                SelectedItems.AddRange(e.AddedItems);
            }
            else
            {
                bindingSelectedItems = false;
            }
        }
    }
}
