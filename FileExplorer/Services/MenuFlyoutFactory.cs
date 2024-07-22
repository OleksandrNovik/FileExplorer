using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services
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

                result = subItemsMenu;
            }
            else
            {
                result = new MenuFlyoutItem
                {
                    Text = itemsMetadata.Text,
                    Command = itemsMetadata.Command,
                    CommandParameter = itemsMetadata.CommandParameter
                };
            }

            return result;
        }
    }
}
