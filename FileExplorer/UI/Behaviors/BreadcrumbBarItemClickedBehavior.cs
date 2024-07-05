using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors
{
    public class BreadcrumbBarItemClickedBehavior : BaseCommandBehavior<BreadcrumbBar>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ItemClicked += OnItemClick;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemClicked -= OnItemClick;
        }

        private void OnItemClick(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            ExecuteIfCan(Command, args.Index);
        }
    }
}
