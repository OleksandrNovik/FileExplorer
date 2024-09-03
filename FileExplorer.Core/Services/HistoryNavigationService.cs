using FileExplorer.Core.Contracts;
using Models.Contracts.Storage;
using Models.TabRelated;

namespace FileExplorer.Core.Services
{
    public class HistoryNavigationService : IHistoryNavigationService
    {
        public TabNavigationHistoryModel History { get; set; }
        public IStorage CurrentDirectory { get; set; }

        public bool CanGoForward => History is { CanGoForward: true };
        public bool CanGoBack => History is { CanGoBack: true };

        public void GoForward(IStorage location)
        {
            CurrentDirectory = location;
        }

        public void OpenDirectory(IStorage location)
        {
            History.ForwardStack.Clear();
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoBack(IStorage location)
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
