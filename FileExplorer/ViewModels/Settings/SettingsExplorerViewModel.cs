using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Models.Settings;
using FileExplorer.Views.Settings.Pages;

namespace FileExplorer.ViewModels.Settings
{
    /// <summary>
    /// View model for <see cref="SettingsExplorerPage"/>
    /// </summary>
    public sealed class SettingsExplorerViewModel : ObservableObject, INavigationAware
    {
        /// <summary>
        /// Service that gets all necessarily properties from local settings
        /// </summary>
        private readonly ILocalSettingsService localSettings;

        /// <summary>
        /// Container for settings properties
        /// </summary>
        public SettingsExplorerModel PageSettings { get; }

        public SettingsExplorerViewModel(ILocalSettingsService settingsService)
        {
            localSettings = settingsService;
            PageSettings = localSettings.GetExplorerSettings();
        }

        public void OnNavigatedTo(object parameter) { }

        /// <summary>
        /// When page is navigated from settings are being saved
        /// </summary>
        public void OnNavigatedFrom()
        {
            localSettings.SaveExplorerSettings(PageSettings);
        }
    }
}
