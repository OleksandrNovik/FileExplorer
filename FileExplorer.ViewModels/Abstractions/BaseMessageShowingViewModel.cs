using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// Base class for view models that has to show a message for user
    /// </summary>
    public abstract partial class BaseMessageShowingViewModel : ObservableRecipient
    {
        /// <summary>
        /// Is message open or not
        /// </summary>
        [ObservableProperty]
        private bool isOpen;

        /// <summary>
        /// Message to show
        /// </summary>
        [ObservableProperty]
        private string message;

        /// <summary>
        /// Timer that counts down for auto-close of a message
        /// </summary>
        protected readonly DispatcherTimer timer;

        protected BaseMessageShowingViewModel(double timerInterval)
        {
            isOpen = false;
            timer = new();
            timer.Interval = TimeSpan.FromSeconds(timerInterval);
            timer.Tick += CloseAfterTime;
        }

        /// <summary>
        /// Closes message after timer tick has completed
        /// </summary>
        private void CloseAfterTime(object? sender, object e) => Close();

        /// <summary>
        /// Closes message and stops timer
        /// </summary>
        [RelayCommand]
        protected virtual void Close()
        {
            IsOpen = false;
            timer.Stop();
        }

    }
}
