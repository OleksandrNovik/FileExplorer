#nullable enable
using Models.Settings;

namespace FileExplorer.Core.Contracts.Settings
{
    public interface ILocalSettingsService
    {
        /// <summary>
        /// Writes provided value to a specific local setting's key
        /// </summary>
        /// <param name="key"> Key for a resource </param>
        /// <param name="value"> Value of resource </param>
        public void WriteSetting(string key, string value);

        /// <summary>
        /// Reads value from local settings by using provided key
        /// </summary>
        /// <param name="key"> Key for local setting's resource </param>
        /// <returns> Found resource by provided key </returns>
        public string? ReadSetting(string key);

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
