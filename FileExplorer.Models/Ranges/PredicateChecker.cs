using System;

namespace FileExplorer.Models.Ranges
{
    public class PredicateChecker<TParam>
    {
        /// <summary>
        /// Predicate that is checked 
        /// </summary>
        protected readonly Predicate<TParam> predicate;
        public PredicateChecker(Predicate<TParam> predicate)
        {
            this.predicate = predicate;
        }

        /// <summary>
        /// Predicate checker that accepts any value
        /// </summary>
        public static PredicateChecker<TParam> Any => new(_ => true);

        /// <summary>
        /// Checks predicate for a provided value and returns if it has passed condition
        /// </summary>
        public bool Satisfies(TParam value) => predicate(value);
    }
}
