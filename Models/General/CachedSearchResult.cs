#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts;
using Models.Contracts.Storage;
using Models.Storage.Additional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.General;

public sealed class CachedSearchResult<TElement> : ObservableObject, IStorage<TElement>
{
    /// <summary>
    /// Cached search result from root catalog 
    /// </summary>
    public IStorage<TElement> RootCatalog { get; set; }

    /// <summary>
    /// Configurations of search
    /// </summary>
    public SearchFilter SearchOptions { get; set; }

    /// <summary>
    /// Collection of items that were found during the search
    /// </summary>
    public ICollection<TElement> SearchResultItems { get; set; }

    /// <summary>
    /// Is search fully completed (no folders to look)
    /// </summary>
    public bool HasCompleted { get; set; }

    public CachedSearchResult(IStorage<TElement> searchCatalog, IEnqueuingCollection<TElement> destination)
    {
        RootCatalog = searchCatalog;
        SearchResultItems = destination;
    }

    #region Implementation to be a storage

    /// <summary>
    /// Search result always returns name to its  <see cref="RootCatalog"/>
    /// </summary>
    public string Name => RootCatalog.Name;

    /// <summary>
    /// Search result always returns path to its <see cref="RootCatalog"/>
    /// </summary>
    public string Path => RootCatalog.Path;

    /// <summary>
    /// Search result returns parent of <see cref="RootCatalog"/>
    /// </summary>
    public IStorage<TElement>? Parent => RootCatalog.Parent;

    public StorageContentType ContentType => StorageContentType.Files;


    /// <summary>
    /// Enumerates found items in cached search result
    /// </summary>
    public IEnumerable<TElement> EnumerateItems() => SearchResultItems;

    /// <summary>
    /// Returns empty enumeration, because search cannot have subdirectories
    /// </summary>
    public IEnumerable<IStorage<TElement>> EnumerateSubDirectories() => [];

    public async Task SearchAsync(SearchOptions searchOptions)
    {
        await RootCatalog.SearchAsync(searchOptions);
    }

    #endregion

}