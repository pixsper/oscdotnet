using System;

namespace Pixsper.OscDotNet.MessageTypes
{
    public readonly struct OscColor : IEquatable<OscColor>
    {
        public static int ByteLength = 4;

        public static OscColor FromByteArray(byte[] data)
        {
            return new OscColor(data[0], data[1], data[2], data[3]);
        }

        public OscColor(byte red, byte green, byte blue, byte alpha)
            : this()
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public byte[] ToByteArray()
        {
            return new[] {Red, Green, Blue, Alpha};
        }

        public byte Red { get; }

        public byte Green { get; }

        public byte Blue { get; }

        public byte Alpha { get; }


        public bool Equals(OscColor other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue && Alpha == other.Alpha;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OscColor other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Red.GetHashCode();
                hashCode = (hashCode * 397) ^ Green.GetHashCode();
                hashCode = (hashCode * 397) ^ Blue.GetHashCode();
                hashCode = (hashCode * 397) ^ Alpha.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(OscColor a, OscColor b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(OscColor a, OscColor b)
        {
            return !(a == b);
        }
    }
}