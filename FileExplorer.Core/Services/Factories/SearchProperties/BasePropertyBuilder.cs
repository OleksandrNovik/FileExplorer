using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Factories.SearchProperties;
using FileExplorer.Models;
using FileExplorer.Models.Ranges;
using System.Collections.Generic;

namespace FileExplorer.Core.Services.Factories.SearchProperties
{
    public abstract class BasePropertyBuilder<TProperty> : ISearchPropertyMenuFactory<TProperty>
    {
        public virtual IList<MenuFlyoutItemViewModel> Build(IRelayCommand command)
        {
            var menu = new List<MenuFlyoutItemViewModel>
            {
                new("Any")
                {
                    Command = command,
                    CommandParameter = PredicateChecker<TProperty>.Any
                }
            };

            menu.AddRange(CompleteMenu(command));

            return menu;
        }

        /// <summary>
        /// Completes menu before returning it
        /// </summary>
        protected abstract IEnumerable<MenuFlyoutItemViewModel> CompleteMenu(IRelayCommand command);
    }
}
