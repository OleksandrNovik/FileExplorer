#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;

namespace FileExplorer.UI.Behaviors.BaseBehaviors
{
    public abstract class BaseCanExecuteBehavior<T> : Behavior<T>
        where T : DependencyObject
    {
        protected void ExecuteIfCan(ICommand command, object? commandParameter)
        {
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
    }
}
