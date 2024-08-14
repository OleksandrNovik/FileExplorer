#nullable enable
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
        public static string? GetNavigationKey(NavigationViewItem item) => item.GetValue(NavigationKeyProperty) as string;

        public static void SetNavigationKey(NavigationViewItem item, string? value) => item.SetValue(NavigationKeyProperty, value);

        public static readonly DependencyProperty NavigationKeyProperty =
            DependencyProperty.RegisterAttached("NavigationKey", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
    }
}
