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
    public sealed partial class SearchOperationsViewModel : ObservableRecipient
    {
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
            new MenuFlyoutItemViewModel("Last year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastYearRange,
            },

        };

        [ObservableProperty]
        private string searchQuery;
        public SearchOptionsModel Options => SearchOptionsModel.Default;

        [RelayCommand]
        private void SetDateOption(DateRange range)
        {
            Options.AccessDateRange = range;
        }

        [RelayCommand]
        private void SetTypeOption(Predicate<string> extensionFilter)
        {
            Options.ExtensionFilter = extensionFilter;
        }

        [RelayCommand]
        private void InitiateSearch()
        {
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

            Messenger.Send(new SearchOperationRequiredMessage(Options));
        }
    }
}
