using FileExplorer.Core.Contracts.General;
using Models.TabRelated;
using System;

namespace FileExplorer.Core.Contracts.DirectoriesNavigation
{
    /// <summary>
    /// Interface to identify navigation service for a left navigation pane
    /// </summary>
    public interface INavigationService : IBasicNavigationService<string>
    {
        public void NotifyTabOpened(TabModel openedTab);

        public event EventHandler<TabModel> TabOpened;
    }
}
