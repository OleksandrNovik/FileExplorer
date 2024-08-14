using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Helpers
{
    /// <summary>
    /// Helper class to create NavigateTo property for a <see cref="NavigationViewItem"/>
    /// that is used to identify page type when item is invoked
    /// </summary>
    public class NavigationHelper
    {
        public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

        public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
    }
}
