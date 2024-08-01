#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Ranges;
using System;

namespace Models
{
    public sealed partial class SearchOptionsModel : ObservableObject
    {
        public static readonly SearchOptionsModel Default = new()
        {
            IsNestedSearch = true,
            AccessDateRange = DateRange.Any,
            ExtensionFilter = _ => true,
            SearchPattern = "*",
            SearchName = null
        };

        [ObservableProperty]
        private bool isNestedSearch;
        public DateRange AccessDateRange { get; set; }
        public Predicate<string> ExtensionFilter { get; set; }
        public string SearchPattern { get; set; }
        public string? SearchName { get; set; }
    }
}
