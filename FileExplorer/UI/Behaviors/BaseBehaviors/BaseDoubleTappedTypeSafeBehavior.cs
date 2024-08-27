using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors.BaseBehaviors
{
    /// <summary>
    /// Base behavior that executes command on some UI element when it's data context has provided type 
    /// </summary>
    /// <typeparam name="TUIElement"> Type of UI element </typeparam>
    /// <typeparam name="TParameter"> Type of parameter that command needs to execute </typeparam>
    public abstract class BaseDoubleTappedTypeSafeBehavior<TUIElement, TParameter> : BaseCommandBehavior<TUIElement>
        where TUIElement : UIElement
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DoubleTapped += OnDoubleClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DoubleTapped -= OnDoubleClick;
        }

        private void OnDoubleClick(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            // If collection element that wa tapped has data context of some IDirectoryItem (type that needed for operation)
            if (e.OriginalSource is FrameworkElement { DataContext: TParameter param })
            {
                ExecuteIfCan(Command, param);
            }
        }
    }
}
