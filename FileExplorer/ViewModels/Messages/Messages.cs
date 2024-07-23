#nullable enable
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using System.Collections.Generic;

namespace FileExplorer.ViewModels.Messages
{
    /// <summary>
    /// Message for <see cref="DirectoryPageViewModel"/> to navigate into a directory
    /// </summary>
    /// <param name="NavigatedDirectory"></param>
    public record NavigationRequiredMessage(DirectoryWrapper NavigatedDirectory);

    /// <summary>
    /// Message for <see cref="DirectoryPageViewModel"/> to open file path for which has been written in the route
    /// </summary>
    /// <param name="OpenFile"> File that has to be opened </param>
    public record FileOpenRequiredMessage(FileWrapper OpenFile);

    /// <summary>
    /// Message for <see cref="DirectoryPageViewModel"/> to navigate into provided directory with provided navigation history
    /// </summary>
    /// <param name="TabDirectoryInfo"> Directory that current tab is holding </param>
    /// <param name="TabNavigationHistory"> Current tab's history </param>
    public record NewTabOpened(DirectoryNavigationInfo TabDirectoryInfo, TabNavigationHistoryModel TabNavigationHistory);

    /// <summary>
    /// Message for <see cref="ShellPageViewModel"/> to open directory in a new tab
    /// </summary>
    /// <param name="TabDirectory"> Directory that has to be opened in new tab </param>
    public record OpenTabMessage(DirectoryWrapper TabDirectory);

    public record InitializeToolBarMessage(DirectoryWrapper? CurrentDirectory, IList<DirectoryItemWrapper>? SelectedItems);

    public record DirectoryChangedMessage(ICollection<DirectoryItemWrapper>? Added = null, ICollection<DirectoryItemWrapper>? Removed = null);
}
