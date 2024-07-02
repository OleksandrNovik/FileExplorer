using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors
{
    /// <summary>
    /// Base class for a behaviours that have to run command on some event
    /// </summary>
    /// <typeparam name="T"> The object type to attach to </typeparam>
    public class BaseCommandBehavior<T> : Behavior<T> where T : DependencyObject
    {
        public IRelayCommand Command { get; set; }
        public object CommandParameter { get; set; } = null;

        /// <summary>
        /// Executes <see cref="Command"/> (if it can be executed) with provided <see cref="CommandParameter"/>
        /// </summary>
        protected void ExecuteIfCan()
        {
            if (Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}
