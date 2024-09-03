using CommunityToolkit.Mvvm.ComponentModel;
using Helpers.Application;
using Helpers.General;
using Microsoft.UI.Xaml;
using Models.Contracts;

namespace Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for preferences page in settings window 
    /// </summary>
    public sealed partial class SettingsPreferencesModel : ObservableObject, ISettingsModel
    {
        private static SettingsPreferencesModel Default = new()
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

        /// <summary>
        /// Creates a model that contains values from settings
        /// </summary>
        public static SettingsPreferencesModel FromSettings()
        {
            var theme = LocalSettings.ReadSetting(LocalSettings.Keys.Theme);
            var showConfirmation = LocalSettings.ReadSetting(LocalSettings.Keys.ShowConfirmationMessage);
            var newTabFolder = LocalSettings.ReadSetting(LocalSettings.Keys.OpenFolderInNewTab);
            var language = LocalSettings.ReadSetting(LocalSettings.Keys.Language) ?? Default.Language;

            return new SettingsPreferencesModel
            {
                Theme = theme.ParseEnum<ElementTheme>(Default.Theme),
                ShowConfirmationMessage = showConfirmation.ParseBool(Default.ShowConfirmationMessage),
                OpenFolderInNewTab = newTabFolder.ParseBool(Default.OpenFolderInNewTab),
                Language = language,
            };
        }

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
