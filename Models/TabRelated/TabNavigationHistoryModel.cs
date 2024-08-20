using Models.Contracts.Storage;
using Models.Storage.Windows;
using System.Collections.Generic;

namespace Models.TabRelated
{
    /// <summary>
    /// Navigation history of the tab. Stores any information to move forward or back through recent visited directories in the tab
    /// </summary>
    public class TabNavigationHistoryModel
    {
        /// <summary>
        /// Directories that can be navigated forward into
        /// </summary>
        public Stack<IStorage<DirectoryItemWrapper>> ForwardStack { get; } = new();

        /// <summary>
        /// Directories that can be navigated back into
        /// </summary>
        public Stack<IStorage<DirectoryItemWrapper>> BackStack { get; } = new();

        /// <summary>
        /// Is there any directory that can be navigated back
        /// </summary>
        public bool CanGoBack => BackStack.Count > 0;

        /// <summary>
        /// Is there any directory that can be navigated forward
        /// </summary>
        public bool CanGoForward => ForwardStack.Count > 0;
    }
}
