using CommunityToolkit.Mvvm.ComponentModel;

namespace Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for explorer page in settings window 
    /// </summary>
    public sealed partial class SettingsExplorerModel : ObservableObject
    {
        public static SettingsExplorerModel Default => new()
        {
            ShowHiddenFiles = false,
            HideSystemFiles = true,
            ShowFileExtensions = false,
        };

        [ObservableProperty]
        private bool showFileExtensions;

        [ObservableProperty]
        private bool hideSystemFiles;

        [ObservableProperty]
        private bool showHiddenFiles;
    }
}
