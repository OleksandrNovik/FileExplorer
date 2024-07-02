using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.ViewModels.Messages;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels
{
    public partial class DirectoriesNavigationViewModel : ObservableRecipient
    {
        private readonly IHistoryNavigationService _navigation;
        private readonly IDirectoryRouteService _router;

        private DirectoryNavigationModel CurrentDirectory => _navigation.CurrentDirectory;

        [ObservableProperty]
        private bool isWritingRoute;

        [ObservableProperty]
        private string currentRoute;

        [ObservableProperty]
        private ObservableCollection<string> routeItems;

        public DirectoriesNavigationViewModel(IHistoryNavigationService navigation, IDirectoryRouteService router)
        {
            _navigation = navigation;
            _router = router;

            Messenger.Register<DirectoriesNavigationViewModel, DirectoryNavigationModel>(this, (_, message) =>
            {
                _navigation.GoForward(message);
                RouteItems.Add(message.Name);
                NotifyCanExecute();
            });

            CurrentRoute = CurrentDirectory.FullPath;

            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }

        #region GoForward

        [RelayCommand(CanExecute = nameof(CanGoForward))]
        private void MoveForward()
        {
            _navigation.GoForward();
            SendNavigationMessage(CurrentDirectory.FullPath);
            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoForward() => _navigation.CanGoForward;

        #endregion

        #region GoBack

        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void MoveBack()
        {
            _navigation.GoBack();
            SendNavigationMessage(CurrentDirectory.FullPath);
            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoBack() => _navigation.CanGoBack;

        #endregion

        #region RefreshAndNavigateUp

        [RelayCommand]
        private void Refresh()
        {
            Messenger.Send(new NavigationRequiredMessage(CurrentDirectory.FullPath));
        }

        [RelayCommand(CanExecute = nameof(CanNavigateUp))]
        private void NavigateUpDirectory()
        {
            _navigation.GoBack(new DirectoryNavigationModel(CurrentDirectory.ParentPath));
            SendNavigationMessage(CurrentDirectory.FullPath);
            RouteItems.RemoveAt(RouteItems.Count - 1);
        }

        private bool CanNavigateUp() => CurrentDirectory.ParentPath != null;

        #endregion

        [RelayCommand]
        private void UseNavigationBar(int lastElementIndex)
        {
            for (int i = RouteItems.Count - 1; i > lastElementIndex; i--)
            {
                RouteItems.RemoveAt(i);
            }
            SendNavigationMessage(_router.CreatePathFrom(RouteItems));
        }

        private void SendNavigationMessage(string path)
        {
            Messenger.Send(new NavigationRequiredMessage(path));
            CurrentRoute = CurrentDirectory.FullPath;
            NotifyCanExecute();
        }

        private void NotifyCanExecute()
        {
            MoveBackCommand.NotifyCanExecuteChanged();
            MoveForwardCommand.NotifyCanExecuteChanged();
            NavigateUpDirectoryCommand.NotifyCanExecuteChanged();
        }

    }
}
