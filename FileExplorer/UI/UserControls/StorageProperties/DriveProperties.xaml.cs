using FileExplorer.Models.Additional.Charts;
using FileExplorer.Models.Storage.Additional.Properties;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls.StorageProperties
{
    public sealed partial class DriveProperties : PropertiesUserControl
    {
        public static readonly DependencyProperty DriveProperty =
            DependencyProperty.Register(nameof(Drive), typeof(DriveBasicProperties),
                typeof(DriveProperties), new PropertyMetadata(null));

        public DriveBasicProperties Drive
        {
            get => GetValue(DriveProperty) as DriveBasicProperties;
            set => SetValue(DriveProperty, value);
        }

        public DriveProperties()
        {
            this.InitializeComponent();
        }

        protected override void PropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DriveBasicProperties properties)
            {
                Drive = properties;
                Chart.Segments =
                [
                    new PieChartSegment
                    {
                        Value = properties.SpaceInfo.UsedSpace.InBytes,
                        Color = UsedRect.Fill
                    },
                    new PieChartSegment
                    {
                        Value = properties.SpaceInfo.FreeSpace.InBytes,
                        Color = AvailableRect.Fill
                    }
                ];
            }
        }
    }
}
