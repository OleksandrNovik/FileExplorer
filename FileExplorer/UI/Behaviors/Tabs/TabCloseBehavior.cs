using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors.Tabs
{
    /// <summary>
    /// Behavior to execute command when user closes tab in <see cref="TabView"/>
    /// </summary>
    public sealed class TabCloseBehavior : BaseCommandBehavior<TabView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.TabCloseRequested += OnCloseButtonClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TabCloseRequested -= OnCloseButtonClick;
        }

        /// <summary>
        /// When close button is pressed command is executed with closed item as parameter
        /// </summary>
        private void OnCloseButtonClick(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            ExecuteIfCan(Command, args.Item);
        }
    }
}
