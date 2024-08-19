#nullable enable
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileExplorer.UI.Behaviors.BaseBehaviors
{
    public abstract class BaseCanExecuteBehavior<T> : Behavior<T>
        where T : DependencyObject
    {
        protected async Task ExecuteAsync(IAsyncRelayCommand command, object? commandParameter)
        {
            await command.ExecuteAsync(commandParameter);
        }

        protected void ExecuteIfCan(ICommand command, object? commandParameter)
        {
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
    }
}
