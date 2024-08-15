using FileExplorer.ViewModels.Informational;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DirectoryItemInfoPane : UserControl
    {
        public DirectoryItemInfoViewModel ViewModel { get; }
        public DirectoryItemInfoPane()
        {
            ViewModel = App.GetService<DirectoryItemInfoViewModel>();
            this.InitializeComponent();
        }
    }
}
