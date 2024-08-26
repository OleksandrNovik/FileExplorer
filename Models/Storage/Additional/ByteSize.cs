using System;

namespace Models.Storage.Additional
{
    public enum ByteUnits
    {
        Bytes, KBytes, MBytes, GBytes, TBytes, PBytes, EBytes, ZBytes, YBytes, RBytes, QBytes
    }
    public class ByteSize : IComparable<ByteSize>
    {
        public static readonly string[] Sizes =
        [
            "Bytes", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "RB", "QB"
        ];

        public static ByteSize Empty => new(0);

        private string fruendlyValue;
        public long InBytes { get; }

        public ByteSize(long inBytes)
        {
            InBytes = inBytes;
            fruendlyValue = Convert(inBytes, ByteUnits.Bytes);
        }

        public ByteSize(double value, ByteUnits units)
        {
            var power = (int)units;
            InBytes = (long)(value * Math.Pow(1024, power));
            fruendlyValue = Convert(value, units);
        }

        private string Convert(double value, ByteUnits units)
        {
            double converted = value;
            int order = (int)units;

            while (converted > 1024)
            {
                order++;
                converted /= 1024;
            }

            return $"{Math.Floor(converted)} {Sizes[order]}";
        }

        public override string ToString()
        {
            return fruendlyValue;
        }


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
