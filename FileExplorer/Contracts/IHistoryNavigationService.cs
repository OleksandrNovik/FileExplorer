using FileExplorer.Models;

namespace FileExplorer.Contracts
{
    public interface IHistoryNavigationService
    {
        public DirectoryNavigationModel CurrentDirectory { get; }
        public bool CanGoForward { get; }
        public bool CanGoBack { get; }
        public void GoForward(DirectoryNavigationModel location);
        public void GoForward();
        public void GoBack();
    }
}
