using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors.Navigation
{
    /// <summary>
    /// Behavior to initiate navigation in settings modal window 
    /// </summary>
    public class SettingsNavigationBehavior : BaseNavigationBehavior<string>
    {
        public SettingsNavigationBehavior() : base(
            App.GetService<ISettingsNavigationService>(),
            App.GetService<IBasicPageService<string>>())
        { }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnNavigationViewLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnNavigationViewLoaded;
        }

        /// <summary>
        /// When <see cref="NavigationView"/> is loaded selects first element of <see cref="NavigationView"/> items
        /// </summary>
        protected void OnNavigationViewLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.SelectedItem = AssociatedObject.MenuItems[0];
            navigationService.NavigateTo();
        }
    }
}
