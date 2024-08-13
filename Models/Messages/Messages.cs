#nullable enable
using Models.General;
using Models.StorageWrappers;
using Models.TabRelated;

namespace Models.Messages
{
    /// <summary>
    /// Message for DirectoryPageViewModel to navigate into a directory
    /// </summary>
    /// <param name="NavigatedDirectory"></param>
    public record NavigationRequiredMessage(DirectoryWrapper NavigatedDirectory);

    /// <summary>
    /// Message for DirectoryPageViewModel to open file path for which has been written in the route
    /// </summary>
    /// <param name="OpenFile"> File that has to be opened </param>
    public record FileOpenRequiredMessage(FileWrapper OpenFile);

    /// <summary>
    /// Message for DirectoryPageViewModel to navigate into provided directory with provided navigation history
    /// </summary>
    /// <param name="TabDirectoryInfo"> Directory that current tab is holding </param>
    /// <param name="TabNavigationHistory"> Current tab's history </param>
    public record TabOpenedMessage(DirectoryNavigationInfo TabDirectoryInfo, TabNavigationHistoryModel TabNavigationHistory);

    /// <summary>
    /// Message for ShellPageViewModel to open directory in a new tab
    /// </summary>
    /// <param name="TabDirectory"> Directory that has to be opened in new tab </param>
    public record OpenTabMessage(DirectoryWrapper TabDirectory);

    public record SearchOperationRequiredMessage(SearchOptionsModel Options);

    /// <summary>
    /// Message to notify search view model to continue search that was stopped time ago
    /// </summary>
    public record ContinueSearchMessage;

    /// <summary>
    /// Message to notify search view model to stop searching items
    /// </summary>
    public record StopSearchMessage;

    public record SearchStartedMessage<TElement>(CachedSearchResult<TElement> CachedResult);

    public record NavigateToSearchResult<TElement>(CachedSearchResult<TElement> SearchResult);

    public record ShowDetailsMessage(DirectoryItemAdditionalInfo Details);
}
