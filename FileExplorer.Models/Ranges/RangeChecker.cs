#nullable enable
using FileExplorer.Models.Contracts;
using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Ranges
{
    public class RangeChecker<T> where T : IComparable<T>
    {
        private readonly IRange<T> range;
        private readonly ExcludingOptions options;
        private Predicate<T?> predicate;

        /// <summary>
        /// Creates checker that accepts any value provided for checking 
        /// </summary>
        public static RangeChecker<T> CreateForAnyValue()
        {
            return new RangeChecker<T>();
        }

        private RangeChecker()
        {
            predicate = _ => true;
        }

        public RangeChecker(IRange<T> range, ExcludingOptions options)
        {
            this.range = range;
            this.options = options;
            predicate = value => range.Satisfies(value, options);
        }

        public bool Satisfies(T? value)
        {
            return predicate(value);
        }

        public override string ToString()
        {
            return options switch
            {
                ExcludingOptions.Less => $"({range.Start})",
                ExcludingOptions.More => $"(> {range.End})",
                ExcludingOptions.Within => $"({range.Start} - {range.End})",
                _ => string.Empty
            };
        }
    }
}
