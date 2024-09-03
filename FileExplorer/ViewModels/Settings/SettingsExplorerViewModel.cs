using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts;
using Models.Settings;

namespace FileExplorer.ViewModels.Settings
{
    public sealed class SettingsExplorerViewModel : ObservableObject, INavigationAware
    {
        public SettingsExplorerModel PageSettings { get; }

        public SettingsExplorerViewModel()
        {
            PageSettings = SettingsExplorerModel.FromSettings();
        }

        public void OnNavigatedTo(object parameter) { }

        public void OnNavigatedFrom()
        {
            PageSettings.SaveSettings();
        }
    }
}
