using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace FileExplorer.UI.Behaviors.Tabs
{
    /// <summary>
    /// Behavior to open new tab when add button is pressed
    /// Loads tabs when app is started (tab view is loaded on the screen)
    /// </summary>
    public sealed class TabOpenedBehavior : BaseCommandBehavior<TabView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddTabButtonClick += OnAddButtonClicked;
            AssociatedObject.Loaded += OnTabViewLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AddTabButtonClick -= OnAddButtonClicked;
            AssociatedObject.Loaded -= OnTabViewLoaded;
        }

        /// <summary>
        /// When add button is pressed we create new default tab and navigate into this tab
        /// (so we should select it on UI too)
        /// </summary>
        private void OnAddButtonClicked(TabView sender, object args)
        {
            ExecuteIfCan(Command, CommandParameter);
            AssociatedObject.SelectedItem = AssociatedObject.TabItems.Last();
        }

        private void OnTabViewLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Need to restore tabs from file if user selected corresponding option
            ExecuteIfCan(Command, CommandParameter);
        }
    }
}
