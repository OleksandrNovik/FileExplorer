using Models.Contracts;
using Models.Storage.Additional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.ModelHelpers
{
    public static class SearchCatalogExtensions
    {
        /// <summary>
        /// Searches with multiple threads through all provided catalogs
        /// </summary>
        /// <typeparam name="TElement"> Type of element of destination collection <see cref="IEnqueuingCollection{T}"/> </typeparam>
        /// <param name="items"> Catalogs that are searched </param>
        /// <param name="searchOptions"> Provided options for this search </param>
        public static async Task SearchCatalogsAsync<TElement>(
            this IEnumerable<ISearchCatalog<TElement>> items,
            SearchOptions searchOptions)
        {
            if (searchOptions.OptimizationsEnabled)
            {
                await items.SearchEachCatalogAsync(searchOptions);
            }
            else
            {
                await Task.Run(async () =>
                {
                    await items.SearchEachCatalogAsync(searchOptions);

                }, searchOptions.Token);
            }
        }

        private static async Task SearchEachCatalogAsync<TElement>(
            this IEnumerable<ISearchCatalog<TElement>> items,
            SearchOptions searchOptions)
        {
            foreach (var subdirectory in items)
            {
                await subdirectory.SearchAsync(searchOptions);
            }
        }

    }
}
