#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts.Factories;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.ViewModels.General
{
    public abstract class ContextMenuCreatorViewModel : ObservableRecipient, IMenuFlyoutBuilder
    {
        /// <summary>
        /// Factory to create right-click menu flyout for any item in directory or for a directory itself
        /// </summary>
        protected readonly IMenuFlyoutFactory menuFactory;

        public ContextMenuCreatorViewModel(IMenuFlyoutFactory factory)
        {
            menuFactory = factory;
        }
        public abstract IEnumerable<MenuFlyoutItemBase> BuildContextMenu(object? parameter = null);
    }
}
