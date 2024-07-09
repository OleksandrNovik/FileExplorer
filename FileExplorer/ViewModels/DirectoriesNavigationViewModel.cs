using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.ViewModels.Messages;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

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

            // Handler that is called when new tab is opened. New directory from that tab is initialized
            Messenger.Register<DirectoriesNavigationViewModel, NewTabOpened>(this, (_, message) =>
            {
                InitializeDirectory(message.TabDirectoryInfo);
                _navigation.History = message.TabNavigationHistory;
                NotifyCanExecute();
            });

            // Handler that is called when user is navigation inside current tab
            Messenger.Register<DirectoriesNavigationViewModel, DirectoryNavigationModel>(this, (_, message) =>
            {
                _navigation.GoForward(message);
                RouteItems.Add(message.Name);
                NotifyCanExecute();
            });

        }

        /// <summary>
        /// Is called when new tab is opened, so directory in that tab should be initialized
        /// </summary>
        /// <param name="directory"> Directory that is held in tab </param>
        private void InitializeDirectory(DirectoryNavigationModel directory)
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
        private async Task NavigateUpDirectory()
        {
            var navigationModel = new DirectoryNavigationModel();
            await navigationModel.InitializeDataAsync(CurrentDirectory.ParentPath);

            _navigation.GoBack(navigationModel);
            SendNavigationMessage(CurrentDirectory.FullPath);
            RouteItems.RemoveAt(RouteItems.Count - 1);
        }

        private bool CanNavigateUp() => CurrentDirectory?.ParentPath != null;

        #endregion

        /// <summary>
        /// Method called when BreadcrumbBar is used to navigate through directory
        /// </summary>
        /// <param name="lastElementIndex"> index of new last element (that is final folder inside route) </param>
        [RelayCommand]
        private async Task UseNavigationBar(int lastElementIndex)
        {
            for (int i = RouteItems.Count - 1; i > lastElementIndex; i--)
            {
                RouteItems.RemoveAt(i);
            }

            var selectedRoute = _router.CreatePathFrom(RouteItems);
            var navigationModel = new DirectoryNavigationModel();
            await navigationModel.InitializeDataAsync(selectedRoute);

            _navigation.GoForward(navigationModel);
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
        private async Task NavigateUsingRouteInput()
        {
            var navigationModel = new DirectoryNavigationModel();
            await navigationModel.InitializeDataAsync(CurrentRoute);

            _navigation.GoForward(navigationModel);
            SendNavigationMessage(CurrentRoute);
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
        /// <param name="path"> Path that will be navigated to </param>
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
