using FileExplorer.ViewModels.Informational;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class ItemPropertiesPanel : UserControl
    {
        public ItemPropertiesPanelViewModel ViewModel { get; }
        public ItemPropertiesPanel()
        {
            ViewModel = App.GetService<ItemPropertiesPanelViewModel>();
            this.InitializeComponent();
        }
    }
}
