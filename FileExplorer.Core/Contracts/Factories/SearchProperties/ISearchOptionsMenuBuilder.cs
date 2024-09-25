using CommunityToolkit.Mvvm.Input;
using FileExplorer.Models;
using System;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Factories.SearchProperties
{
    /// <summary>
    /// Contract for a class that builds menu items for search filter flyout
    /// </summary>
    public interface ISearchOptionsMenuBuilder
    {
        /// <summary>
        /// Builds menu for a search property of provided type 
        /// </summary>
        /// <typeparam name="TProperty"> Type of search property to build menu </typeparam>
        /// <param name="command"> Command to save search property </param>
        /// <returns> Enumeration of menu items for current search property </returns>
        public IEnumerable<MenuFlyoutItemViewModel> Build<TProperty>(IRelayCommand command)
            where TProperty : IComparable<TProperty>;

    }
}
