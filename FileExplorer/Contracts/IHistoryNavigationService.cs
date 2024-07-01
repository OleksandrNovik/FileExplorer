namespace FileExplorer.Contracts
{
    public interface IHistoryNavigationService
    {
        public bool CanGoForward { get; }
        public bool CanGoBack { get; }
        public void GoForward(string location);
        public string GoForward();
        public string GoBack();
    }
}
