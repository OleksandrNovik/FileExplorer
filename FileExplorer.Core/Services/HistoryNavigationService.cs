using FileExplorer.Core.Contracts;
using Models.General;
using Models.TabRelated;

namespace FileExplorer.Core.Services
{
    public class HistoryNavigationService : IHistoryNavigationService
    {
        public TabNavigationHistoryModel History { get; set; }
        public DirectoryNavigationInfo CurrentDirectory { get; set; }

        public bool CanGoForward => History is { CanGoForward: true };
        public bool CanGoBack => History is { CanGoBack: true };

        public void GoForward(DirectoryNavigationInfo location)
        {
            CurrentDirectory = location;
        }

        public void OpenDirectory(DirectoryNavigationInfo location)
        {
            History.ForwardStack.Clear();
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoBack(DirectoryNavigationInfo location)
        {
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoForward()
        {
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = History.ForwardStack.Pop();
        }

        public void GoBack()
        {
            History.ForwardStack.Push(CurrentDirectory);
            CurrentDirectory = History.BackStack.Pop();
        }
    }
}
