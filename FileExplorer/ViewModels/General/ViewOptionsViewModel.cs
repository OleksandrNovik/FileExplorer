using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Core.Contracts.Settings;
using Helpers.Application;
using Models.Messages;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class ViewOptionsViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ViewOptions value;

        private readonly ILocalSettingsService localSettings;

        public ViewOptionsViewModel()
        {
            localSettings = App.GetService<ILocalSettingsService>();

            var settingsValue = localSettings.ReadEnum<ViewOptions>(LocalSettings.Keys.ViewOptions);
            value = settingsValue ?? ViewOptions.GridView;
        }

        [RelayCommand]
        private void SetViewOptions(int viewOptions)
        {
            Value = (ViewOptions)viewOptions;
        }
    }
}
