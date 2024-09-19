using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace FileExplorer.Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for preferences page in settings window 
    /// </summary>
    public sealed partial class SettingsPreferencesModel : ObservableObject
    {
        public static SettingsPreferencesModel Default => new()
        {
            Theme = ElementTheme.Default,
            ShowConfirmationMessage = false,
            OpenFolderInNewTab = false,
            Language = "English",
            FoldersFirst = true,
        };
        public ElementTheme Theme { get; set; }

        [ObservableProperty]
        public string language;

        [ObservableProperty]
        private bool showConfirmationMessage;

        [ObservableProperty]
        private bool openFolderInNewTab;

        [ObservableProperty]
        private bool foldersFirst;
    }
}
