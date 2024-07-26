using System;

namespace FileExplorer.Contracts;

public interface IRange<T> where T : IComparable<T>
{
    public T Start { get; }
    public T End { get; }

    public bool Includes(T value);
}