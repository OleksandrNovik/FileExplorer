using FileExplorer.Core.Contracts;
using Models.Contracts.Storage;
using Models.Storage.Windows;
using Models.TabRelated;

namespace FileExplorer.Core.Services
{
    public class HistoryNavigationService : IHistoryNavigationService
    {
        public TabNavigationHistoryModel History { get; set; }
        public IStorage<DirectoryItemWrapper> CurrentDirectory { get; set; }

        public bool CanGoForward => History is { CanGoForward: true };
        public bool CanGoBack => History is { CanGoBack: true };

        public void GoForward(IStorage<DirectoryItemWrapper> location)
        {
            CurrentDirectory = location;
        }

        public void OpenDirectory(IStorage<DirectoryItemWrapper> location)
        {
            History.ForwardStack.Clear();
            History.BackStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoBack(IStorage<DirectoryItemWrapper> location)
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
