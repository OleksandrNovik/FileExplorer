﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System.Collections;

namespace FileExplorer.UI.Behaviors
{
    /// <summary>
    /// Behaviour to bind list property to a multiple selection items from <see cref="GridView"/>
    /// </summary>
    public class MultiSelectionBehavior : Behavior<GridView>
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
            foreach (var item in e.RemovedItems)
            {
                SelectedItems.Remove(item);
            }

            foreach (var item in e.AddedItems)
            {
                SelectedItems.Add(item);
            }
        }
    }
}