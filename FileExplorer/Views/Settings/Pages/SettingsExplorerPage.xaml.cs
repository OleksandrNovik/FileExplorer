using FileExplorer.ViewModels.Settings;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.Views.Settings.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsExplorerPage : Page
    {
        public SettingsExplorerViewModel ViewModel { get; }
        public SettingsExplorerPage()
        {
            ViewModel = App.GetService<SettingsExplorerViewModel>();
            this.InitializeComponent();
        }
    }
}
