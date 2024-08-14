using Microsoft.UI.Xaml.Controls;
using Models;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Factories
{
    public interface IMenuFlyoutFactory
    {
        public List<MenuFlyoutItemBase> Create(List<MenuFlyoutItemViewModel> metadata);
    }
}
