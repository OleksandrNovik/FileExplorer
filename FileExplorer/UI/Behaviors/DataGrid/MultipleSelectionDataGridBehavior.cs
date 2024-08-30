using Helpers.General;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System.Collections;

namespace FileExplorer.UI.Behaviors.DataGrid
{
    public sealed class MultipleSelectionDataGridBehavior : Behavior<CommunityToolkit.WinUI.UI.Controls.DataGrid>
    {
        /// <summary>
        /// Selected items property that used to bind list item from ViewModel
        /// </summary>
        public IList SelectedItems { get; set; }

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

        /// <summary>
        /// Method that takes care of removing and adding new selected items from <see cref="GridView"/>
        /// </summary>
        /// <param name="sender"> Selection event sender (GridView) </param>
        /// <param name="e"> Event argument that stores all information about selection </param>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems.RemoveRange(e.RemovedItems);
            SelectedItems.AddRange(e.AddedItems);
        }
    }
}
