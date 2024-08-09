using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts;
using System;

namespace Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for explorer page in settings window 
    /// </summary>
    public sealed partial class SettingsExplorerModel : ObservableObject, ISettingsModel
    {
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
            return new SettingsExplorerModel
            {
                ShowHiddenFiles = false,
                HideSystemFiles = false,
                showFileExtensions = false,
            };
        }

        /// <inheritdoc />
        public void SaveSettings()
        {
            throw new NotImplementedException();
        }
    }
}
