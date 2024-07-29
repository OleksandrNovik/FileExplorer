#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Ranges;
using System;

namespace FileExplorer.Models
{
    public sealed partial class SearchOptionsModel : ObservableObject
    {
        public static readonly SearchOptionsModel Default = new()
        {
            IsNestedSearch = true,
            DateOption = default,
            ExtensionFilter = _ => true,
        };

        [ObservableProperty]
        private bool isNestedSearch;

        public DateRange? DateOption { get; set; }
        public Predicate<string> ExtensionFilter { get; set; }
    }
}
