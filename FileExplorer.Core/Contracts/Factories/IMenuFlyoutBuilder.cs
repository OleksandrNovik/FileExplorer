#nullable enable
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Factories
{
    /// <summary>
    /// Service that builds menu flyout items when needed
    /// </summary>
    public interface IMenuFlyoutBuilder
    {
        /// <summary>
        /// Creates enumeration of menu flyout items
        /// </summary>
        public IList<MenuFlyoutItemBase> BuildContextMenu(object? parameter = null);
    }
}
