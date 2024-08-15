#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    /// <summary>
    /// Implements generic context flyout behavior for any <see cref="FrameworkElement"/>
    /// </summary>
    public sealed class BuildingContextMenuBehavior : BaseContextMenuBehavior<FrameworkElement>
    {
        public object? Parameter { get; set; }

        protected override void OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            RightClickWithParameter(Parameter);
        }
    }
}
