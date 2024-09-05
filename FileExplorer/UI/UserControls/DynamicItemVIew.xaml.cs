using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models;
using Models.Messages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class DynamicItemView : UserControl
    {

        public static readonly DependencyProperty ViewOptionsProperty =
            DependencyProperty.Register(nameof(ViewOptions), typeof(ViewOptions),
                typeof(DynamicItemView), new PropertyMetadata(ViewOptions.GridView));

        public ViewOptions ViewOptions
        {
            get => (ViewOptions)GetValue(ViewOptionsProperty);
            set => SetValue(ViewOptionsProperty, value);
        }

        public FileOperationsViewModel FileOperations { get; }
        public ConcurrentWrappersCollection ItemsSource { get; set; }
        public DynamicItemView()
        {
            FileOperations = App.GetService<FileOperationsViewModel>();
            DataContext = this;

            this.InitializeComponent();
        }

    }
}
