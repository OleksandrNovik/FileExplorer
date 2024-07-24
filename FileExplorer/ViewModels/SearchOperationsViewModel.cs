using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Models;

namespace FileExplorer.ViewModels
{
    public sealed partial class SearchOperationsViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private string searchQuery;

        public SearchOptionsModel Options { get; private set; } = SearchOptionsModel.Default;

        private bool CanExecuteSearch() => !string.IsNullOrWhiteSpace(SearchQuery);

        [RelayCommand(CanExecute = nameof(CanExecuteSearch))]
        private void SearchItemsAsync()
        {

        }
    }
}
