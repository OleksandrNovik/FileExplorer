using System;

namespace FileExplorer.Models.General
{
    public interface ISortProperty
    {
    }
    public sealed class SortProperty<TItem, TProperty> : ISortProperty
    {
        public SortProperty(Func<TItem, TProperty> func)
        {
            Func = func;
        }
        public Func<TItem, TProperty> Func { get; }
    }
}
