using CommunityToolkit.Mvvm.Input;
using FileExplorer.Models;
using System;
using System.Collections.Generic;

namespace FileExplorer.Core.Contracts.Factories.SearchProperties
{
    /// <summary>
    /// Contract for a factory that creates menu items for certain search menu option
    /// </summary>
    /// <typeparam name="TProperty"> Option type </typeparam>
    public interface ISearchPropertyMenuFactory<TProperty>
        where TProperty : IComparable<TProperty>
    {
        /// <summary>
        /// Builds menu for a certain search option property 
        /// </summary>
        /// <param name="command"> Command to store search option </param>
        /// <returns> Menu items for menu </returns>
        public IList<MenuFlyoutItemViewModel> Build(IRelayCommand command);
    }
}
