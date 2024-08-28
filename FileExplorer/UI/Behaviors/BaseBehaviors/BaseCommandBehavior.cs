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
        public static DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(IRelayCommand),
                typeof(BaseCommandBehavior<T>), new PropertyMetadata(null));

        public IRelayCommand Command
        {
            get => GetValue(CommandProperty) as IRelayCommand;
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter { get; set; } = null;
    }
}
