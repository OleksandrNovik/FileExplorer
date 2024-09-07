using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models.Storage.Additional.Properties;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls.StorageProperties
{
    public sealed partial class DirectoryItemPropertiesViewer : UserControl
    {
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(DirectoryItemBasicProperties),
                typeof(DirectoryItemPropertiesViewer), new PropertyMetadata(null));

        public DirectoryItemBasicProperties Item
        {
            get => GetValue(ItemProperty) as DirectoryItemBasicProperties;
            set => SetValue(ItemProperty, value);
        }

        public DirectoryItemPropertiesViewer()
        {
            this.InitializeComponent();
        }
    }
}
