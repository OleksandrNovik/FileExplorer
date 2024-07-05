using FileExplorer.Contracts;
using FileExplorer.Models;

namespace FileExplorer.Services
{
    public class HistoryNavigationService : IHistoryNavigationService
    {
        public TabNavigationHistoryModel History { get; set; }
        public DirectoryNavigationModel CurrentDirectory { get; set; }

        public bool CanGoForward => History != null && History.CanGoForward;
        public bool CanGoBack => History != null && History.CanGoBack;

        public void GoForward(DirectoryNavigationModel location)
        {
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoBack(DirectoryNavigationModel location)
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
