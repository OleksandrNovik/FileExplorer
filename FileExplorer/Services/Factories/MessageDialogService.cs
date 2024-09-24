using FileExplorer.Core.Contracts;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace FileExplorer.Services
{
    /// <summary>
    /// Service that provides access to <see cref="App.MainWindow"/> dialog methods
    /// </summary>
    public sealed class MessageDialogService : IMessageDialogService
    {
        /// <inheritdoc />
        public async Task ShowMessageAsync(string title, string message)
        {
            await App.MainWindow.ShowMessageDialogAsync(title, message);
        }

        /// <inheritdoc />
        public async Task<ContentDialogResult> ShowConfirmationMessageAsync(string title, string message)
        {
            return await App.MainWindow.ShowConfirmationDialogAsync(title, message);
        }

        /// <inheritdoc />
        public async Task ShowModalWindowAsync(ContentDialog dialog)
        {
            await App.MainWindow.ShowCustomDialogAsync(dialog);
        }
    }
}
