using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.ViewModels.Search;
using Models.Contracts.Storage;
using Models.Messages;
using Models.Storage.Windows;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.ViewModels
{
    public partial class DirectoriesNavigationViewModel : ObservableRecipient
    {
        /// <summary>
        /// Stores and updates navigation history for a tab (that is currently opened)
        /// </summary>
        private readonly IHistoryNavigationService navigation;

        /// <summary>
        /// Has methods to work with routes (get navigated item, split route into subfolders to conveniently navigate ot create route from parts)
        /// </summary>
        private readonly IDirectoryRouteService router;

        /// <summary>
        /// Storage or search result that is currently opened 
        /// </summary>
        private IStorage<DirectoryItemWrapper> CurrentDirectory => navigation.CurrentDirectory;

        /// <summary>
        /// View model that works with search options and search query, this view model sends message to start the search when user needs to
        /// </summary>
        public SearchOptionsViewModel SearchOperator { get; }

        /// <summary>
        /// Decides if user is currently writing route into the text box or using route breadcrumb bar
        /// </summary>
        [ObservableProperty]
        private bool isWritingRoute;

        /// <summary>
        /// Stores information about route (in string representation) that we are currently located in
        /// </summary>
        [ObservableProperty]
        private string currentRoute;

        /// <summary>
        /// Stores parts of the route for breadcrumb bar to easily navigate to up-folders 
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> routeItems;

        public DirectoriesNavigationViewModel(IHistoryNavigationService navigation, IDirectoryRouteService router, SearchOptionsViewModel searchOperator)
        {
            this.navigation = navigation;
            this.router = router;
            SearchOperator = searchOperator;

            // Handler that is called when new tab is opened. New directory from that tab is initialized
            Messenger.Register<DirectoriesNavigationViewModel, TabOpenedMessage>(this, (_, message) =>
            {
                InitializeDirectory(message.TabStorage);
                this.navigation.History = message.TabNavigationHistory;
                NotifyCanExecute();
            });

            // Handler that is called when user is navigation inside current tab
            Messenger.Register<DirectoriesNavigationViewModel, StorageNavigatedMessage>(this, (_, message) =>
            {
                this.navigation.OpenDirectory(message.NavigatedStorage);
                NotifyCanExecute();
            });
        }

        /// <summary>
        /// Is called when new tab is opened, so directory in that tab should be initialized
        /// </summary>
        /// <param name="directory"> Storage that is held in tab </param>
        private void InitializeDirectory(IStorage<DirectoryItemWrapper> directory)
        {
            navigation.CurrentDirectory = directory;

            CurrentRoute = CurrentDirectory.Path;

            RouteItems = new ObservableCollection<string>(router.ExtractRouteItems(CurrentRoute));
        }

        #region GoForward

        [RelayCommand(CanExecute = nameof(CanGoForward))]
        private void MoveForward()
        {
            navigation.GoForward();

            SendNavigationMessage(CurrentDirectory);

            RouteItems = new ObservableCollection<string>(router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoForward() => navigation.CanGoForward;

        #endregion

        #region GoBack

        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void MoveBack()
        {
            navigation.GoBack();

            SendNavigationMessage(CurrentDirectory);

            RouteItems = new ObservableCollection<string>(router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoBack() => navigation.CanGoBack;

        #endregion

        #region RefreshAndNavigateUp

        [RelayCommand]
        private void Refresh()
        {
            //TODO: Refresh this component
        }


        [RelayCommand(CanExecute = nameof(CanNavigateUp))]
        private void NavigateUpDirectory()
        {
            ArgumentNullException.ThrowIfNull(CurrentDirectory.Parent);

            navigation.GoBack(CurrentDirectory.Parent);

            SendNavigationMessage(CurrentDirectory);

            RouteItems.RemoveAt(RouteItems.Count - 1);
        }

        private bool CanNavigateUp() => CurrentDirectory?.Parent is not null;

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

            var selectedRoute = router.CreatePathFrom(RouteItems);
            var navigationItem = router.UseNavigationRoute(selectedRoute);

            var folder = new DirectoryWrapper(navigationItem.Path);

            navigation.GoForward(folder);
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
                CurrentRoute = router.CreatePathFrom(RouteItems);
            }
        }

        /// <summary>
        /// Uses route that user has inputted into text box and navigates to a new directory
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanUseRouteInput))]
        private void NavigateUsingRouteInput()
        {
            var navigationItem = router.UseNavigationRoute(CurrentRoute);
            var currentDirectory = navigationItem.GetCurrentDirectory();

            if (navigationItem is FileWrapper file)
            {
                Messenger.Send(new FileOpenRequiredMessage(file));
            }

            navigation.GoForward(currentDirectory);

            SendNavigationMessage(currentDirectory);

            IsWritingRoute = !IsWritingRoute;
            RouteItems = new ObservableCollection<string>(router.ExtractRouteItems(CurrentRoute));
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
        private void SendNavigationMessage(IStorage<DirectoryItemWrapper> item)
        {
            Messenger.Send(new NavigationRequiredMessage(item));
            CurrentRoute = CurrentDirectory.Path;
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
