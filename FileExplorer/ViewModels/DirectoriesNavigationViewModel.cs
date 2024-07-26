using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Core.Contracts;
using Models;
using Models.Messages;
using Models.StorageWrappers;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.ViewModels
{
    public partial class DirectoriesNavigationViewModel : ObservableRecipient
    {
        private readonly IHistoryNavigationService _navigation;
        private readonly IDirectoryRouteService _router;
        private DirectoryNavigationInfo CurrentDirectory => _navigation.CurrentDirectory;
        public SearchOperationsViewModel SearchOperator { get; }

        [ObservableProperty]
        private bool isWritingRoute;

        [ObservableProperty]
        private string currentRoute;

        [ObservableProperty]
        private ObservableCollection<string> routeItems;

        public DirectoriesNavigationViewModel(IHistoryNavigationService navigation, IDirectoryRouteService router, SearchOperationsViewModel searchOperator)
        {
            _navigation = navigation;
            _router = router;

            SearchOperator = searchOperator;

            // Handler that is called when new tab is opened. New directory from that tab is initialized
            Messenger.Register<DirectoriesNavigationViewModel, NewTabOpened>(this, (_, message) =>
            {
                InitializeDirectory(message.TabDirectoryInfo);
                _navigation.History = message.TabNavigationHistory;
                NotifyCanExecute();
            });

            // Handler that is called when user is navigation inside current tab
            Messenger.Register<DirectoriesNavigationViewModel, DirectoryNavigationInfo>(this, (_, message) =>
            {
                _navigation.GoForward(message);
                RouteItems.Add(message.Name);
                CurrentRoute = CurrentDirectory.FullPath;
                NotifyCanExecute();
            });

        }

        /// <summary>
        /// Is called when new tab is opened, so directory in that tab should be initialized
        /// </summary>
        /// <param name="directory"> Directory that is held in tab </param>
        private void InitializeDirectory(DirectoryNavigationInfo directory)
        {
            _navigation.CurrentDirectory = directory;

            CurrentRoute = CurrentDirectory.FullPath;

            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }

        #region GoForward

        [RelayCommand(CanExecute = nameof(CanGoForward))]
        private void MoveForward()
        {
            _navigation.GoForward();

            var navigatedDirectory = _router.UseNavigationRoute(CurrentDirectory.FullPath)
                                            .GetCurrentDirectory();
            SendNavigationMessage(navigatedDirectory);

            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoForward() => _navigation.CanGoForward;

        #endregion

        #region GoBack

        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void MoveBack()
        {
            _navigation.GoBack();

            var navigatedDirectory = _router.UseNavigationRoute(CurrentDirectory.FullPath)
                                            .GetCurrentDirectory();
            SendNavigationMessage(navigatedDirectory);

            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoBack() => _navigation.CanGoBack;

        #endregion

        #region RefreshAndNavigateUp

        [RelayCommand]
        private void Refresh()
        {
            var currentFolder = _router.UseNavigationRoute(CurrentDirectory.FullPath)
                                                     .GetCurrentDirectory();

            Messenger.Send(new NavigationRequiredMessage(currentFolder));
        }

        [RelayCommand(CanExecute = nameof(CanNavigateUp))]
        private void NavigateUpDirectory()
        {
            ArgumentNullException.ThrowIfNull(CurrentDirectory.ParentPath);
            var navigationModel = new DirectoryNavigationInfo(CurrentDirectory.ParentPath);

            _navigation.GoBack(navigationModel);
            var folder = _router.UseNavigationRoute(CurrentDirectory.FullPath)
                                              .GetCurrentDirectory();
            SendNavigationMessage(folder);
            RouteItems.RemoveAt(RouteItems.Count - 1);
        }

        private bool CanNavigateUp() => CurrentDirectory?.ParentPath != null;

        #endregion

        /// <summary>
        /// Method called when BreadcrumbBar is used to navigate through directory
        /// </summary>
        /// <param name="lastElementIndex"> index of new last element (that is final folder inside route) </param>
        [RelayCommand]
        private void UseNavigationBar(int lastElementIndex)
        {
            for (var i = RouteItems.Count - 1; i > lastElementIndex; i--)
            {
                RouteItems.RemoveAt(i);
            }

            var selectedRoute = _router.CreatePathFrom(RouteItems);
            var navigationItem = _router.UseNavigationRoute(selectedRoute);

            var folder = new DirectoryWrapper(navigationItem.Path);
            var navigationModel = new DirectoryNavigationInfo(folder);

            _navigation.GoForward(navigationModel);
            SendNavigationMessage(folder);
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
            var navigationItem = _router.UseNavigationRoute(CurrentRoute);
            var currentDirectory = navigationItem.GetCurrentDirectory();

            var navigationModel = new DirectoryNavigationInfo(currentDirectory);

            if (navigationItem is FileWrapper file)
            {
                Messenger.Send(new FileOpenRequiredMessage(file));
            }

            _navigation.GoForward(navigationModel);

            SendNavigationMessage(currentDirectory);

            IsWritingRoute = !IsWritingRoute;
            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }

        private bool CanUseRouteInput() => Path.Exists(CurrentRoute);

        partial void OnCurrentRouteChanged(string value)
        {
            NavigateUsingRouteInputCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsWritingRouteChanged(bool value)
        {
            NavigateUsingRouteInputCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Sends message to all listeners that navigation is required to a certain path
        /// </summary>
        /// <param name="item"> Folder that is being navigated </param>
        private void SendNavigationMessage(DirectoryWrapper item)
        {
            Messenger.Send(new NavigationRequiredMessage(item));
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
