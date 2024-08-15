using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Models;
using Models.Messages;

namespace FileExplorer.ViewModels.Informational
{
    /// <summary>
    /// View model that handles details menu
    /// </summary>
    public sealed partial class DirectoryItemInfoViewModel : ObservableRecipient
    {
        /// <summary>
        /// Is pane open or closed at the moment
        /// </summary>
        [ObservableProperty]
        private Visibility paneVisibility;

        /// <summary>
        /// Information that is currently shown
        /// </summary>
        [ObservableProperty]
        private DirectoryItemAdditionalInfo info;

        public DirectoryItemInfoViewModel()
        {
            paneVisibility = Visibility.Collapsed;

            // Opens details pane when needed
            Messenger.Register<DirectoryItemInfoViewModel, ShowDetailsMessage>(this, (_, message) =>
            {
                PaneVisibility = Visibility.Visible;
                Info = message.Details;
            });
        }

        /// <summary>
        /// Command to close details pane
        /// </summary>
        [RelayCommand]
        private void Close() => PaneVisibility = Visibility.Collapsed;
    }
}
