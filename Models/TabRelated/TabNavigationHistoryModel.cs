using Models.Contracts.Storage;
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
        public Stack<IStorage<IDirectoryItem>> ForwardStack { get; } = new();

        /// <summary>
        /// Directories that can be navigated back into
        /// </summary>
        public Stack<IStorage<IDirectoryItem>> BackStack { get; } = new();

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
