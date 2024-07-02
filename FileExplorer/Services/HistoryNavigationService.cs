using FileExplorer.Contracts;
using FileExplorer.Models;
using System.Collections.Generic;

namespace FileExplorer.Services
{
    public class HistoryNavigationService : IHistoryNavigationService
    {
        private readonly Stack<DirectoryNavigationModel> _forwardStack = new Stack<DirectoryNavigationModel>();
        private readonly Stack<DirectoryNavigationModel> _backStack = new Stack<DirectoryNavigationModel>();
        public DirectoryNavigationModel CurrentDirectory { get; private set; } = new(@"D:\Files");
        public bool CanGoForward => _forwardStack.Count > 0;
        public bool CanGoBack => _backStack.Count > 0;

        public void GoForward(DirectoryNavigationModel location)
        {
            _backStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoBack(DirectoryNavigationModel location)
        {
            _forwardStack.Push(CurrentDirectory);
            CurrentDirectory = location;
        }

        public void GoForward()
        {
            _backStack.Push(CurrentDirectory);
            CurrentDirectory = _forwardStack.Pop();
        }

        public void GoBack()
        {
            _forwardStack.Push(CurrentDirectory);
            CurrentDirectory = _backStack.Pop();
        }
    }
}
