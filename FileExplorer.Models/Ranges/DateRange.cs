using FileExplorer.Models.Contracts;
using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Ranges
{
    public sealed class DateRange : IRange<DateTime>
    {
        public static DateRange TodayRange => new(DateTime.Today, DateTime.Today.AddDays(1).AddSeconds(-1));
        public static DateRange YesterdayRange => new(DateTime.Today.AddDays(-1), DateTime.Today.AddSeconds(-1));
        public static DateRange ThisWeekRange
        {
            get
            {
                var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);
                return new DateRange(startOfWeek, endOfWeek);
            }
        }

        public static DateRange LastWeekRange => ThisWeekRange.AddDays(-7);

        public static DateRange ThisMonthRange
        {
            get
            {
                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);
                return new DateRange(startOfMonth, endOfMonth);
            }
        }

        public static DateRange LastMonthRange => ThisMonthRange.AddMonths(-1);

        public static DateRange ThisYearRange
        {
            get
            {
                var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
                var endOfYear = startOfYear.AddYears(1).AddSeconds(-1);
                return new DateRange(startOfYear, endOfYear);
            }
        }

        public static DateRange LastYearRange => ThisYearRange.AddYears(-1);

        public DateTime Start { get; }
        public DateTime End { get; }
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Satisfies(DateTime value, ExcludingOptions options)
        {
            return options switch
            {
                ExcludingOptions.Less => value <= Start,
                ExcludingOptions.More => value > End,
                ExcludingOptions.Within => value >= Start && value <= End,
                _ => throw new ArgumentException("Invalid excluding options", nameof(options))
            };
        }


        public DateRange AddDays(int days)
        {
            return new DateRange(Start.AddDays(days), End.AddDays(days));
        }

        public DateRange AddMonths(int months)
        {
            return new DateRange(Start.AddMonths(months), End.AddMonths(months));
        }

        public DateRange AddYears(int years)
        {
            return new DateRange(Start.AddYears(years), End.AddYears(years));
        }
    }
}
