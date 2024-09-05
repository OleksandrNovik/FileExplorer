using Helpers.General;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System.Collections;

namespace FileExplorer.UI.Behaviors.BaseBehaviors
{
    public abstract class MultipleSelectionBase<TCollection> : Behavior<TCollection>
        where TCollection : UIElement
    {
        protected bool bindingSelectedItems;

        /// <summary>
        /// Dependency property to check whenever selected items have changed
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList),
                typeof(MultipleSelectionBase<TCollection>), new PropertyMetadata(null, OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultipleSelectionBase<TCollection> behavior && e.NewValue is IList { Count: > 0 } list)
            {
                behavior.bindingSelectedItems = true;
                behavior.SelectItemInCollection(list);
            }
        }

        protected abstract void SelectItemInCollection(IList items);

        /// <summary>
        /// Selected items property that used to bind list item from ViewModel
        /// </summary>
        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Method that takes care of removing and adding new selected items for binded collection
        /// </summary>
        protected virtual void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bindingSelectedItems)
            {
                SelectedItems.RemoveRange(e.RemovedItems);
                SelectedItems.AddRange(e.AddedItems);
            }
        }

    }
}
