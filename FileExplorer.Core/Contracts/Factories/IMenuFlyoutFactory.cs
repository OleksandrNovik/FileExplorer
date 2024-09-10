using Models;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Factories
{
    /// <summary>
    /// Factory that creates UI menu items from provided metadata
    /// </summary>
    /// <typeparam name="MenuItem"> Type of menu item </typeparam>
    public interface IMenuFlyoutFactory<MenuItem> where MenuItem : class
    {
        /// <summary>
        /// Creates menu items for context menu
        /// </summary>
        /// <param name="metadata"> Data for items that are created </param>
        /// <returns> List of UI elements that were created </returns>
        public IReadOnlyList<MenuItem> Create(IEnumerable<MenuFlyoutItemViewModel> metadata);
    }
}
