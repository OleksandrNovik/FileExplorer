using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Contracts;

namespace FileExplorer.ViewModels
{
    public partial class DirectoriesNavigationViewModel : ObservableRecipient
    {
        public const int NavigationRequiredChannelCode = 1;
        private readonly IHistoryNavigationService _navigation;

        public DirectoriesNavigationViewModel(IHistoryNavigationService navigation)
        {
            _navigation = navigation;

            Messenger.Register<DirectoriesNavigationViewModel, DirectoryOpenedMessage, int>(this,
                DirectoryPageViewModel.FolderOpenedChannelCode, (_, message) =>
            {
                _navigation.GoForward(message.DirectoryPath);
                NotifyCanExecute();
            });
        }

        [RelayCommand(CanExecute = nameof(CanGoForward))]
        private void MoveForward()
        {
            var forwardDirectory = _navigation.GoForward();
            SendMessage(forwardDirectory);
        }
        private bool CanGoForward() => _navigation.CanGoForward;


        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void MoveBack()
        {
            var backDirectory = _navigation.GoBack();
            SendMessage(backDirectory);
        }
        private bool CanGoBack() => _navigation.CanGoBack;

        private void SendMessage(string message)
        {
            Messenger.Send(new DirectoryOpenedMessage(message), NavigationRequiredChannelCode);
            NotifyCanExecute();
        }

        private void NotifyCanExecute()
        {
            MoveBackCommand.NotifyCanExecuteChanged();
            MoveForwardCommand.NotifyCanExecuteChanged();
        }

    }
}
