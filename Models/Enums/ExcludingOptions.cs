using Models.Contracts;

namespace Models.Enums;

/// <summary>
/// Contains excluding options to compare value within <see cref="IRange{T}"/>
/// </summary>
public enum ExcludingOptions
{
    /// <summary>
    /// Not provided
    /// </summary>
    None,

    /// <summary>
    /// Less than minimum 
    /// </summary>
    Less,

    /// <summary>
    /// More that maximum
    /// </summary>
    More,

    /// <summary>
    /// Within range
    /// </summary>
    Within
}
