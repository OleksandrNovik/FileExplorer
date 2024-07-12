using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.ViewModels.Messages;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

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
                CurrentRoute = CurrentDirectory.FullPath;
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
        private async Task MoveForward()
        {
            _navigation.GoForward();

            var folder = await _router.UseNavigationRouteAsync(CurrentDirectory.FullPath);
            SendNavigationMessage(folder);

            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoForward() => _navigation.CanGoForward;

        #endregion

        #region GoBack

        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private async Task MoveBack()
        {
            _navigation.GoBack();

            var folder = await _router.UseNavigationRouteAsync(CurrentDirectory.FullPath);
            SendNavigationMessage(folder);

            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }
        private bool CanGoBack() => _navigation.CanGoBack;

        #endregion

        #region RefreshAndNavigateUp

        [RelayCommand]
        private async Task Refresh()
        {
            var folder = await _router.UseNavigationRouteAsync(CurrentDirectory.FullPath);
            Messenger.Send(new NavigationRequiredMessage(folder));
        }

        [RelayCommand(CanExecute = nameof(CanNavigateUp))]
        private async Task NavigateUpDirectory()
        {
            var navigationModel = new DirectoryNavigationModel();
            await navigationModel.InitializeDataAsync(CurrentDirectory.ParentPath);

            _navigation.GoBack(navigationModel);
            var folder = await _router.UseNavigationRouteAsync(CurrentDirectory.FullPath);
            SendNavigationMessage(folder);

            // If we can go up directory but did not document it in the application
            // This condition will be false. So we'll just navigate to a parent folder while it is possible
            if (RouteItems.Count > 0)
            {
                RouteItems.RemoveAt(RouteItems.Count - 1);
            }

            // If there is no route items documented, and we still can navigate to a parent route
            // Will add parent's name as route item to display current location on UI
            if (RouteItems.Count == 0)
            {
                RouteItems.Add(folder.DisplayName);
            }
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
            var folder = await _router.UseNavigationRouteAsync(selectedRoute);

            var navigationModel = new DirectoryNavigationModel();
            await navigationModel.InitializeDataAsync(folder);

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
        private async Task NavigateUsingRouteInput()
        {
            // When user writes down navigation route it can be either file or folder
            StorageFolder folder;
            // We can check if current route is file (has extension)
            if (Path.HasExtension(CurrentRoute))
            {
                // If so we can call message to open this file in DirectoryPageViewModel
                var file = await StorageFile.GetFileFromPathAsync(CurrentRoute);
                Messenger.Send(new FileOpenRequiredMessage(file));
                // And then use parent folder for file as navigation folder
                folder = await file.GetParentAsync();
            }
            // Otherwise we just use route to identify new navigation folder
            else
            {
                folder = await _router.UseNavigationRouteAsync(CurrentRoute);
            }

            var navigationModel = new DirectoryNavigationModel();
            await navigationModel.InitializeDataAsync(folder);

            _navigation.GoForward(navigationModel);

            SendNavigationMessage(folder);

            IsWritingRoute = !IsWritingRoute;
            RouteItems = new ObservableCollection<string>(_router.ExtractRouteItems(CurrentRoute));
        }

        private bool CanUseRouteInput() => _router.IsSpecialRoute(CurrentRoute) || Path.Exists(CurrentRoute);

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
        /// <param name="folder"> Folder that will be navigated to </param>
        private void SendNavigationMessage(StorageFolder folder)
        {
            Messenger.Send(new NavigationRequiredMessage(folder));
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
