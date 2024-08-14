using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors.Tabs
{
    /// <summary>
    /// Behavior that controls selecting in <see cref="TabView"/>
    /// </summary>
    public sealed class TabSelectionChangedBehavior : BaseCommandBehavior<TabView>
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

        /// <summary>
        /// Selects tab when it is pressed, if there is no tabs left closes window
        /// </summary>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ExecuteIfCan(Command, e.AddedItems[0]);
            }
            //TODO: handle last tab closed...
        }
    }
}
