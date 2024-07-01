using FileExplorer.Contracts;
using System.Collections.Generic;

namespace FileExplorer.Services
{
    public class HistoryNavigationService : IHistoryNavigationService
    {
        private readonly Stack<string> _forwardStack = new Stack<string>();
        private readonly Stack<string> _backStack = new Stack<string>();
        private string _currentDirectoryName;

        public bool CanGoForward => _forwardStack.Count > 0;
        public bool CanGoBack => _backStack.Count > 0;

        public HistoryNavigationService(string currentDirectoryName)
        {
            _currentDirectoryName = currentDirectoryName;
        }

        public void GoForward(string location)
        {
            _backStack.Push(_currentDirectoryName);
            _currentDirectoryName = location;
        }

        public string GoForward()
        {
            _backStack.Push(_currentDirectoryName);
            _currentDirectoryName = _forwardStack.Pop();
            return _currentDirectoryName;
        }

        public string GoBack()
        {
            _forwardStack.Push(_currentDirectoryName);
            _currentDirectoryName = _backStack.Pop();
            return _currentDirectoryName;
        }
    }
}
