using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.Messages;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class ViewOptionsViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ViewOptions viewOptions;

        public ViewOptionsViewModel()
        {
            viewOptions = ViewOptions.GridView;
        }

        [RelayCommand]
        private void SetViewOptions(int selectedOption)
        {
            ViewOptions = (ViewOptions)selectedOption;
        }

    }
}
