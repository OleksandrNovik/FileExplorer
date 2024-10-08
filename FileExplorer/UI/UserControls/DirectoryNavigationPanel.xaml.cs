using Microsoft.UI.Xaml.Controls;
using TabNavigationViewModel = FileExplorer.ViewModels.Controls.TabNavigationViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DirectoryNavigationPanel : UserControl
    {
        public TabNavigationViewModel ViewModel { get; }
        public DirectoryNavigationPanel()
        {
            this.ViewModel = App.GetService<TabNavigationViewModel>();
            this.InitializeComponent();
        }
    }
}
