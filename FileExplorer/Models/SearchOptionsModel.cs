using CommunityToolkit.Mvvm.ComponentModel;

namespace FileExplorer.Models
{
    public sealed partial class SearchOptionsModel : ObservableObject
    {
        public static readonly SearchOptionsModel Default = new SearchOptionsModel();

        [ObservableProperty]
        private bool isNestedSearch;
    }
}
