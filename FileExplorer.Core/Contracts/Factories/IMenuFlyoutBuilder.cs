#nullable enable
using Models;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Factories
{
    /// <summary>
    /// Service that builds menu flyout items when needed
    /// </summary>
    public interface IMenuFlyoutBuilder
    {
        /// <summary>
        /// Creates enumeration of menu items metadata 
        /// </summary>
        public IReadOnlyList<MenuFlyoutItemViewModel> BuildMenu(object? parameter);
    }
}
