﻿#nullable enable
using Microsoft.UI.Xaml.Controls;
using Models.Contracts.Storage;
using Models.General;
using Models.Storage.Windows;
using Models.TabRelated;

namespace Models.Messages
{
    /// <summary>
    /// Message for StorageViewModel to navigate into a directory
    /// </summary>
    /// <param name="NavigatedStorage"></param>
    public record NavigationRequiredMessage(IStorage<IDirectoryItem> NavigatedStorage);

    public record StorageNavigatedMessage(IStorage<IDirectoryItem> NavigatedStorage);

    /// <summary>
    /// Message that notify listeners that current tab's directory has changed
    /// </summary>
    /// <param name="Storage"></param>
    public record TabStorageChangedMessage(IStorage<IDirectoryItem> Storage);

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
    public record TabOpenedMessage(IStorage<IDirectoryItem> TabStorage, TabNavigationHistoryModel TabNavigationHistory);

    /// <summary>
    /// Message for ShellPageViewModel to open storage in a new tab
    /// </summary>
    /// <param name="TabStorage"> Storage that has to be opened in new tab </param>
    public record OpenTabMessage(IStorage<IDirectoryItem> TabStorage);

    public record SearchOperationRequiredMessage(SearchFilter Options);

    /// <summary>
    /// Message to notify search view model to stop searching items
    /// </summary>
    public record StopSearchMessage;

    public record ShowDetailsMessage(DirectoryItemAdditionalInfo Details);

    /// <summary>
    /// Message that notifies InfoBar view model to show InfoBar with provided content
    /// </summary>
    /// <param name="Severity"> InfoBar severity to show particular type of message (warning or success for example) </param>
    /// <param name="Title"> Title of shown message </param>
    /// <param name="Message"> Inner text of shown message </param>
    public record ShowInfoBarMessage(InfoBarSeverity Severity, string Title, string? Message = null);

    /// <summary>
    /// Message to begin search in provided <see cref="IStorage{TElement}"/> item 
    /// </summary>
    /// <param name="Storage"> Storage that's being searched </param>
    /// <param name="Options"> Search options </param>
    public record SearchStorageMessage(IStorage<IDirectoryItem> Storage, SearchFilter Options);
}
