using FileExplorer.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DirectoryToolBar : UserControl
    {
        public DirectoryToolBarViewModel ViewModel { get; }

        public DirectoryToolBar()
        {
            this.ViewModel = App.GetService<DirectoryToolBarViewModel>();
            this.InitializeComponent();
        }
    }
}
