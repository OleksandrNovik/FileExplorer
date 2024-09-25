using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors.Text
{
    public sealed class SearchBoxBehavior : BaseCommandBehavior<AutoSuggestBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.QuerySubmitted += OnSubmit;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.QuerySubmitted -= OnSubmit;
        }

        private void OnSubmit(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ExecuteIfCan(Command, CommandParameter);
        }
    }
}
