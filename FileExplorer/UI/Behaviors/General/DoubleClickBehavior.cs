using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors.General
{
    /// <summary>
    /// General behavior that runs a command when <see cref="UIElement"/> is double-clicked
    /// </summary>
    public sealed class DoubleClickBehavior : BaseCommandBehavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DoubleTapped += OnDoubleTapped;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DoubleTapped -= OnDoubleTapped;
        }
        private void OnDoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ExecuteIfCan(Command, CommandParameter);
        }
    }
}
