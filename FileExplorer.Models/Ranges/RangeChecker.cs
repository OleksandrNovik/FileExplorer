#nullable enable
using FileExplorer.Models.Contracts.Ranges;
using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Ranges
{
    /// <summary>
    /// Class that checks values for satisfying ranges and excluding conditions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RangeChecker<T> : PredicateChecker<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Range that is checked each time
        /// </summary>
        private readonly IRange<T> range;

        /// <summary>
        /// Can check within range, outer start of range or end of the range
        /// See <see cref="ExcludingOptions" /> for more details
        /// </summary>
        private readonly ExcludingOptions options;

        /// <summary>
        /// Creates checker that runs predicate to check if item satisfies conditions
        /// </summary>
        /// <param name="range"> Range that is checked </param>
        /// <param name="options"> Excluding options that is provided for range each check </param>
        public RangeChecker(IRange<T> range, ExcludingOptions options)
            : base(value => range.Satisfies(value, options))
        {
            this.range = range;
            this.options = options;
        }

        /// <summary>
        /// Returns special string description for a checker
        /// </summary>
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
