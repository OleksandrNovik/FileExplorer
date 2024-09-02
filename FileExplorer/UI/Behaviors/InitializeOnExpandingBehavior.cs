using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors
{
    public sealed class InitializeOnExpandingBehavior : BaseCommandBehavior<Expander>
    {
        private bool hasInitialized;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Expanding += OnExpanding;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Expanding -= OnExpanding;
        }

        private void OnExpanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (!hasInitialized)
            {
                hasInitialized = true;
                ExecuteIfCan(Command, CommandParameter);
            }
        }
    }
}
