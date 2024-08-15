using FileExplorer.Core.Contracts.Factories;
using Helpers.General;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    /// <summary>
    /// Behavior that allows menu flyout to build itself depending 
    /// </summary>
    public sealed class MenuFlyoutBuildingBehavior : Behavior<BuildingMenuFlyout>
    {
        /// <summary>
        /// Builder that builds flyout's menu items when needed
        /// </summary>
        public IMenuFlyoutBuilder Builder { get; set; }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Opening += OnOpening;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Opening -= OnOpening;
        }

        /// <summary>
        /// When flyout is opening it receives new items from builder
        /// </summary>
        private void OnOpening(object sender, object e)
        {
            AssociatedObject.Items.Clear();
            AssociatedObject.Items.AddRange(Builder.BuildContextMenu(AssociatedObject.Paramter));
        }
    }
}
