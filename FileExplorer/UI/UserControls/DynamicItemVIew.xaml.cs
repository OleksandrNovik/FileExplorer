using FileExplorer.Core.Contracts.Storage;
using FileExplorer.Models;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Messages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
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

        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(ConcurrentWrappersCollection),
                typeof(DynamicItemView), new PropertyMetadata(null));

        public ViewOptions ViewOptions
        {
            get => (ViewOptions)GetValue(ViewOptionsProperty);
            set => SetValue(ViewOptionsProperty, value);
        }
        public ConcurrentWrappersCollection ItemsSource
        {
            get => (ConcurrentWrappersCollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public ObservableCollection<IDirectoryItem> SelectedItems { get; set; }
        public FlyoutBase ContextMenu { get; set; }
        public INameValidator NameValidator { get; set; }

        public DynamicItemView()
        {
            DataContext = this;

            this.InitializeComponent();
        }
    }
}
