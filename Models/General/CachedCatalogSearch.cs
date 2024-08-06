#nullable enable
using Models.Contracts;
using Models.ModelHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Models.General
{
    public sealed class CachedCatalogSearch<TElement> : ISearchCatalog<TElement>
    {
        public CachedCatalogSearch(ISystemSearchCatalog<TElement> cachedCatalog)
        {
            SearchCatalog = cachedCatalog;
            SearchCatalog.Cache = this;
        }

        /// <summary>
        /// Catalog whose search result is saved in this cache object
        /// </summary>
        public ISystemSearchCatalog<TElement> SearchCatalog { get; }

        /// <summary>
        /// Cached data of catalogs subdirectories or sub catalogs
        /// </summary>
        public ICollection<CachedCatalogSearch<TElement>> CachedSubCatalogs { get; set; }

        /// <summary>
        /// Has searched been performed in the catalog
        /// </summary>
        public bool HasSearched { get; set; }

        public async Task SearchAsync(IEnqueuingCollection<TElement> destination, SearchOptionsModel options, CancellationToken token)
        {
            if (!HasSearched)
            {
                await SearchCatalog.SearchAsync(destination, options, token);
            }

            if (options.IsNestedSearch)
            {
                await CachedSubCatalogs.Where(catalog => !catalog.HasSearched).SearchCatalogsAsync(destination, options, token);
            }
        }
    }
}
