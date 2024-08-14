using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace FileExplorer.UI.Behaviors
{
    public abstract class SelectionChangedBehavior<TElement> : BaseCommandBehavior<TElement> where TElement : Selector
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

        protected virtual void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExecuteIfCan(Command, CommandParameter);
        }
        //{
        //    if (e.AddedItems.Count > 0)
        //    {
        //        ExecuteIfCan(Command, e.AddedItems[0]);
        //    }
        //    //TODO: handle last tab closed...
        //}
    }
}
