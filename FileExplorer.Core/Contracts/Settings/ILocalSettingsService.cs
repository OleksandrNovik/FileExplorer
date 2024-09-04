#nullable enable
using Models.Settings;
using System;

namespace FileExplorer.Core.Contracts.Settings
{
    public interface ILocalSettingsService
    {
        /// <summary>
        /// Writes provided value to a specific local settings key
        /// </summary>
        /// <param name="key"> Key for a resource </param>
        /// <param name="value"> Value of resource </param>
        public void WriteSetting<T>(string key, T value) where T : struct;

        /// <summary>
        /// Writes string value to a specific local settings key
        /// </summary>
        /// <param name="key"> Key for a resource </param>
        /// <param name="value"> Value of resource </param>
        public void WriteSetting(string key, string value);

        /// <summary>
        /// Reads string value from local settings
        /// </summary>
        /// <param name="key"> Key for a resource </param>
        /// <returns> Null is there is not resize for provided key </returns>
        public string? ReadString(string key);

        /// <summary>
        /// Reads bool value from local settings
        /// </summary>
        /// <param name="key"> Key for a resource </param>
        /// <returns> Null is there is not resize for provided key </returns>
        public bool? ReadBool(string key);

        /// <summary>
        /// Reads enum value from local settings
        /// </summary>
        /// <typeparam name="TEnum"> Type of enum to read </typeparam>
        /// <param name="key"> Key for a resource </param>
        /// <returns> Null is there is not resize for provided key </returns>
        public TEnum? ReadEnum<TEnum>(string key) where TEnum : struct, Enum;

        /// <summary>
        /// Gets explorer options from local settings
        /// </summary>
        /// <returns> Explorer options that system uses </returns>
        public SettingsExplorerModel GetExplorerSettings();

        /// <summary>
        /// Saves explorer options to a local settings
        /// </summary>
        /// <param name="settings"> Settings to save </param>
        public void SaveExplorerSettings(SettingsExplorerModel settings);

        /// <summary>
        /// Gets all user preferences from local settings 
        /// </summary>
        /// <returns> Model that represents user preferences </returns>
        public SettingsPreferencesModel GetUserPreferences();

        /// <summary>
        /// Saves provided preferences to local settings
        /// </summary>
        /// <param name="preferences"> Preferences that are saved to settings </param>
        public void SaveUserPreferences(SettingsPreferencesModel preferences);
    }
}
