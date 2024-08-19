using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models.Storage.Drives;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DriveInformationItem : UserControl
    {
        public DriveWrapper Drive
        {
            get => GetValue(DriveProperty) as DriveWrapper;
            set => SetValue(DriveProperty, value);
        }

        public static DependencyProperty DriveProperty = DependencyProperty
            .Register(nameof(Drive), typeof(DriveWrapper), typeof(DriveInformationItem), new PropertyMetadata(null));

        public DriveInformationItem()
        {
            this.InitializeComponent();
        }
    }
}
