using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using Windows.System;

namespace FileExplorer.UI.Behaviors
{
    public class KeyPressedBehavior : Behavior<UIElement>
    {
        public VirtualKey Key { get; set; }
        public IRelayCommand Command { get; set; }
        public object CommandParameter { get; set; } = null;

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
