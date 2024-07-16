using System.Collections.Generic;

namespace FileExplorer.Models
{
    public class TabNavigationHistoryModel
    {
        public Stack<DirectoryNavigationInfo> ForwardStack { get; } = new();
        public Stack<DirectoryNavigationInfo> BackStack { get; } = new();
        public bool CanGoBack => BackStack.Count > 0;
        public bool CanGoForward => ForwardStack.Count > 0;
    }
}
