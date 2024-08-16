#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models.Messages;
using System;

namespace FileExplorer.ViewModels.Informational
{
    /// <summary>
    /// View model to handle little info bar that provides quick information for user
    /// </summary>
    public sealed partial class InfoBarViewModel : ObservableRecipient
    {
        /// <summary>
        /// Timer that counts down 2 second to auto-close info bar
        /// </summary>
        private readonly DispatcherTimer timer;

        /// <summary>
        /// Is info bar opened
        /// </summary>
        [ObservableProperty]
        private bool isOpen;

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

        /// <summary>
        /// Info bar message (can be ignored)
        /// </summary>
        [ObservableProperty]
        private string? message;

        public InfoBarViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2.2);
            timer.Tick += CloseAfterTime; ;
            isOpen = false;

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

        private void CloseAfterTime(object? sender, object e) => Close();

        [RelayCommand]
        private void Close()
        {
            IsOpen = false;
            timer.Stop();
        }
    }
}
