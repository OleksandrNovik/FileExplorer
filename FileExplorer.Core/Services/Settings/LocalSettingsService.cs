#nullable enable
using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using Helpers.Application;
using Microsoft.UI.Xaml;
using Models.Settings;

namespace FileExplorer.Core.Services.Settings
{
    public sealed class LocalSettingsService : ILocalSettingsService
    {
        private readonly IStringParser parser;

        public LocalSettingsService(IStringParser parser)
        {
            this.parser = parser;
        }

        /// <inheritdoc />
        public void WriteSetting(string key, string value) => LocalSettings.WriteSetting(key, value);

        /// <inheritdoc />
        public string? ReadSetting(string key) => LocalSettings.ReadSetting(key);

        /// <inheritdoc />
        public SettingsExplorerModel GetExplorerSettings()
        {
            var defaultSettings = SettingsExplorerModel.Default;

            var showHidden = ReadSetting(LocalSettings.Keys.ShowHiddenFiles);
            var hideSystem = ReadSetting(LocalSettings.Keys.HideSystemFiles);
            var showExtensions = ReadSetting(LocalSettings.Keys.ShowFileExtensions);

            return new SettingsExplorerModel
            {
                ShowHiddenFiles = parser.ParseBool(showHidden, defaultSettings.ShowHiddenFiles),
                HideSystemFiles = parser.ParseBool(hideSystem, defaultSettings.HideSystemFiles),
                ShowFileExtensions = parser.ParseBool(showExtensions, defaultSettings.ShowFileExtensions),
            };
        }

        /// <inheritdoc />
        public void SaveExplorerSettings(SettingsExplorerModel settings)
        {
            WriteSetting(LocalSettings.Keys.ShowHiddenFiles, settings.ShowHiddenFiles.ToString());
            WriteSetting(LocalSettings.Keys.HideSystemFiles, settings.HideSystemFiles.ToString());
            WriteSetting(LocalSettings.Keys.ShowFileExtensions, settings.ShowFileExtensions.ToString());
        }

        /// <inheritdoc />
        public SettingsPreferencesModel GetUserPreferences()
        {
            var defaultPreferences = SettingsPreferencesModel.Default;

            var theme = ReadSetting(LocalSettings.Keys.Theme);
            var showConfirmation = ReadSetting(LocalSettings.Keys.ShowConfirmationMessage);
            var newTabFolder = ReadSetting(LocalSettings.Keys.OpenFolderInNewTab);
            var language = ReadSetting(LocalSettings.Keys.Language) ?? defaultPreferences.Language;

            return new SettingsPreferencesModel
            {
                Theme = parser.ParseEnum<ElementTheme>(theme, defaultPreferences.Theme),
                ShowConfirmationMessage = parser.ParseBool(showConfirmation, defaultPreferences.ShowConfirmationMessage),
                OpenFolderInNewTab = parser.ParseBool(newTabFolder, defaultPreferences.OpenFolderInNewTab),
                Language = language,
            };
        }

        /// <inheritdoc />
        public void SaveUserPreferences(SettingsPreferencesModel preferences)
        {
            WriteSetting(LocalSettings.Keys.Theme, preferences.Theme.ToString());
            WriteSetting(LocalSettings.Keys.ShowConfirmationMessage, preferences.ShowConfirmationMessage.ToString());
            WriteSetting(LocalSettings.Keys.OpenFolderInNewTab, preferences.OpenFolderInNewTab.ToString());
            WriteSetting(LocalSettings.Keys.Language, preferences.Language);
        }
    }
}
