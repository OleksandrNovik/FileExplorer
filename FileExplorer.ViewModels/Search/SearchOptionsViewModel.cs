#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts.Factories.SearchProperties;
using FileExplorer.Models;
using FileExplorer.Models.General;
using FileExplorer.Models.Messages;
using FileExplorer.Models.Ranges;
using FileExplorer.Models.Storage.Additional;
using System;
using System.Collections.Generic;
using PathHelper = FileExplorer.Helpers.StorageHelpers.PathHelper;

namespace FileExplorer.ViewModels.Search
{
    public sealed partial class SearchOptionsViewModel : ObservableRecipient
    {
        /// <summary>
        /// Builder for search filter menu options
        /// </summary>
        private readonly ISearchOptionsMenuBuilder menuBuilder;

        public SearchOptionsViewModel(ISearchOptionsMenuBuilder menuBuilder)
        {
            this.menuBuilder = menuBuilder;
            Filter = SearchFilter.Default;

            IsNestedSearch = Filter.IsNestedSearch;

            Messenger.Register<SearchOptionsViewModel, StopSearchMessage>(this,
                (_, message) =>
                {
                    IsSearchRunning = false;
                });
        }

        /// <summary>
        /// Filter options for date
        /// </summary>
        public IEnumerable<MenuFlyoutItemViewModel> DateSearchOptions => menuBuilder.Build<DateTime>(SetDateOptionCommand);

        /// <summary>
        /// Filter options for size
        /// </summary>
        public IEnumerable<MenuFlyoutItemViewModel> SizeSearchOptions => menuBuilder.Build<ByteSize>(SetSizeOptionCommand);

        /// <summary>
        /// Filter options for type
        /// </summary>
        public IEnumerable<MenuFlyoutItemViewModel> FileTypeOptions => menuBuilder.Build<string>(SetTypeOptionCommand);

        /// <summary>
        /// Current search query
        /// </summary>
        [ObservableProperty]
        private string searchQuery;

        /// <summary>
        /// Current search filter
        /// </summary>
        public SearchFilter Filter { get; private set; }

        /// <summary>
        /// Is search run through subdirectories
        /// </summary>
        [ObservableProperty]
        private bool isNestedSearch;

        /// <summary>
        /// Is search currently running
        /// </summary>
        [ObservableProperty]
        private bool isSearchRunning;

        /// <summary>
        /// Sets value for file's access date checker
        /// </summary>
        [RelayCommand]
        private void SetDateOption(PredicateChecker<DateTime> checker)
        {
            Filter.AccessDateChecker = checker;
            StopSearch();
        }

        /// <summary>
        /// Sets value for file's size checker
        /// </summary>
        [RelayCommand]
        private void SetSizeOption(PredicateChecker<ByteSize> checker)
        {
            Filter.SizeChecker = checker;
            StopSearch();
        }

        /// <summary>
        /// Sets value for file's type checker
        /// </summary>
        [RelayCommand]
        private void SetTypeOption(PredicateChecker<string> checker)
        {
            Filter.ExtensionFilter = checker;
            StopSearch();
        }

        /// <summary>
        /// Resets search filter
        /// </summary>
        [RelayCommand]
        private void ResetOptions()
        {
            Filter = SearchFilter.Default;
            IsNestedSearch = Filter.IsNestedSearch;
            StopSearch();
        }

        /// <summary>
        /// Initiates new search with currently selected options
        /// </summary>
        [RelayCommand]
        private void InitiateSearch()
        {
            ExtractQueryString();
            Filter.IsNestedSearch = IsNestedSearch;
            Messenger.Send(new SearchOperationRequiredMessage(Filter));
            IsSearchRunning = true;
            //TODO: React to a search completed message
        }


        private void ExtractQueryString()
        {
            Filter.OriginalSearchQuery = SearchQuery;
            Filter.SearchPattern = PathHelper.CreatePattern(SearchQuery);

            // If we cannot generate more precise pattern we should search by name
            if (Filter.SearchPattern == "*")
            {
                Filter.SearchName = SearchQuery;
            }
            else
            {
                Filter.SearchName = null;
            }
        }

        /// <summary>
        /// If nested search property is changing stops currently running search
        /// </summary>
        partial void OnIsNestedSearchChanging(bool value)
        {
            StopSearch();
        }

        /// <summary>
        /// Stop search if it is running
        /// </summary>
        [RelayCommand]
        private void StopSearch()
        {
            if (IsSearchRunning)
            {
                Messenger.Send(new StopSearchMessage());
            }
        }
    }
}
