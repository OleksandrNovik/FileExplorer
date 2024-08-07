#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts;
using System.Collections.Generic;

namespace Models.General;

public sealed class CachedSearchResult<TElement> : ObservableObject
{
    /// <summary>
    /// Cached search result from root catalog 
    /// </summary>
    public ISystemSearchCatalog<TElement> RootCatalog { get; set; }

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

    public CachedSearchResult(ISystemSearchCatalog<TElement> searchCatalog, IEnqueuingCollection<TElement> destination, SearchOptionsModel options)
    {
        RootCatalog = searchCatalog;
        SearchOptions = options;
        SearchResultItems = destination;
    }
}