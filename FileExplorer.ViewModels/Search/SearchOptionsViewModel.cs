#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Models;
using FileExplorer.Models.Enums;
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
        public SearchOptionsViewModel()
        {
            Options = SearchFilter.Default;

            IsNestedSearch = Options.IsNestedSearch;

            Messenger.Register<SearchOptionsViewModel, StopSearchMessage>(this,
                (_, message) =>
                {
                    IsSearchRunning = false;
                });
        }
        public IEnumerable<MenuFlyoutItemViewModel> DateSearchOptions => new List<MenuFlyoutItemViewModel>
        {
            new MenuFlyoutItemViewModel("Any")
            {
                Command = SetDateOptionCommand,
                CommandParameter = RangeChecker<DateTime>.CreateForAnyValue()
            },
            new MenuFlyoutItemViewModel("Today")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.TodayRange, ExcludingOptions.Within)
            },
            new MenuFlyoutItemViewModel("Yesterday")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.YesterdayRange, ExcludingOptions.Within)
            },
            new MenuFlyoutItemViewModel("This week")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.ThisWeekRange, ExcludingOptions.Within)
            },
            new MenuFlyoutItemViewModel("Last week")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.LastWeekRange, ExcludingOptions.Within)
            },
            new MenuFlyoutItemViewModel("This month")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.ThisMonthRange, ExcludingOptions.Within)
            },
            new MenuFlyoutItemViewModel("Last month")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.LastMonthRange, ExcludingOptions.Within),
            },
            new MenuFlyoutItemViewModel("This year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.ThisYearRange, ExcludingOptions.Within),
            },
            new MenuFlyoutItemViewModel("Last year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = new RangeChecker<DateTime>(DateRange.LastYearRange, ExcludingOptions.Within),
            },
        };
        public IEnumerable<MenuFlyoutItemViewModel> SizeSearchOptions => new List<MenuFlyoutItemViewModel>
        {
            new MenuFlyoutItemViewModel("Any")
            {
                Command = SetSizeOptionCommand,
                CommandParameter = RangeChecker<ByteSize>.CreateForAnyValue()
            },
            new MenuFlyoutItemViewModel("Empty ",
                new RangeChecker<ByteSize>(ByteSizeRange.Empty, ExcludingOptions.Less))
            {
                Command = SetSizeOptionCommand,
            },
            new MenuFlyoutItemViewModel("Tiny ",
                new RangeChecker<ByteSize>(ByteSizeRange.Tiny, ExcludingOptions.Within))
            {
                Command = SetSizeOptionCommand,
            },

            new MenuFlyoutItemViewModel("Tiny ",
                new RangeChecker<ByteSize>(ByteSizeRange.Small, ExcludingOptions.Within))
            {
                Command = SetSizeOptionCommand,
            },

            new MenuFlyoutItemViewModel("Medium ",
                new RangeChecker<ByteSize>(ByteSizeRange.Medium, ExcludingOptions.Within))
            {
                Command = SetSizeOptionCommand,
            },

            new MenuFlyoutItemViewModel("Large ",
                new RangeChecker<ByteSize>(ByteSizeRange.Large, ExcludingOptions.Within))
            {
                Command = SetSizeOptionCommand,
            },

            new MenuFlyoutItemViewModel("Huge ",
                new RangeChecker<ByteSize>(ByteSizeRange.Huge, ExcludingOptions.Within))
            {
                Command = SetSizeOptionCommand,
            },
            new MenuFlyoutItemViewModel("Giant ",
                new RangeChecker<ByteSize>(ByteSizeRange.Giant, ExcludingOptions.More))
            {
                Command = SetSizeOptionCommand,
            },
        };

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
