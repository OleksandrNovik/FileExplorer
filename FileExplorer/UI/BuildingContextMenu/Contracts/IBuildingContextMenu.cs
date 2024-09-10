#nullable enable
using FileExplorer.Core.Contracts.Factories;

namespace FileExplorer.UI.BuildingContextMenu.Contracts
{
    /// <summary>
    /// Basic interface that
    /// </summary>
    public interface IBuildingContextMenu
    {
        /// <summary>
        /// Parameter for menu items builder
        /// </summary>
        public object? Parameter { get; set; }

        /// <summary>
        /// Builds menu items using provided builder and parameter
        /// </summary>
        /// <param name="builder"> Builder that can provide metadata for menu </param>
        public void Build(IMenuFlyoutBuilder builder);
    }

    /// <summary>
    /// Contract for context layouts to build inner menu items
    /// </summary>
    /// <typeparam name="MemuItem"> Base type of menu items </typeparam>
    public interface IBuildingContextMenu<MemuItem> : IBuildingContextMenu where MemuItem : class
    {
        /// <summary>
        /// Factory to create items from metadata 
        /// </summary>
        public IMenuFlyoutFactory<MemuItem> Factory { get; }
    }
}
