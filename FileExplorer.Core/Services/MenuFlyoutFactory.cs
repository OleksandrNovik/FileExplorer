using FileExplorer.Core.Contracts;
using Helpers.General;
using Microsoft.UI.Xaml.Controls;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Core.Services
{
    public class MenuFlyoutFactory : IMenuFlyoutFactory
    {
        public List<MenuFlyoutItemBase> Create(List<MenuFlyoutItemViewModel> metadata)
        {
            return metadata.Select(CreateSingeItem).ToList();
        }

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
