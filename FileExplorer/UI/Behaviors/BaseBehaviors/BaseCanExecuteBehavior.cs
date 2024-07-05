#nullable enable
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.BaseBehaviors
{
    public abstract class BaseCanExecuteBehavior<T> : Behavior<T>
        where T : DependencyObject
    {
        protected void ExecuteIfCan(IRelayCommand command, object? commandParameter)
        {
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
    }
}
