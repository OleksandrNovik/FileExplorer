using CommunityToolkit.Mvvm.Input;
using FileExplorer.Models;
using FileExplorer.Models.Enums;
using FileExplorer.Models.Ranges;
using System;
using System.Collections.Generic;

namespace FileExplorer.Core.Services.Factories.SearchProperties
{
    /// <summary>
    /// Factory for <see cref="DateTime" /> property of search options
    /// </summary>
    public sealed class DateMenuBuilder : BasePropertyBuilder<DateTime>
    {
        /// <inheritdoc />
        protected override IEnumerable<MenuFlyoutItemViewModel> CompleteMenu(IRelayCommand command)
        {
            return
            [
                new MenuFlyoutItemViewModel("Today")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.TodayRange, ExcludingOptions.Within)
                },
                new MenuFlyoutItemViewModel("Yesterday")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.YesterdayRange, ExcludingOptions.Within)
                },
                new MenuFlyoutItemViewModel("This week")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.ThisWeekRange, ExcludingOptions.Within)
                },
                new MenuFlyoutItemViewModel("Last week")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.LastWeekRange, ExcludingOptions.Within)
                },
                new MenuFlyoutItemViewModel("This month")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.ThisMonthRange, ExcludingOptions.Within)
                },
                new MenuFlyoutItemViewModel("Last month")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.LastMonthRange, ExcludingOptions.Within),
                },
                new MenuFlyoutItemViewModel("This year")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.ThisYearRange, ExcludingOptions.Within),
                },
                new MenuFlyoutItemViewModel("Last year")
                {
                    Command = command,
                    CommandParameter = new RangeChecker<DateTime>(DateRange.LastYearRange, ExcludingOptions.Within),
                }
            ];
        }
    }
}
