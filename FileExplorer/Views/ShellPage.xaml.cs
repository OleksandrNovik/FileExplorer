using FileExplorer.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public ShellPageViewModel ViewModel { get; }
        public ShellPage(ShellPageViewModel vm)
        {
            this.ViewModel = vm;
            this.InitializeComponent();
            this.ViewModel.TabService.CurrentTab = NavigationFrame;
            this.ViewModel.OpenNewTab();
        }
    }
}
