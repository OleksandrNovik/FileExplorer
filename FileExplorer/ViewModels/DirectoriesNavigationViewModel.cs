using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.ViewModels.Messages;

namespace FileExplorer.ViewModels
{
    public partial class DirectoriesNavigationViewModel : ObservableRecipient
    {
        private readonly IHistoryNavigationService _navigation;
        private DirectoryNavigationModel CurrentDirectory => _navigation.CurrentDirectory;

        public DirectoriesNavigationViewModel(IHistoryNavigationService navigation)
        {
            _navigation = navigation;

            Messenger.Register<DirectoriesNavigationViewModel, DirectoryNavigationModel>(this, (_, message) =>
            {
                _navigation.GoForward(message);
                NotifyCanExecute();
            });
        }

        [RelayCommand(CanExecute = nameof(CanGoForward))]
        private void MoveForward()
        {
            _navigation.GoForward();
            SendNavigationMessage(CurrentDirectory.FullPath);
        }
        private bool CanGoForward() => _navigation.CanGoForward;


        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void MoveBack()
        {
            _navigation.GoBack();
            SendNavigationMessage(CurrentDirectory.FullPath);
        }
        private bool CanGoBack() => _navigation.CanGoBack;

        [RelayCommand]
        private void Refresh()
        {
            Messenger.Send(new NavigationRequiredMessage(CurrentDirectory.FullPath));
        }

        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void NavigateUpDirectory()
        {
            MoveBackCommand.Execute(null);
        }

        private void SendNavigationMessage(string path)
        {
            Messenger.Send(new NavigationRequiredMessage(path));
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
