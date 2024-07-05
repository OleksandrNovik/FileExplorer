using System.Collections.Generic;

namespace FileExplorer.Models
{
    public class TabNavigationHistoryModel
    {
        public Stack<DirectoryNavigationModel> ForwardStack { get; } = new();
        public Stack<DirectoryNavigationModel> BackStack { get; } = new();
        public bool CanGoBack => BackStack.Count > 0;
        public bool CanGoForward => ForwardStack.Count > 0;
    }
}
