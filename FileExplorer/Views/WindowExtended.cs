using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using WinUIEx;

namespace FileExplorer.Views
{
    public class WindowExtended : WindowEx
    {
        public async Task<ContentDialogResult> ShowYesNoDialog(string content, string title)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = base.Content.XamlRoot,
                Title = title,
                Content = content,
                SecondaryButtonText = "No",
                PrimaryButtonText = "Yes"
            };
            return await dialog.ShowAsync();
        }
    }
}
