using FileExplorer.Core.Contracts.Factories;
using FileExplorer.UI.BuildingContextMenu.Contracts;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.ContextMenu
{
    /// <summary>
    /// Behavior that builds context menu right when it is opening 
    /// </summary>
    public sealed class ContextMenuBuildingBehavior : Behavior<FlyoutBase>
    {
        /// <summary>
        /// Builder that provides context menu with metadata for its items
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
        /// Requires build from building context menu when it is opened
        /// </summary>
        private void OnOpening(object sender, object e)
        {
            if (AssociatedObject is IBuildingContextMenu buildingContext)
            {
                buildingContext.Build(Builder);
            }
        }
    }
}
