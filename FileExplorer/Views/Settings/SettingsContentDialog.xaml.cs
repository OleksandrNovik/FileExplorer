using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SettingsViewModel = FileExplorer.ViewModels.Settings.SettingsViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.Views.Settings
{
    public sealed partial class SettingsContentDialog : ContentDialog
    {
        public SettingsViewModel ViewModel { get; }
        public SettingsContentDialog()
        {
            ViewModel = App.GetService<SettingsViewModel>();
            this.InitializeComponent();
            ViewModel.NavigationService.Frame = CurrentSettingsPage;
        }

        /// <summary>
        /// When close button is pressed we save last opened options and close dialog
        /// </summary>
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.NavigateFromCurrent();
            Hide();
        }
    }
}
