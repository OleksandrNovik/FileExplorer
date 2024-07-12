using FileExplorer.Models;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class FileDetails : UserControl
    {
        public FileInfoModel Details { get; set; }
        public FileDetails()
        {
            this.InitializeComponent();
        }


    }
}
