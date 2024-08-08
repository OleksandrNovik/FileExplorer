using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using WinUIEx;

namespace FileExplorer.Views
{
    public class WindowExtended : WindowEx
    {
        public async Task ShowCustomDialog(ContentDialog dialog)
        {
            dialog.XamlRoot = Content.XamlRoot;
            await dialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowYesNoDialog(string content, string title)
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
