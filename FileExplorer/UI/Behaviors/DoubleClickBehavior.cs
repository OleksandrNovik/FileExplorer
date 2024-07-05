using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors
{
    public class DoubleClickBehavior : BaseCommandBehavior<UIElement>
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
