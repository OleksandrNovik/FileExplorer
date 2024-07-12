using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class FileDetails : UserControl
    {
        public IStorageItem ItemInfo { get; set; }
        public bool isOpen { get; private set; } = true;

        public FileDetails()
        {
            this.InitializeComponent();
        }
    }
}
