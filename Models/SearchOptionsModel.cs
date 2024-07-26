#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Ranges;

namespace FileExplorer.Models
{
    public sealed partial class SearchOptionsModel : ObservableObject
    {
        public static readonly SearchOptionsModel Default = new()
        {
            IsNestedSearch = true,
            DateOption = default,
        };

        [ObservableProperty]
        private bool isNestedSearch;

        public DateRange? DateOption { get; set; }
    }
}
