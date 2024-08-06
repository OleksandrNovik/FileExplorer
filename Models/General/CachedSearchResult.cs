#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts;
using System.Collections.Generic;

namespace Models.General;

public sealed partial class CachedSearchResult<TElement> : ObservableObject
{
    /// <summary>
    /// Cached search result from root catalog 
    /// </summary>
    public CachedCatalogSearch<TElement> RootCatalog { get; set; }

    /// <summary>
    /// Configurations of search
    /// </summary>
    public SearchOptionsModel SearchOptions { get; set; }

    /// <summary>
    /// Collection of items that were found during the search
    /// </summary>
    public ICollection<TElement> SearchResultItems { get; set; }

    /// <summary>
    /// Is search fully completed (no folders to look)
    /// </summary>
    public bool HasCompleted { get; set; }

    /// <summary>
    /// Is search in progress (this property can be used to see if search is running right now)
    /// </summary>
    [ObservableProperty]
    private bool inProgress;

    public CachedSearchResult(ISystemSearchCatalog<TElement> searchCatalog, IEnqueuingCollection<TElement> destination, SearchOptionsModel options)
    {
        RootCatalog = new CachedCatalogSearch<TElement>(searchCatalog);
        SearchOptions = options;
        SearchResultItems = destination;
    }
}