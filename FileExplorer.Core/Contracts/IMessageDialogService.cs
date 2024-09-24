using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace FileExplorer.Core.Contracts
{
    public interface IMessageDialogService
    {
        public Task ShowMessageAsync(string title, string message);

        public Task<ContentDialogResult> ShowConfirmationMessageAsync(string title, string message);

        public Task ShowModalWindowAsync(ContentDialog dialog);
    }
}
