using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors
{
    public class LostFocusBehavior : BaseCommandBehavior<UIElement>
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
            ExecuteIfCan();
        }
    }
}
