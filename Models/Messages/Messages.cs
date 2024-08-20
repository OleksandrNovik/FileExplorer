#nullable enable
using Microsoft.UI.Xaml.Controls;
using Models.Contracts.Storage;
using Models.General;
using Models.Storage.Windows;
using Models.TabRelated;

namespace Models.Messages
{
    /// <summary>
    /// Message for DirectoryPageViewModel to navigate into a directory
    /// </summary>
    /// <param name="NavigatedStorage"></param>
    public record NavigationRequiredMessage(IStorage<DirectoryItemWrapper> NavigatedStorage);

    public record StorageNavigatedMessage(IStorage<DirectoryItemWrapper> NavigatedStorage);

    /// <summary>
    /// Message that notify listeners that current tab's directory has changed
    /// </summary>
    /// <param name="Directory"></param>
    public record TabStorageChangedMessage(IStorage<DirectoryItemWrapper> Directory);

    /// <summary>
    /// Message for DirectoryPageViewModel to open file path for which has been written in the route
    /// </summary>
    /// <param name="OpenFile"> File that has to be opened </param>
    public record FileOpenRequiredMessage(FileWrapper OpenFile);

    /// <summary>
    /// Message for DirectoryPageViewModel to navigate into provided directory with provided navigation history
    /// </summary>
    /// <param name="TabStorage"> Storage that current tab is holding </param>
    /// <param name="TabNavigationHistory"> Current tab's history </param>
    public record TabOpenedMessage(IStorage<DirectoryItemWrapper> TabStorage, TabNavigationHistoryModel TabNavigationHistory);

    /// <summary>
    /// Message for ShellPageViewModel to open storage in a new tab
    /// </summary>
    /// <param name="TabStorage"> Storage that has to be opened in new tab </param>
    public record OpenTabMessage(IStorage<DirectoryItemWrapper> TabStorage);

    public record SearchOperationRequiredMessage(SearchOptionsModel Options);

    /// <summary>
    /// Message to notify search view model to stop searching items
    /// </summary>
    public record StopSearchMessage;

    public record NavigateToSearchResult<TElement>(CachedSearchResult<TElement> SearchResult);

    public record ShowDetailsMessage(DirectoryItemAdditionalInfo Details);

    public record ShowInfoBarMessage(InfoBarSeverity Severity, string Title, string? Message = null);
}
