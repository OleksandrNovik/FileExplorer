#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Models.Ranges;
using FileExplorer.Models.Storage.Additional;
using System;

namespace FileExplorer.Models.General
{
    /// <summary>
    /// Model that stores all necessary data to initiate catalog search
    /// </summary>
    public sealed class SearchFilter : ObservableObject
    {
        public static SearchFilter Default => new()
        {
            IsNestedSearch = true,
            AccessDateChecker = PredicateChecker<DateTime>.Any,
            ExtensionFilter = PredicateChecker<string>.Any,
            SearchPattern = "*",
            SearchName = null,
            OriginalSearchQuery = "",
            SizeChecker = PredicateChecker<ByteSize>.Any
        };

        /// <summary>
        /// Is search on a top level (false) or all levels (true)
        /// </summary>
        public bool IsNestedSearch { get; set; }

        /// <summary>
        /// Range that LastAccessDate of file needs to be in to satisfy search request 
        /// </summary>
        public PredicateChecker<DateTime> AccessDateChecker { get; set; }

        /// <summary>
        /// Checker for size of directory item
        /// </summary>
        public PredicateChecker<ByteSize> SizeChecker { get; set; }

        /// <summary>
        /// Checker that filters out any files that does not have searched extension
        /// </summary>
        public PredicateChecker<string> ExtensionFilter { get; set; }

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
