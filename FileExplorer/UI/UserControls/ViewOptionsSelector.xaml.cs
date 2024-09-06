using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class ViewOptionsSelector : UserControl
    {
        public ViewOptionsViewModel ViewModel { get; }
        public ViewOptionsSelector()
        {
            ViewModel = App.GetService<ViewOptionsViewModel>();
            this.InitializeComponent();
        }
    }
}
