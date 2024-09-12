using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Models;
using FileExplorer.UI.BuildingContextMenu;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services.Factories
{
    /// <summary>
    /// Factory to create menu items for command bar element
    /// </summary>
    public sealed class CommandBarFactory : IMenuFlyoutFactory<ICommandBarElement>
    {
        /// <inheritdoc />
        public IReadOnlyList<ICommandBarElement> Create(IEnumerable<MenuFlyoutItemViewModel> metadata)
        {
            return metadata.Select(CreateSingeItem).ToList();
        }

        /// <summary>
        /// Creates UI menu element for a single item
        /// </summary>
        /// <param name="itemsMetadata"> Data for UI menu item </param>
        private ICommandBarElement CreateSingeItem(MenuFlyoutItemViewModel itemsMetadata)
        {
            ICommandBarElement result;

            if (itemsMetadata.Items is not null)
            {
                var subItemsMenu = new AppBarButton
                {
                    Label = itemsMetadata.Text,
                };

                var flyout = new BuildingMenuFlyout();
                flyout.Build(itemsMetadata.Items);
                subItemsMenu.Flyout = flyout;

                if (itemsMetadata.IconGlyph is not null)
                {
                    subItemsMenu.Icon = new FontIcon
                    {
                        Glyph = itemsMetadata.IconGlyph,
                    };
                }

                result = subItemsMenu;
            }
            else
            {
                var menuItem = new AppBarButton
                {
                    Label = itemsMetadata.Text,
                    Command = itemsMetadata.Command,
                    CommandParameter = itemsMetadata.CommandParameter
                };

                if (itemsMetadata.IconGlyph is not null)
                {
                    menuItem.Icon = new FontIcon
                    {
                        Glyph = itemsMetadata.IconGlyph,
                    };
                }

                result = menuItem;
            }

            return result;
        }
    }
}
