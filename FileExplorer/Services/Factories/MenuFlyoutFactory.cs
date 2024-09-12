using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Helpers.General;
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services.Factories
{
    /// <summary>
    /// Factory to create menu items for menu flyout
    /// </summary>
    public sealed class MenuFlyoutFactory : IMenuFlyoutFactory<MenuFlyoutItemBase>
    {
        /// <inheritdoc />
        public IReadOnlyList<MenuFlyoutItemBase> Create(IEnumerable<MenuFlyoutItemViewModel> metadata)
        {
            return metadata.Select(CreateSingeItem).ToList();
        }

        /// <summary>
        /// Creates UI menu element for a single item
        /// </summary>
        /// <param name="itemsMetadata"> Data for UI menu item </param>
        private MenuFlyoutItemBase CreateSingeItem(MenuFlyoutItemViewModel itemsMetadata)
        {
            MenuFlyoutItemBase result;

            if (itemsMetadata.Items is not null)
            {
                var subItemsMenu = new MenuFlyoutSubItem
                {
                    Text = itemsMetadata.Text,
                };

                subItemsMenu.Items.AddRange(itemsMetadata.Items.Select(CreateSingeItem));

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
                var menuItem = new MenuFlyoutItem
                {
                    Text = itemsMetadata.Text,
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
