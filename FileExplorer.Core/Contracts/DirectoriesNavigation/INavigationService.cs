#nullable enable
using FileExplorer.Core.Contracts.General;
using Models.Enums;
using Models.TabRelated;
using System;

namespace FileExplorer.Core.Contracts.DirectoriesNavigation
{
    /// <summary>
    /// Interface to identify navigation service for a left navigation pane
    /// </summary>
    public interface INavigationService : IBasicNavigationService<StorageContentType>
    {
        public void NotifyTabOpened(TabModel openedTab);

        public event EventHandler<TabModel> TabOpened;
    }
}
