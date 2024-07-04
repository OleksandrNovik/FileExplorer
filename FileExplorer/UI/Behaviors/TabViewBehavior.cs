using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors
{
    public class TabViewBehavior : Behavior<TabView>
    {
        public IRelayCommand OpenTabCommand { get; set; }
        public IRelayCommand CloseTabCommand { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddTabButtonClick += OnAddButtonClick;
            AssociatedObject.TabCloseRequested += OnCloseButtonClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AddTabButtonClick -= OnAddButtonClick;
            AssociatedObject.TabCloseRequested -= OnCloseButtonClick;
        }

        private void OnAddButtonClick(TabView sender, object args)
        {
            if (OpenTabCommand.CanExecute(null))
            {
                OpenTabCommand.Execute(null);
            }
        }

        private void OnCloseButtonClick(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            var removedIndex = sender.TabItems.IndexOf(args.Item);
            if (CloseTabCommand.CanExecute(removedIndex))
            {
                CloseTabCommand.Execute(removedIndex);
            }
        }


    }
}
