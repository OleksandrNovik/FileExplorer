using FileExplorer.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectoryPage : Page
    {
        public DirectoryPageViewModel ViewModel { get; }
        public DirectoryPage()
        {
            this.ViewModel = App.GetService<DirectoryPageViewModel>();
            this.Resources["EndRenamingCommand"] = this.ViewModel.EndRenamingItemCommand;
            this.InitializeComponent();
        }
    }
}
