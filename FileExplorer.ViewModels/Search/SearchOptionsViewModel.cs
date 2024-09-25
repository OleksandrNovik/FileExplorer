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
            Options = SearchFilter.Default;

            IsNestedSearch = Options.IsNestedSearch;

            Messenger.Register<SearchOptionsViewModel, StopSearchMessage>(this,
                (_, message) =>
                {
                    IsSearchRunning = false;
                });
        }
        public IEnumerable<MenuFlyoutItemViewModel> DateSearchOptions => menuBuilder.Build<DateTime>(SetDateOptionCommand);
        public IEnumerable<MenuFlyoutItemViewModel> SizeSearchOptions => menuBuilder.Build<ByteSize>(SetSizeOptionCommand);

        [ObservableProperty]
        private string searchQuery;
        public SearchFilter Options { get; private set; }

        [ObservableProperty]
        private bool isNestedSearch;

        [ObservableProperty]
        private bool isSearchRunning;

        [RelayCommand]
        private void SetDateOption(RangeChecker<DateTime> checker)
        {
            Options.AccessDateChecker = checker;
            StopSearch();
        }

        [RelayCommand]
        private void SetSizeOption(RangeChecker<ByteSize> checker)
        {
            Options.SizeChecker = checker;
            StopSearch();
        }

        [RelayCommand]
        private void SetTypeOption(Predicate<string> extensionFilter)
        {
            Options.ExtensionFilter = extensionFilter;
            StopSearch();
        }

        [RelayCommand]
        private void ResetOptions()
        {
            Options = SearchFilter.Default;
            IsNestedSearch = Options.IsNestedSearch;
            StopSearch();
        }

        [RelayCommand]
        private void InitiateSearch()
        {
            ExtractQueryString();
            Options.IsNestedSearch = IsNestedSearch;
            Messenger.Send(new SearchOperationRequiredMessage(Options));
            IsSearchRunning = true;
            //TODO: React to a search completed message
        }

        private void ExtractQueryString()
        {
            Options.OriginalSearchQuery = SearchQuery;
            Options.SearchPattern = PathHelper.CreatePattern(SearchQuery);

            // If we cannot generate more precise pattern we should search by name
            if (Options.SearchPattern == "*")
            {
                Options.SearchName = SearchQuery;
            }
            else
            {
                Options.SearchName = null;
            }
        }

        partial void OnIsNestedSearchChanging(bool value)
        {
            StopSearch();
        }

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
