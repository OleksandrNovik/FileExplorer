using FileExplorer.Models.Storage.Additional.Properties;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls.StorageProperties
{
    public sealed partial class DirectoryItemPropertiesViewer : PropertiesUserControl
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

        protected override void PropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DirectoryItemBasicProperties properties)
            {
                Item = properties;
            }
        }
    }
}
