using Microsoft.UI.Xaml;
using Windows.System;

namespace FileExplorer.UI.Behaviors
{
    public class KeyPressedBehavior : BaseCommandBehavior<UIElement>
    {
        public VirtualKey Key { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (Key == e.Key && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}
