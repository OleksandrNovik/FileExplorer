using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.ViewModels.Messages;
using System.Collections.ObjectModel;
using System.IO;

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

            var selectedRoute = _router.CreatePathFrom(RouteItems);
            _navigation.GoForward(new DirectoryNavigationModel(selectedRoute));
            SendNavigationMessage(selectedRoute);
        }

        /// <summary>
        /// Unselects input field for navigation route or shows it whenever needed
        /// If user typed in not existing route switches back to current route
        /// </summary>
        [RelayCommand]
        private void SwitchNavigationBarMode()
        {
            IsWritingRoute = !IsWritingRoute;

            if (!CanUseRouteInput())
            {
                CurrentRoute = _router.CreatePathFrom(RouteItems);
            }
        }

        /// <summary>
        /// Uses route that user has inputted into text box and navigates to a new directory
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanUseRouteInput))]
        private void NavigateUsingRouteInput()
        {
            _navigation.GoForward(new DirectoryNavigationModel(CurrentRoute));
            SendNavigationMessage(CurrentRoute);
            IsWritingRoute = !IsWritingRoute;
            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }

        private bool CanUseRouteInput() => IsWritingRoute && Path.Exists(CurrentRoute);

        partial void OnCurrentRouteChanged(string value)
        {
            NavigateUsingRouteInputCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsWritingRouteChanged(bool value)
        {
            NavigateUsingRouteInputCommand.NotifyCanExecuteChanged();
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
