#nullable enable
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Helpers.General;
using FileExplorer.Models;
using FileExplorer.Services.Factories;
using FileExplorer.UI.BuildingContextMenu.Contracts;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.UI.BuildingContextMenu
{
    public sealed class BuildingMenuFlyout : MenuFlyout, IBuildingContextMenu<MenuFlyoutItemBase>
    {
        /// <inheritdoc />
        public IMenuFlyoutFactory<MenuFlyoutItemBase> Factory { get; }

        /// <inheritdoc />
        public object? Parameter { get; set; }
        public BuildingMenuFlyout()
        {
            Factory = new MenuFlyoutFactory();
        }

        /// <inheritdoc />
        public void Build(IMenuFlyoutBuilder builder)
        {
            var metadata = builder.BuildMenu(Parameter);
            Build(metadata);
        }

        /// <summary>
        /// Builds menu for a provided metadata (if there is no menu items hides menu flyout)
        /// </summary>
        /// <param name="metadata"> Metadata for menu items </param>
        public void Build(IReadOnlyCollection<MenuFlyoutItemViewModel> metadata)
        {
            Items.Clear();

            if (metadata.Count > 0)
            {
                var menuItems = Factory.Create(metadata);
                Items.AddRange(menuItems);
            }
            else
            {
                Hide();
            }
        }
    }
}
