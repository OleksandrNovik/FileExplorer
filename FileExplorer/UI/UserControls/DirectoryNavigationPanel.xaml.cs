using FileExplorer.ViewModels;
using FileExplorer.Views.Settings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DirectoryNavigationPanel : UserControl
    {
        public DirectoriesNavigationViewModel ViewModel { get; }
        public DirectoryNavigationPanel()
        {
            this.ViewModel = App.GetService<DirectoriesNavigationViewModel>();
            this.InitializeComponent();
        }

        private async void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            await App.MainWindow.ShowCustomDialog(new SettingsContentDialog());
        }
    }
}
