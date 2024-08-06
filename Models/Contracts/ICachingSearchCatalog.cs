using Models.General;

namespace Models.Contracts
{
    public interface ICachingSearchCatalog<TElement> : ISearchCatalog<TElement>
    {
        public CachedCatalogSearch<TElement> Cache { get; set; }
    }
}
