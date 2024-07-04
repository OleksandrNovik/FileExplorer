using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;

namespace FileExplorer.UI
{
    public class CommandKeyboardAccelerator : KeyboardAccelerator
    {
        public IRelayCommand Command { get; set; }
        public object CommandParameter { get; set; }

        public CommandKeyboardAccelerator()
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
