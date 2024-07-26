using Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.UI.Behaviors
{
    public class InitializeWithItemsBehavior : Behavior<MenuFlyoutSubItem>
    {
        public IEnumerable<MenuFlyoutItemViewModel> MenuSubItems { get; set; }

        protected override void OnAttached()
        {
            AssociatedObject.Loading += OnMenuItemLoading;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loading -= OnMenuItemLoading;
            base.OnDetaching();
        }

        private void OnMenuItemLoading(Microsoft.UI.Xaml.FrameworkElement sender, object args)
        {
            AssociatedObject.Items.AddRange(MenuSubItems.Select(menuItems => new MenuFlyoutItem
            {
                Text = menuItems.Text,
                Command = menuItems.Command,
                CommandParameter = menuItems.CommandParameter
            }));
        }
    }
}
