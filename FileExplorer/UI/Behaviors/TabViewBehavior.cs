using CommunityToolkit.Mvvm.Input;
using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors
{
    public class TabViewBehavior : BaseCanExecuteBehavior<TabView>
    {
        public IRelayCommand OpenTabCommand { get; set; }
        public IRelayCommand CloseTabCommand { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddTabButtonClick += OnAddButtonClick;
            AssociatedObject.TabCloseRequested += OnCloseButtonClick;
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AddTabButtonClick -= OnAddButtonClick;
            AssociatedObject.TabCloseRequested -= OnCloseButtonClick;
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnAddButtonClick(TabView sender, object args)
        {
            ExecuteIfCan(OpenTabCommand, null);
        }

        private void OnCloseButtonClick(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            ExecuteIfCan(CloseTabCommand, args.Item);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
