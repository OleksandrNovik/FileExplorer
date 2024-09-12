using FileExplorer.Helpers.General;
using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;
using System.Collections;
using System.Linq;

namespace FileExplorer.UI.Behaviors.Collections
{
    /// <summary>
    /// Behavior to bind list property to a multiple selection items from <see cref="GridView"/>
    /// </summary>
    public sealed class MultiSelectionBehavior : MultipleSelectionBase<GridView>
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
            AssociatedObject.SelectedItems.AddRange(items.OfType<object>());

            bindingSelectedItems = false;
        }
    }
}
