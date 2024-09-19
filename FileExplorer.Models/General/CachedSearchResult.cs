#nullable enable
using FileExplorer.Models.Contracts;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Enums;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Additional;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileExplorer.Models.General;

/// <summary>
/// Class that represents search result and contains all necessary information about search result itself (including found items)
/// </summary>
public sealed class CachedSearchResult : IStorage
{
    /// <summary>
    /// Cached search result from root catalog 
    /// </summary>
    public IStorage RootCatalog { get; set; }

    /// <summary>
    /// Configurations of search
    /// </summary>
    public SearchFilter Filter { get; set; }

    /// <summary>
    /// Collection of items that were found during the search
    /// </summary>
    public ICollection<IDirectoryItem> SearchResultItems { get; set; }

    /// <summary>
    /// Is search fully completed
    /// </summary>
    public bool HasCompleted { get; set; }

    public CachedSearchResult(IStorage searchCatalog, IEnqueuingCollection<IDirectoryItem> destination)
    {
        RootCatalog = searchCatalog;
        SearchResultItems = destination;
    }

    #region Implementation to be a storage

    /// <summary>
    /// Search result always returns name to its <see cref="RootCatalog"/>
    /// </summary>
    public string Name => RootCatalog.Name;

    /// <summary>
    /// Search result always returns path to its <see cref="RootCatalog"/>
    /// </summary>
    public string Path => RootCatalog.Path;

    /// <summary>
    /// Search result returns parent of <see cref="RootCatalog"/>
    /// </summary>
    public IStorage? Parent => RootCatalog.Parent;

    /// <inheritdoc />
    public StorageContentType ContentType => StorageContentType.Files;

    /// <summary>
    /// Enumerates found items in cached search result
    /// </summary>
    public IEnumerable<IDirectoryItem> EnumerateItems(FileAttributes rejectedAttributes = 0)
    {
        foreach (var item in SearchResultItems)
        {
            if (item.HasAttributes(rejectedAttributes))
                continue;

            yield return item;
        }
    }

    /// <summary>
    /// Returns empty enumeration, because search cannot have subdirectories
    /// </summary>
    public IEnumerable<IStorage> EnumerateSubDirectories(FileAttributes rejectedAttributes = 0) => [];

    public async Task SearchAsync(SearchOptions searchOptions)
    {
        await RootCatalog.SearchAsync(searchOptions);
    }

    public IEnumerable<IDirectoryItem> EnumerateFiles(FileAttributes rejectedAttributes = FileAttributes.None)
    {
        return RootCatalog.EnumerateFiles(rejectedAttributes);
    }

    #endregion

}