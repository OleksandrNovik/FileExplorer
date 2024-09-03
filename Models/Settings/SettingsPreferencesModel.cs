using CommunityToolkit.Mvvm.ComponentModel;
using Helpers.Application;
using Microsoft.UI.Xaml;
using Models.Contracts;

namespace Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for preferences page in settings window 
    /// </summary>
    public sealed partial class SettingsPreferencesModel : ObservableObject, ISettingsModel
    {
        public static SettingsPreferencesModel Default => new()
        {
            Theme = ElementTheme.Default,
            ShowConfirmationMessage = false,
            OpenFolderInNewTab = false,
            Language = "English",
        };
        public ElementTheme Theme { get; set; }

        [ObservableProperty]
        public string language;

        [ObservableProperty]
        private bool showConfirmationMessage;

        [ObservableProperty]
        private bool openFolderInNewTab;

        /// <inheritdoc />
        public void SaveSettings()
        {
            LocalSettings.WriteSetting(LocalSettings.Keys.Theme, Theme.ToString());
            LocalSettings.WriteSetting(LocalSettings.Keys.ShowConfirmationMessage, ShowConfirmationMessage.ToString());
            LocalSettings.WriteSetting(LocalSettings.Keys.OpenFolderInNewTab, OpenFolderInNewTab.ToString());
            LocalSettings.WriteSetting(LocalSettings.Keys.Language, Language);
        }
    }
}
