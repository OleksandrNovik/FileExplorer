#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;
using Models.General;
using Models.Messages;
using Models.Ranges;
using System;
using System.Collections.Generic;
using PathHelper = Helpers.StorageHelpers.PathHelper;

namespace FileExplorer.ViewModels
{
    public sealed partial class SearchOptionsViewModel : ObservableRecipient
    {
        public SearchOptionsViewModel()
        {
            Options = SearchOptionsModel.Default;

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
                CommandParameter = DateRange.Any
            },
            new MenuFlyoutItemViewModel("Today")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.TodayRange
            },
            new MenuFlyoutItemViewModel("Yesterday")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.YesterdayRange
            },
            new MenuFlyoutItemViewModel("This week")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.ThisWeekRange
            },
            new MenuFlyoutItemViewModel("Last week")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastWeekRange
            },
            new MenuFlyoutItemViewModel("This month")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.ThisMonthRange
            },
            new MenuFlyoutItemViewModel("Last month")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastMonthRange,
            },
            new MenuFlyoutItemViewModel("This year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.ThisYearRange,
            },
            new MenuFlyoutItemViewModel("Last year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastYearRange,
            },
        };
        public IEnumerable<MenuFlyoutItemViewModel> SizeSearchOptions => new List<MenuFlyoutItemViewModel>
        {
            new MenuFlyoutItemViewModel("11111")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastYearRange,
            },

        };

        [ObservableProperty]
        private string searchQuery;

        public SearchOptionsModel Options { get; }

        [ObservableProperty]
        private bool isNestedSearch;

        [ObservableProperty]
        private bool isSearchRunning;

        [RelayCommand]
        private void SetDateOption(DateRange range)
        {
            Options.AccessDateRange = range;
            StopSearch();
        }

        [RelayCommand]
        private void SetTypeOption(Predicate<string> extensionFilter)
        {
            Options.ExtensionFilter = extensionFilter;
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
