#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts.Factories;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.ViewModels.Abstractions
{
    public abstract class ContextMenuCreatorViewModel : ObservableRecipient, IMenuFlyoutBuilder
    {
        /// <summary>
        /// Factory to create right-click menu flyout for any item in Directory or for a Directory itself
        /// </summary>
        protected readonly IMenuFlyoutFactory menuFactory;

        public ContextMenuCreatorViewModel(IMenuFlyoutFactory factory)
        {
            menuFactory = factory;
        }
        public abstract IList<MenuFlyoutItemBase> BuildContextMenu(object? parameter = null);
    }
}
