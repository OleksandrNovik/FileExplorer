using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using WinUIEx;

namespace FileExplorer.Views
{
    public class WindowExtended : WindowEx
    {
        public async Task ShowCustomDialogAsync(ContentDialog dialog)
        {
            dialog.XamlRoot = Content.XamlRoot;
            await dialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowConfirmationDialogAsync(string content, string title)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = Content.XamlRoot,
                Title = title,
                Content = content,
                SecondaryButtonText = "No",
                PrimaryButtonText = "Yes"
            };
            return await dialog.ShowAsync();
        }
    }
}
