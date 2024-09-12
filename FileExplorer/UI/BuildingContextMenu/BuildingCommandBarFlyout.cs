#nullable enable
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Helpers.General;
using FileExplorer.Services.Factories;
using FileExplorer.UI.BuildingContextMenu.Contracts;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.UI.BuildingContextMenu
{
    /// <summary>
    /// Command bar flyout that can build its items from provided metadata
    /// </summary>
    public sealed class BuildingCommandBarFlyout : CommandBarFlyout, IBuildingContextMenu<ICommandBarElement>
    {
        /// <inheritdoc />
        public IMenuFlyoutFactory<ICommandBarElement> Factory { get; }

        /// <inheritdoc />
        public object? Parameter { get; set; }

        public BuildingCommandBarFlyout()
        {
            Factory = new CommandBarFactory();
        }

        /// <inheritdoc />
        public void Build(IMenuFlyoutBuilder builder)
        {
            SecondaryCommands.Clear();
            var metadata = builder.BuildMenu(Parameter);
            var menuItems = Factory.Create(metadata);
            SecondaryCommands.AddRange(menuItems);
        }
    }
}
