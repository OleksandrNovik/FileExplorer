using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Models.Messages;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class ViewOptionsViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ViewOptions viewOptions;

        public ViewOptionsViewModel()
        {
            viewOptions = ViewOptions.GridView;

            Messenger.Register<ViewOptionsViewModel, ViewOptionsChangedMessage>(this, (_, message) =>
            {
                ViewOptions = message.ViewOptions;
            });
        }

        public void OnControlUnloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Messenger.UnregisterAll(this);
        }
    }
}
