using FileExplorer.Core.Contracts.Factories;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models;
using Models.Contracts.Storage.Directory;
using Models.Messages;
using System.Collections.ObjectModel;

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
        public ConcurrentWrappersCollection ItemsSource { get; set; }
        public ObservableCollection<IDirectoryItem> SelectedItems { get; set; }
        public IMenuFlyoutBuilder FlyoutBuilder { get; set; }

        public DynamicItemView()
        {
            DataContext = this;

            this.InitializeComponent();
        }
    }
}
