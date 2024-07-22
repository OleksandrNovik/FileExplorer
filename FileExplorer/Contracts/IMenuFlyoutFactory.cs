using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.Contracts
{
    public interface IMenuFlyoutFactory
    {
        public List<MenuFlyoutItemBase> Create(List<MenuFlyoutItemViewModel> metadata);
    }
}
