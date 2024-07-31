#nullable enable
using Models.Contracts;
using Models.StorageWrappers;
using System.Collections.Generic;
using System.Threading;

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

    public record SearchOperationRequiredMessage(CancellationTokenSource CancelSearch);

    public record SearchDirectoryMessage(ISearchable<DirectoryItemWrapper> SearchedCatalog);

    public record SearchIterationMessage(ICollection<DirectoryItemWrapper> Items, bool IsSearchFinished);
}
