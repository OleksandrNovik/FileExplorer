using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Models.Contracts;
using System;

namespace Models.Settings
{
    /// <summary>
    /// Model that stores and saves settings for preferences page in settings window 
    /// </summary>
    public sealed partial class SettingsPreferencesModel : ObservableObject, ISettingsModel
    {
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
            //TODO: get from settings
            return new SettingsPreferencesModel
            {
                Theme = ElementTheme.Default,
                ShowConfirmationMessage = false,
                OpenFolderInNewTab = false,
                Language = "English",
            };
        }

        /// <inheritdoc />
        public void SaveSettings()
        {
            throw new NotImplementedException();
        }
    }
}
