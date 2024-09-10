#nullable enable
using FileExplorer.UI.BuildingContextMenu.Contracts;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    /// <summary>
    /// Basic behavior that shows self-building menu flyout with parameter (by default DataContext of element)
    /// </summary>
    /// <typeparam name="TElement"> Type of associated object</typeparam>
    public abstract class BaseContextMenuBehavior<TElement> : Behavior<TElement>
        where TElement : FrameworkElement
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.RightTapped += OnRightTapped;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RightTapped -= OnRightTapped;
        }

        /// <summary>
        /// When associated object is right tapped context menu is shown with parameter that is DataContext of associated object
        /// </summary>
        protected virtual void OnRightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            RightClickWithParameter(AssociatedObject.DataContext);
        }

        /// <summary>
        /// Uses context menu ability to store parameter, which is used to build it when it is opening
        /// </summary>
        /// <param name="parameter"> Provided parameter for building </param>
        protected void RightClickWithParameter(object? parameter)
        {
            if (AssociatedObject.ContextFlyout is IBuildingContextMenu buildingFlyout)
            {
                buildingFlyout.Parameter = parameter;
            }
        }
    }
}
