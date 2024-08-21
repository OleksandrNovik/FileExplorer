using FileExplorer.ViewModels.Settings;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.Views.Settings.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPreferencesPage : Page
    {
        public SettingsPreferencesViewModel ViewModel { get; set; }
        public SettingsPreferencesPage()
        {
            NavigationCacheMode = NavigationCacheMode.Required;
            ViewModel = App.GetService<SettingsPreferencesViewModel>();
            this.InitializeComponent();
        }
    }
}
