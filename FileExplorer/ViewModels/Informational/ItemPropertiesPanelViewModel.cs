using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Models.Messages;
using Models.Storage.Abstractions;

namespace FileExplorer.ViewModels.Informational
{
    /// <summary>
    /// View model that handles details menu
    /// </summary>
    public sealed partial class ItemPropertiesPanelViewModel : ObservableRecipient
    {
        /// <summary>
        /// Is pane open or closed at the moment
        /// </summary>
        [ObservableProperty]
        private Visibility paneVisibility;

        [ObservableProperty]
        private BasicStorageItemProperties properties;

        public ItemPropertiesPanelViewModel()
        {
            paneVisibility = Visibility.Collapsed;

            Messenger.Register<ItemPropertiesPanelViewModel, ShowPropertiesMessage>(this, (_, message) =>
            {
                Properties = message.Properties;
            });
        }

        /// <summary>
        /// ItemInvokedCommand to close details pane
        /// </summary>
        [RelayCommand]
        private void Close() => PaneVisibility = Visibility.Collapsed;
    }
}
