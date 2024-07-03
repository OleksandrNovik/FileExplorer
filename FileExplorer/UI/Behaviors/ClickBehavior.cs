using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors
{
    public class ClickBehavior : BaseCommandBehavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Tapped += OnClick;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Tapped -= OnClick;
        }
        private void OnClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ExecuteIfCan();
        }
    }
}
