using FileExplorer.Core.Contracts;
using Models;

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
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoBack(DirectoryNavigationInfo location)
        {
            History.ForwardStack.Push(CurrentDirectory);
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
