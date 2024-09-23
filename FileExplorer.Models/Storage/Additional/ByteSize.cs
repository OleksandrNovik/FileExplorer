using FileExplorer.Models.Enums;
using System;

namespace FileExplorer.Models.Storage.Additional
{
    /// <summary>
    /// Represents size of any storage item
    /// </summary>
    public class ByteSize : IComparable<ByteSize>
    {
        /// <summary>
        /// List of sizes available for items to have
        /// </summary>
        public static readonly string[] Sizes =
        [
            "Bytes", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "RB", "QB"
        ];

        public static ByteSize Empty => new(0);

        /// <summary>
        /// Real size of item (without its size value)
        /// </summary>
        private long realSize;

        /// <summary>
        /// Size value of item (can be KB, GB etc.)
        /// </summary>
        private string sizeName;

        /// <summary>
        /// Size of item in bytes
        /// </summary>
        public long InBytes { get; }

        /// <summary>
        /// Creates size from number of bytes it requires to store
        /// Automatically evaluates real size of item
        /// </summary>
        /// <param name="inBytes"> Number of bytes to store item </param>
        public ByteSize(long inBytes)
        {
            InBytes = inBytes;
            Convert(inBytes, ByteUnits.Bytes);
        }

        /// <summary>
        /// Creates size of item from number and value of item's size
        /// </summary>
        /// <param name="value"> Number of some value that item require to be stored </param>
        /// <param name="units"> Type of item's size value </param>
        public ByteSize(double value, ByteUnits units)
        {
            var power = (int)units;
            InBytes = (long)(value * Math.Pow(1024, power));
            Convert(value, units);
        }

        /// <summary>
        /// Converts provided number and value to maximal possible value and size
        /// </summary>
        /// <param name="value"> Number of value to store item </param>
        /// <param name="units"> Type of units that number is measured </param>
        private void Convert(double value, ByteUnits units)
        {
            double converted = value;
            int order = (int)units;

            while (converted > 1024)
            {
                order++;
                converted /= 1024;
            }

            realSize = (long)Math.Floor(converted);
            sizeName = Sizes[order];
        }

        public override string ToString()
        {
            return $"{realSize} {sizeName}";
        }

        /// <summary>
        /// Compares two byte sizes by <see cref="InBytes"/> property
        /// </summary>
        /// <param name="other"> Byte size to compare this with </param>
        /// <returns> 0 - items are equal, more 0 - this is bigger, less 0 this is smaller </returns>
        public int CompareTo(ByteSize other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return InBytes.CompareTo(other.InBytes);
        }

        public static bool operator <(ByteSize a, ByteSize b)
        {
            return a is not null && a.CompareTo(b) < 0;
        }

        public static bool operator >(ByteSize a, ByteSize b)
        {
            return a is not null && a.CompareTo(b) > 0;
        }

        public static bool operator ==(ByteSize a, ByteSize b)
        {
            return a is not null && a.CompareTo(b) == 0;
        }

        public static bool operator !=(ByteSize a, ByteSize b)
        {
            return !(a == b);
        }

        public static bool operator >=(ByteSize a, ByteSize b)
        {
            return a > b || a == b;
        }

        public static bool operator <=(ByteSize a, ByteSize b)
        {
            return a < b || a == b;
        }
    }
}
