using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Views.Settings.Pages;
using Helpers.Application;
using Models.Settings;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels.Settings
{
    /// <summary>
    /// View model for <see cref="SettingsPreferencesPage"/>
    /// </summary>
    public sealed partial class SettingsPreferencesViewModel : ObservableObject, INavigationAware
    {
        /// <summary>
        /// Service that gets all necessarily properties from local settings
        /// </summary>
        private readonly ILocalSettingsService localSettings;

        /// <summary>
        /// Property to store selected theme from dropdown
        /// </summary>
        [ObservableProperty]
        private string selectedTheme;

        /// <summary>
        /// Available themes
        /// </summary>
        public ObservableCollection<string> Themes { get; }

        /// <summary>
        /// Available date formats
        /// </summary>
        public ObservableCollection<string> DateFormats { get; }

        /// <summary>
        /// Available languages
        /// </summary>
        public ObservableCollection<string> Languages { get; }

        /// <summary>
        /// Container for settings properties
        /// </summary>
        public SettingsPreferencesModel PageSettings { get; }

        public SettingsPreferencesViewModel(ILocalSettingsService settingsService)
        {
            localSettings = settingsService;

            Themes = new ObservableCollection<string>(LocalSettings.Themes);
            DateFormats = new ObservableCollection<string>(LocalSettings.DateFormats);
            Languages = ["English", "Other"];

            PageSettings = localSettings.GetUserPreferences();
            selectedTheme = PageSettings.Theme.ToString();
        }

        public void OnNavigatedTo(object parameter) { }

        /// <summary>
        /// When page is navigated from settings are being saved
        /// </summary>
        public void OnNavigatedFrom()
        {
            localSettings.SaveUserPreferences(PageSettings);
        }
    }
}
