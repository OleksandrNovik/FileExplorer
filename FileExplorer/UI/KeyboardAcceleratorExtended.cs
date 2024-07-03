using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;

namespace FileExplorer.UI
{
    public class KeyboardAcceleratorExtended : KeyboardAccelerator
    {
        public IRelayCommand Command { get; set; }
        public object CommandParameter { get; set; }

        public KeyboardAcceleratorExtended()
        {
            Invoked += OnAcceleratorInvoked;
        }

        private void OnAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
            args.Handled = true;
        }
    }
}
