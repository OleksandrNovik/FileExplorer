using System;

namespace FileExplorer.Models.Ranges
{
    public sealed class PredicateChecker<TParam>
    {
        public PredicateChecker<TParam> Any => new(_ => true);

        private readonly Predicate<TParam> predicate;
        public PredicateChecker(Predicate<TParam> predicate)
        {
            this.predicate = predicate;
        }

        public bool Check(TParam value) => predicate(value);
    }
}
