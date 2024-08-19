using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml;

namespace FileExplorer.UI.Behaviors.General
{
    /// <summary>
    /// Behaviour that executes a command when associated object is loading
    /// </summary>
    public sealed class LoadingCommandBehavior : BaseCommandBehavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loading += OnElementLoading;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loading -= OnElementLoading;
        }

        private void OnElementLoading(FrameworkElement sender, object args)
        {
            ExecuteIfCan(Command, CommandParameter);
        }
    }
}
