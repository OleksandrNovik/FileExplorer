using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml.Controls;
using Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DynamicItemView : UserControl
    {
        public ConcurrentWrappersCollection ItemsSource { get; set; }
        public ViewOptionsViewModel ViewModel { get; }
        public DynamicItemView()
        {
            DataContext = this;
            ViewModel = App.GetService<ViewOptionsViewModel>();
            Unloaded += ViewModel.OnControlUnloaded;

            this.InitializeComponent();
        }

    }
}
