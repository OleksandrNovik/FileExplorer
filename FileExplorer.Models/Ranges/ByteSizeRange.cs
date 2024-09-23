#nullable enable
using FileExplorer.Models.Contracts;
using FileExplorer.Models.Enums;
using FileExplorer.Models.Storage.Additional;
using System;

namespace FileExplorer.Models.Ranges
{
    public sealed class ByteSizeRange : IRange<ByteSize>
    {
        /// <summary>
        /// Defines range of empty files
        /// </summary>
        public static ByteSizeRange Empty => new(ByteSize.Empty, ByteSize.Empty);

        /// <summary>
        /// Defines range of tiny files
        /// </summary>
        public static ByteSizeRange Tiny => new(Empty.End, new ByteSize(16, ByteUnits.KBytes));

        /// <summary>
        /// Defines range of small files
        /// </summary>
        public static ByteSizeRange Small => new(Tiny.End, new ByteSize(1, ByteUnits.MBytes));

        /// <summary>
        /// Defines range of medium files
        /// </summary>
        public static ByteSizeRange Medium => new(Small.End, new ByteSize(128, ByteUnits.MBytes));

        /// <summary>
        /// Defines range of large files
        /// </summary>
        public static ByteSizeRange Large => new(Medium.End, new ByteSize(1, ByteUnits.GBytes));

        /// <summary>
        /// Defines range of huge files
        /// </summary>
        public static ByteSizeRange Huge => new(Large.End, new ByteSize(4, ByteUnits.GBytes));

        /// <summary>
        /// Defines range of giant files
        /// </summary>
        public static ByteSizeRange Giant => new(Huge.End, Huge.End);

        /// <inheritdoc />
        public ByteSize Start { get; }

        /// <inheritdoc />
        public ByteSize End { get; }

        public ByteSizeRange(ByteSize start, ByteSize end)
        {
            Start = start;
            End = end;
        }

        /// <inheritdoc />
        public bool Satisfies(ByteSize? value, ExcludingOptions options)
        {
            return options switch
            {
                ExcludingOptions.Less => value <= Start,
                ExcludingOptions.More => value > End,
                ExcludingOptions.Within => value >= Start && value <= End,
                _ => throw new ArgumentException("Invalid excluding options", nameof(options))
            };
        }
    }
}
