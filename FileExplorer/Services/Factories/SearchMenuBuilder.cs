using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories.SearchProperties;
using FileExplorer.Models;
using System;
using System.Collections.Generic;

namespace FileExplorer.Services.Factories
{
    /// <summary>
    /// Service that uses a factory to build menu for certain search option
    /// </summary>
    public sealed class SearchMenuBuilder : ISearchOptionsMenuBuilder
    {
        /// <inheritdoc />
        public IEnumerable<MenuFlyoutItemViewModel> Build<TProperty>(IRelayCommand command) where TProperty : IComparable<TProperty>
        {
            var creator = App.GetService<ISearchPropertyMenuFactory<TProperty>>();

            return creator.Build(command);
        }
    }
}
