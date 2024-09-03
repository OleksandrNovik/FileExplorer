using CommunityToolkit.Mvvm.ComponentModel;
using Helpers.Application;
using Helpers.General;
using Models.Contracts;

namespace Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for explorer page in settings window 
    /// </summary>
    public sealed partial class SettingsExplorerModel : ObservableObject, ISettingsModel
    {
        public static SettingsExplorerModel Default = new()
        {
            ShowHiddenFiles = false,
            HideSystemFiles = true,
            ShowFileExtensions = false,
        };

        private SettingsExplorerModel() { }

        [ObservableProperty]
        private bool showFileExtensions;

        [ObservableProperty]
        private bool hideSystemFiles;

        [ObservableProperty]
        private bool showHiddenFiles;

        /// <summary>
        /// Creates a model that contains values from settings
        /// </summary>
        public static SettingsExplorerModel FromSettings()
        {
            var showHidden = LocalSettings.ReadSetting(LocalSettings.Keys.ShowHiddenFiles);
            var hideSystem = LocalSettings.ReadSetting(LocalSettings.Keys.HideSystemFiles);
            var showExtensions = LocalSettings.ReadSetting(LocalSettings.Keys.ShowFileExtensions);

            return new SettingsExplorerModel
            {
                ShowHiddenFiles = showHidden.ParseBool(Default.ShowHiddenFiles),
                HideSystemFiles = hideSystem.ParseBool(Default.HideSystemFiles),
                ShowFileExtensions = showExtensions.ParseBool(Default.ShowFileExtensions),
            };
        }

        /// <inheritdoc />
        public void SaveSettings()
        {
            LocalSettings.WriteSetting(LocalSettings.Keys.ShowHiddenFiles, ShowHiddenFiles.ToString());
            LocalSettings.WriteSetting(LocalSettings.Keys.HideSystemFiles, HideSystemFiles.ToString());
            LocalSettings.WriteSetting(LocalSettings.Keys.ShowFileExtensions, ShowFileExtensions.ToString());
        }
    }
}
