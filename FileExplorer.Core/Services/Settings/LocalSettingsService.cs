#nullable enable
using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using Helpers.Application;
using Microsoft.UI.Xaml;
using Models.Settings;
using System;
using System.Diagnostics;

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
        public void WriteSetting<T>(string key, T value)
            where T : struct
        {
            var strValue = value.ToString();
            Debug.Assert(strValue is not null);
            LocalSettings.WriteSetting(key, strValue);
        }

        /// <inheritdoc />
        public void WriteSetting(string key, string value)
        {
            LocalSettings.WriteSetting(key, value);
        }

        /// <inheritdoc />
        public string? ReadString(string key)
        {
            return LocalSettings.ReadSetting(key);
        }

        /// <inheritdoc />
        public bool? ReadBool(string key)
        {
            return parser.ParseBool(ReadString(key));
        }

        /// <inheritdoc />
        public TEnum? ReadEnum<TEnum>(string key) where TEnum : struct, Enum
        {
            return parser.ParseEnum<TEnum>(key);
        }

        /// <inheritdoc />
        public SettingsExplorerModel GetExplorerSettings()
        {
            var defaultSettings = SettingsExplorerModel.Default;

            return new SettingsExplorerModel
            {
                ShowHiddenFiles = ReadBool(LocalSettings.Keys.ShowHiddenFiles) ?? defaultSettings.ShowHiddenFiles,
                HideSystemFiles = ReadBool(LocalSettings.Keys.HideSystemFiles) ?? defaultSettings.HideSystemFiles,
                ShowFileExtensions = ReadBool(LocalSettings.Keys.ShowFileExtensions) ?? defaultSettings.ShowFileExtensions,
            };
        }

        /// <inheritdoc />
        public void SaveExplorerSettings(SettingsExplorerModel settings)
        {
            WriteSetting(LocalSettings.Keys.ShowHiddenFiles, settings.ShowHiddenFiles);
            WriteSetting(LocalSettings.Keys.HideSystemFiles, settings.HideSystemFiles);
            WriteSetting(LocalSettings.Keys.ShowFileExtensions, settings.ShowFileExtensions);
        }

        /// <inheritdoc />
        public SettingsPreferencesModel GetUserPreferences()
        {
            var defaultPreferences = SettingsPreferencesModel.Default;

            return new SettingsPreferencesModel
            {
                Theme = ReadEnum<ElementTheme>(LocalSettings.Keys.Theme) ?? defaultPreferences.Theme,
                ShowConfirmationMessage = ReadBool(LocalSettings.Keys.ShowConfirmationMessage) ?? defaultPreferences.ShowConfirmationMessage,
                OpenFolderInNewTab = ReadBool(LocalSettings.Keys.OpenFolderInNewTab) ?? defaultPreferences.OpenFolderInNewTab,
                Language = ReadString(LocalSettings.Keys.Language) ?? defaultPreferences.Language,
            };
        }

        /// <inheritdoc />
        public void SaveUserPreferences(SettingsPreferencesModel preferences)
        {
            WriteSetting(LocalSettings.Keys.Theme, preferences.Theme);
            WriteSetting(LocalSettings.Keys.ShowConfirmationMessage, preferences.ShowConfirmationMessage);
            WriteSetting(LocalSettings.Keys.OpenFolderInNewTab, preferences.OpenFolderInNewTab);
            WriteSetting(LocalSettings.Keys.Language, preferences.Language);
        }
    }
}
