using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;

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


        public DirectoryItemInfoViewModel()
        {
            paneVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// ItemInvokedCommand to close details pane
        /// </summary>
        [RelayCommand]
        private void Close() => PaneVisibility = Visibility.Collapsed;
    }
}
