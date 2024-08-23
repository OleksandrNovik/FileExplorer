#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Ranges;
using Models.Storage.Additional;
using System;

namespace Models.General
{
    /// <summary>
    /// Model that stores all necessary data to initiate catalog search
    /// </summary>
    public sealed class SearchFilter : ObservableObject
    {
        public static SearchFilter Default => new()
        {
            IsNestedSearch = true,
            AccessDateChecker = RangeChecker<DateTime>.CreateForAnyValue(),
            ExtensionFilter = _ => true,
            SearchPattern = "*",
            SearchName = null,
            OriginalSearchQuery = "",
            SizeChecker = RangeChecker<ByteSize>.CreateForAnyValue()
        };

        /// <summary>
        /// Is search on a top level (false) or all levels (true)
        /// </summary>
        public bool IsNestedSearch { get; set; }

        /// <summary>
        /// Range that LastAccessDate of file needs to be in to satisfy search request 
        /// </summary>
        public RangeChecker<DateTime> AccessDateChecker { get; set; }

        public RangeChecker<ByteSize> SizeChecker { get; set; }

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
        /// ContainsPattern original search query that was written in search field 
        /// </summary>
        public string OriginalSearchQuery { get; set; }
    }
}
