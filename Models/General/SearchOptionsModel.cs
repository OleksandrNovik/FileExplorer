#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Ranges;
using System;

namespace Models.General
{
    /// <summary>
    /// Model that stores all necessary data to initiate catalog search
    /// </summary>
    public sealed class SearchOptionsModel : ObservableObject
    {
        public static SearchOptionsModel Default => new()
        {
            IsNestedSearch = true,
            AccessDateRange = DateRange.Any,
            ExtensionFilter = _ => true,
            SearchPattern = "*",
            SearchName = null,
            OriginalSearchQuery = ""
        };

        /// <summary>
        /// Is search on a top level (false) or all levels (true)
        /// </summary>
        public bool IsNestedSearch { get; set; }

        /// <summary>
        /// Range that LastAccessDate of file needs to be in to satisfy search request 
        /// </summary>
        public DateRange AccessDateRange { get; set; }

        /// <summary>
        /// Function that filters out any files that does not have searched extension
        /// </summary>
        public Predicate<string> ExtensionFilter { get; set; }

        /// <summary>
        /// Pattern that file name needs to follow to satisfy search condition 
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// If pattern cannot be created from query, stores query used to search files
        /// </summary>
        public string? SearchName { get; set; }

        /// <summary>
        /// Contains original search query that was written in search field 
        /// </summary>
        public string OriginalSearchQuery { get; set; }
    }
}
