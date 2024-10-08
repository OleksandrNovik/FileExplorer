﻿#nullable enable
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.General;
using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.Models.TabRelated;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FileExplorer.Models.Messages
{
    /// <summary>
    /// Message for StorageViewModel to navigate into a directory
    /// </summary>
    /// <param name="NavigatedStorage"></param>
    public record NavigationRequiredMessage(IStorage NavigatedStorage);

    public record StorageNavigatedMessage(IStorage NavigatedStorage);

    /// <summary>
    /// Message that notify listeners that current tab's directory has changed
    /// </summary>
    /// <param name="Storage"></param>
    public record TabStorageChangedMessage(IStorage Storage);

    /// <summary>
    /// Message for DirectoryPageViewModel to navigate into provided directory with provided navigation history
    /// </summary>
    /// <param name="TabStorage"> Storage that current tab is holding </param>
    /// <param name="TabNavigationHistory"> Current tab's history </param>
    public record TabOpenedMessage(IStorage TabStorage, TabNavigationHistoryModel TabNavigationHistory);

    /// <summary>
    /// Message for ShellPageViewModel to open storage in a new tab
    /// </summary>
    /// <param name="TabStorage"> Storage that has to be opened in new tab </param>
    public record OpenTabMessage(IStorage TabStorage);

    public record SearchOperationRequiredMessage(SearchFilter Options);

    /// <summary>
    /// Message to notify search view model to stop searching items
    /// </summary>
    public record StopSearchMessage;

    /// <summary>
    /// Message that notifies InfoBar view model to show InfoBar with provided content
    /// </summary>
    /// <param name="Severity"> InfoBar severity to show particular type of message (warning or success for example) </param>
    /// <param name="Title"> Title of shown message </param>
    /// <param name="Message"> Inner text of shown message </param>
    public record ShowInfoBarMessage(InfoBarSeverity Severity, string Title, string? Message = null);

    /// <summary>
    /// Message to begin search in provided <see cref="IStorage"/> item 
    /// </summary>
    /// <param name="Storage"> Storage that's being searched </param>
    /// <param name="Options"> Search options </param>
    public record SearchStorageMessage(IStorage Storage, SearchFilter Options);

    /// <summary>
    /// Message that requires to show properties of some storage item
    /// </summary>
    /// <param name="Properties"> Properties to show </param>
    public record ShowPropertiesMessage(StorageItemProperties Properties);

    /// <summary>
    /// Message to notify listeners that setting "Show file extension" has been changed
    /// </summary>
    /// <param name="Value"> New value of setting </param>
    public record ShowExtensionsChangedMessage(bool Value);

    public record SortExecutedMessage(ICollection<IDirectoryItem> Sorted);
}
