using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models.Storage.Abstractions;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DynamicItemView : UserControl
    {
        public ObservableCollection<InteractiveStorageItem> ItemsSource { get; set; }
        public DataTemplate GridTemplate { get; set; }

        public DynamicItemView()
        {
            this.InitializeComponent();
        }
    }
}
