using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors.General
{
    /// <summary>
    /// General behavior that runs a command when <see cref="UIElement"/> is clicked
    /// </summary>
    public sealed class ClickBehavior : BaseCommandBehavior<UIElement>
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
            ExecuteIfCan(Command, CommandParameter);
        }
    }
}
