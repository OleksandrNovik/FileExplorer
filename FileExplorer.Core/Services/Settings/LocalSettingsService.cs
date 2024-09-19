#nullable enable
using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Helpers.Application;
using FileExplorer.Models.Settings;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.IO;

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
        public FileAttributes GetSkippedAttributes()
        {
            var explorerSettings = GetExplorerSettings();

            // Checking what files are not allowed to be shown
            FileAttributes rejectedAttributes = 0;

            if (explorerSettings.HideSystemFiles)
            {
                rejectedAttributes |= FileAttributes.System;
            }

            if (!explorerSettings.ShowHiddenFiles)
            {
                rejectedAttributes |= FileAttributes.Hidden;
            }

            return rejectedAttributes;
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
                FoldersFirst = ReadBool(LocalSettings.Keys.FoldersFirst) ?? defaultPreferences.FoldersFirst,
            };
        }

        /// <inheritdoc />
        public void SaveUserPreferences(SettingsPreferencesModel preferences)
        {
            WriteSetting(LocalSettings.Keys.Theme, preferences.Theme);
            WriteSetting(LocalSettings.Keys.ShowConfirmationMessage, preferences.ShowConfirmationMessage);
            WriteSetting(LocalSettings.Keys.OpenFolderInNewTab, preferences.OpenFolderInNewTab);
            WriteSetting(LocalSettings.Keys.Language, preferences.Language);
            WriteSetting(LocalSettings.Keys.FoldersFirst, preferences.FoldersFirst);
        }
    }
}
