using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors.General
{
    /// <summary>
    /// General behavior that runs a command when <see cref="UIElement"/> loses focus
    /// </summary>
    public sealed class LostFocusBehavior : BaseCommandBehavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LostFocus += OnLostFocus;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LostFocus -= OnLostFocus;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            ExecuteIfCan(Command, CommandParameter);
        }
    }
}
