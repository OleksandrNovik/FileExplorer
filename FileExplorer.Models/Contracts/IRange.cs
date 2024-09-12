using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Contracts;

public interface IRange<T> where T : IComparable<T>
{
    public T Start { get; }
    public T End { get; }

    public bool Satisfies(T value, ExcludingOptions options);
}