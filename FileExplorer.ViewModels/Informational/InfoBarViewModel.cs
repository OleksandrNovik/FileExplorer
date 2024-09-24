#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Models.Messages;
using FileExplorer.ViewModels.Abstractions;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.ViewModels.Informational
{
    /// <summary>
    /// View model to handle little info bar that provides quick information for user
    /// </summary>
    public sealed partial class InfoBarViewModel : BaseMessageShowingViewModel
    {
        /// <summary>
        /// Info bar severity to show what kind of message is displayed
        /// </summary>
        [ObservableProperty]
        private InfoBarSeverity severity;

        /// <summary>
        /// Title for info bar message
        /// </summary>
        [ObservableProperty]
        private string title;

        public InfoBarViewModel() : base(2.2)
        {
            Messenger.Register<InfoBarViewModel, ShowInfoBarMessage>(this,
                (_, infoBarMessage) =>
                {
                    IsOpen = true;
                    timer.Start();
                    Title = infoBarMessage.Title;
                    Message = infoBarMessage.Message;
                    Severity = infoBarMessage.Severity;
                });
        }
    }
}
