using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Contracts.Ranges;

/// <summary>
/// General interface for any range to fulfill
/// </summary>
/// <typeparam name="T"> Type of range's elements </typeparam>
public interface IRange<T> where T : IComparable<T>
{
    /// <summary>
    /// Starting value of range
    /// </summary>
    public T Start { get; }

    /// <summary>
    /// Final value of range
    /// </summary>
    public T End { get; }

    /// <summary>
    /// Checks if provided value satisfies range
    /// </summary>
    /// <param name="value"> Value to check </param>
    /// <param name="options">Decides if check is run within range or outer boundaries </param>
    /// <returns> True if value satisfies provided conditions </returns>
    public bool Satisfies(T value, ExcludingOptions options);
}