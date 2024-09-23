#nullable enable
using FileExplorer.Models.Contracts;
using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Ranges
{
    /// <summary>
    /// Class that checks values for satisfying ranges and excluding conditions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RangeChecker<T> where T : IComparable<T>
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
        /// Predicate that checks item for satisfying ranges conditions
        /// </summary>
        private Predicate<T?> predicate;

        /// <summary>
        /// Creates checker that accepts any value provided for checking 
        /// </summary>
        public static RangeChecker<T> CreateForAnyValue()
        {
            return new RangeChecker<T>();
        }

        /// <summary>
        /// Constructor to create range checker that accepts any value 
        /// </summary>
        private RangeChecker()
        {
            predicate = _ => true;
        }

        /// <summary>
        /// Creates checker that runs predicate to check if item satisfies conditions
        /// </summary>
        /// <param name="range"> Range that is checked </param>
        /// <param name="options"> Excluding options that is provided for range each check </param>
        public RangeChecker(IRange<T> range, ExcludingOptions options)
        {
            this.range = range;
            this.options = options;
            predicate = value => range.Satisfies(value, options);
        }

        /// <summary>
        /// Checks if value satisfies conditions of checker
        /// </summary>
        public bool Satisfies(T? value)
        {
            return predicate(value);
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
