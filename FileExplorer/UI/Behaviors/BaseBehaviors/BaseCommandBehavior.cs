using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors.BaseBehaviors
{
    /// <summary>
    /// Base class for a behaviours that have to run command on some event
    /// </summary>
    /// <typeparam name="T"> The object type to attach to </typeparam>
    public class BaseCommandBehavior<T> : BaseCanExecuteBehavior<T>
        where T : DependencyObject
    {
        public IRelayCommand Command { get; set; }
        public object CommandParameter { get; set; } = null;
    }
}
