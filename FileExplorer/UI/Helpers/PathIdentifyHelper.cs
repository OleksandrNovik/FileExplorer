using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Helpers
{
    /// <summary>
    /// Helper class that holds directory path that is used to navigate to a certain directory when <see cref="NavigationViewItem"/> is invoked
    /// </summary>
    public class PathIdentifyHelper
    {
        public static string GetPath(NavigationViewItem item) => (string)item.GetValue(PathProperty);

        public static void SetPath(NavigationViewItem item, string value) => item.SetValue(PathProperty, value);

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.RegisterAttached("DirectoryPath", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
    }
}
