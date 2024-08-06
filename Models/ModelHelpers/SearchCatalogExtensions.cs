using Models.Contracts;
using Models.General;
using System.Collections.Generic;
using System.Threading;
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
        /// <param name="destination"> Destination collection for catalogs to fill with found values </param>
        /// <param name="options"> Search options </param>
        /// <param name="token"> Cancellation token  </param>
        public static async Task SearchCatalogsAsync<TElement>(
            this IEnumerable<ISearchCatalog<TElement>> items,
            IEnqueuingCollection<TElement> destination,
            SearchOptionsModel options,
            CancellationToken token)
        {
            var parallelOption = new ParallelOptions
            {
                MaxDegreeOfParallelism = 1,
                CancellationToken = token
            };
            await Parallel.ForEachAsync(items, parallelOption,
                async (subdirectory, ct) =>
                {
                    await subdirectory.SearchAsync(destination, options, ct);
                });
        }
    }
}
