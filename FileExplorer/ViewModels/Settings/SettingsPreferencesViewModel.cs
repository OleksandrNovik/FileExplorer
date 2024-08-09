using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts;
using Helpers.Application;
using Models.Settings;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels.Settings
{
    public sealed partial class SettingsPreferencesViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string selectedTheme;
        public ObservableCollection<string> Themes { get; }
        public ObservableCollection<string> DateFormats { get; }
        public ObservableCollection<string> Languages { get; }
        public SettingsPreferencesModel PageSettings { get; }

        public SettingsPreferencesViewModel()
        {
            Themes = new ObservableCollection<string>(LocalSettings.Themes);
            DateFormats = new ObservableCollection<string>(LocalSettings.DateFormats);
            Languages = ["English"];

            //TODO: Get theme from settings
            PageSettings = SettingsPreferencesModel.FromSettings();
            selectedTheme = PageSettings.Theme.ToString();
        }

        public void OnNavigatedTo(object parameter) { }

        public void OnNavigatedFrom()
        {
            //PageSettings.SaveSettings();
        }
    }
}
