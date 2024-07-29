using FileExplorer.Views;
using System;
using System.IO;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowExtended
    {
        public MainWindow()
        {
            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Images/WindowIcon.ico"));
            this.InitializeComponent();
        }
    }
}
